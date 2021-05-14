import torch
import torch.nn as nn
import torch.nn.functional as F
from torch.utils.tensorboard import SummaryWriter

import copy 
from tqdm import tqdm
import os
import json
import numpy as np

from utils import Config
from model import Regressor, SVMRegressor, Regressorv2
from data import CSVDataset

def compute_r2(y, y_pred):
    y_copy = y.detach().cpu().numpy()
    y_pred_copy = y_pred.detach().cpu().numpy()
    correlation_matrix = np.corrcoef(y_copy, y_pred_copy)
    correlation_xy = correlation_matrix[0,1]
    r_squared = correlation_xy**2
    return r_squared

def r2_score(target, prediction):
    """Calculates the r2 score of the model
    
    Args-
        target- Actual values of the target variable
        prediction- Predicted values, calculated using the model
        
    Returns- 
        r2- r-squared score of the model
    """
    with torch.set_grad_enabled(False):
        r2 = 1- torch.sum((target-prediction)**2) / torch.sum((target-target.float().mean())**2)
    return r2.item()

def train(dataloader, dataset_sizes, model, criterion, optimizer, device, num_epochs=Config['num_epochs'], batch_size=Config['batch_size']):

    model.to(device)

    best_loss = 100.0
    best_acc = 0.0
    best_wts = copy.deepcopy(model.state_dict())
    dummy_input = None

    if Config['tensorboard_log']:
        writer = SummaryWriter(Config['model_path'])
    
    for epoch in range(num_epochs):
        for phase in ['train', 'valid']:
            if phase=='train':
                model.train()
            else:
                model.eval()
            
            running_loss = 0.0
            running_acc = 0.0
            running_total = 0

            for inputs, labels in tqdm(dataloader[phase]):
                with torch.set_grad_enabled(True):

                    optimizer.zero_grad()
                    inputs = inputs.to(device)
                    labels = labels.to(device)

                    if dummy_input is None:
                        dummy_input = inputs[0]

                    outputs = model(inputs)
                    loss = criterion(outputs, labels)
                    # print('Predicted: ', outputs )
                    # print('Expected: ', labels)
                    if Config['use_cuda']:
                        l2_regularization = torch.tensor(0.).cuda()
                    else:
                        l2_regularization = torch.tensor(0.)
                    
                    for param in model.parameters():
                        l2_regularization += torch.norm(param, 2)**2

                    loss += 1e-5 * l2_regularization
                    running_acc += r2_score(labels, outputs)*inputs.size(0)
                    running_total += inputs.size(0)
                    if phase =='train':
                        loss.backward()
                        optimizer.step()
                    running_loss += loss.item()*inputs.size(0)
            epoch_loss = running_loss/running_total
            epoch_acc = running_acc/running_total
            print(f'Epoch {epoch} Loss: {loss:.4f} R2 Score: {epoch_acc}')
            if epoch_acc > best_acc and phase=='valid':
                best_wts = copy.deepcopy(model.state_dict())
            writer.add_scalar(phase+'_loss', epoch_loss, global_step=epoch)
            writer.add_scalar(phase+'_acc', epoch_acc, global_step=epoch)
        if (epoch+1)%5==0:
            torch.onnx.export(model,
                      dummy_input.unsqueeze(0),
                      os.path.join(
                            Config['model_path'],
                            f'checkpoints/model_{epoch}.onnx'
                        ),
                      export_params=True
                      )
            torch.save(
                model.state_dict(),
                os.path.join(
                    Config['model_path'],
                    f'checkpoints/model_{epoch}.pth'
                )
            )
    print('Training ended')
    torch.save(
        best_wts,
        os.path.join(
            Config['model_path'],
            'checkpoints/model_final.pth'
        )
    )
    model.load_state_dict(best_wts)
    torch.onnx.export(model,
                      dummy_input.unsqueeze(0),
                      os.path.join(
                            Config['model_path'],
                            'checkpoints/model_final.onnx'
                      ),
                      export_params=True
                      )


if __name__ == '__main__':
    os.makedirs(Config['model_path'], exist_ok=True)
    os.makedirs(os.path.join(Config['model_path'],'logs'),exist_ok=True)
    os.makedirs(os.path.join(Config['model_path'],'checkpoints'),exist_ok=True)
    
    dataset = CSVDataset(root_path=Config['dataset_path'])
    headers = [
        'headPosx', 
        'headPosy', 
        'headPosz', 
        'headRotx', 
        'headRoty', 
        'headRotz', 
        'headRotQx', 
        'headRotQy', 
        'headRotQz', 
        'headRotQw', 
        'handRPosx', 
        'handRPosy', 
        'handRPosz', 
        'handRRotx', 
        'handRRoty', 
        'handRRotz', 
        'handRRotQx', 
        'handRRotQy', 
        'handRRotQz', 
        'handRRotQw',
        'handLPosx', 
        'handLPosy', 
        'handLPosz', 
        'handLRotx', 
        'handLRoty', 
        'handLRotz', 
        'handLRotQx', 
        'handLRotQy', 
        'handLRotQz', 
        'handLRotQw',
        'tracker1Posx', 
        'tracker1Posy', 
        'tracker1Posz', 
        'tracker1Rotx', 
        'tracker1Roty', 
        'tracker1Rotz', 
        'tracker1RotQx', 
        'tracker1RotQy', 
        'tracker1RotQz', 
        'tracker1RotQw',
        'relativeHandRPosx', 
        'relativeHandRPosy', 
        'relativeHandRPosz', 
        'relativeHandLPosx',
        'relativeHandLPosy',
        'relativeHandLPosz',
        'relativeTracker1Posx',
        'relativeTracker1Posy',
        'relativeTracker1Posz',
        ]
    with open(os.path.join(Config['model_path'], 'scaler.json'), 'w') as f:
        scaler = {'scalers': []}
        for idx, header in enumerate(headers):
            scaler['scalers'].append({
                'type': header,
                'min': dataset.scaler.min_.tolist()[idx],
                'scale': dataset.scaler.scale_.tolist()[idx],
                'data_min': dataset.scaler.data_min_.tolist()[idx],
                'data_max': dataset.scaler.data_max_.tolist()[idx],
                'data_range': dataset.scaler.data_range_.tolist()[idx],
                'n_samples_seen': dataset.scaler.n_samples_seen_
            })
        json.dump(scaler, f)

    if Config['data_type']=='euler':
        model = Regressor(input_size=18, output_size=6)
    elif Config['data_type']=='quaternion':
        model = Regressor(input_size=21, output_size=7)
    elif Config['data_type']=='both':
        model = Regressor(input_size=30, output_size=10)
    elif Config['data_type']=='relative':
        model = Regressorv2(input_size=19, output_size=7)
    elif Config['data_type']=='relative_svm':
        model = SVMRegressor(input_size=16, output_size=7)

    device = torch.device('cuda:0' if torch.cuda.is_available() and Config['use_cuda'] else 'cpu')

    optimizer = torch.optim.SGD(model.parameters(), lr=Config['lr']) #rmsprop, adam

    criterion = torch.nn.MSELoss(reduction='sum')

    ratio = [int(len(dataset)*0.8), len(dataset)-int(len(dataset)*0.8)]
    train_dataset, valid_dataset = torch.utils.data.random_split(dataset, ratio)

    train_loader = torch.utils.data.DataLoader(
                        train_dataset,
                        batch_size = Config['batch_size'],
                        shuffle=True,
                        num_workers = Config['num_workers']
                    )

    valid_loader = torch.utils.data.DataLoader(
                        valid_dataset,
                        batch_size = Config['batch_size'],
                        shuffle=False,
                        num_workers = Config['num_workers']
                    )
    dataloaders = {
        'train' : train_loader,
        'valid' : valid_loader
    }

    dataset_sizes = {
        'train' : len(train_dataset),
        'valid' : len(valid_dataset)
    }

    train(
        dataloaders, 
        dataset_sizes,
        model,
        criterion,
        optimizer,
        device
        )