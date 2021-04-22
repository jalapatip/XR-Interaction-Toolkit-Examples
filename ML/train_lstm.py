import torch
import torch.nn as nn
import torch.nn.functional as F
from torch.utils.tensorboard import SummaryWriter

import copy 
from tqdm import tqdm
import os
import json

from utils import Config
from model import LSTMRegressorv2
from data import LSTMCSVDataset
from datetime import datetime

def train(dataloader, dataset_sizes, model, criterion, optimizer, device, num_epochs=Config['num_epochs'], batch_size=Config['batch_size']):
    print(Config)

    start_time = datetime.now()
    print("Start Time =", start_time)

    model.to(device)

    best_loss = 100.0
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
            print(dataloader[phase])
            for inputs, labels in tqdm(dataloader[phase]):
                # if inputs.shape[0]!=Config['batch_size']:
                    # continue
                optimizer.zero_grad()
                with torch.set_grad_enabled(phase=='train'):
                    inputs = inputs.to(device)
                    labels = labels.to(device)
                    model.h1= model.init_hidden(batch_size=inputs.shape[0], device='cuda:0')

                    if dummy_input is None:
                        dummy_input = inputs

                    outputs = model(inputs)
                    # print(labels.shape)
                    loss = criterion(outputs, labels[:,-1,:])
                    # return
                    # print('Predicted: ', outputs )
                    # print('Expected: ', labels)
                    if Config['use_cuda']:
                        l2_regularization = torch.tensor(0.).cuda()
                        #print('Using Cuda')
                    else:
                        l2_regularization = torch.tensor(0.)
                    
                    for param in model.parameters():
                        l2_regularization += torch.norm(param, 2)**2

                    loss += 1e-5 * l2_regularization
                    # print(loss)
                    if phase =='train':
                        loss.backward()
                        optimizer.step()
                        model.h1[0].detach_()
                        model.h1[1].detach_()
                        # model.h2[0].detach_()
                        # model.h2[1].detach_()
                    running_loss += loss.item()*inputs.size(0)
            epoch_loss = running_loss/dataset_sizes[phase]
            print(f'Epoch {epoch} Loss: {epoch_loss:.4f}')
            if epoch_loss < best_loss and phase=='valid':
                best_wts = copy.deepcopy(model.state_dict())
            writer.add_scalar(phase+'_loss', epoch_loss, global_step=epoch)
        if (epoch+1)%5==0:
            model.h1 = model.init_hidden(batch_size=1, device='cuda:0')
            torch.onnx.export(model,
                      dummy_input[0].unsqueeze(0),
                      os.path.join(
                            Config['model_path'],
                            f'checkpoints/model_{epoch}.onnx'
                        ),
                      )
            torch.save(
                model.state_dict(),
                os.path.join(
                    Config['model_path'],
                    f'checkpoints/model_{epoch}.pth'
                )
            )
    print('Training ended')
    end_time = datetime.now()
    print("End Time =", end_time)
    print("Total Time =", end_time-start_time)
    torch.save(
        best_wts,
        os.path.join(
            Config['model_path'],
            'checkpoints/model_final.pth'
        )
    )
    model.load_state_dict(best_wts)
    torch.onnx.export(model,
                      dummy_input,
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
    
    dataset = LSTMCSVDataset(root_path=Config['dataset_path'])
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

    model = LSTMRegressorv2(input_size=19, output_size=7)

    device = torch.device('cuda:0' if torch.cuda.is_available() and Config['use_cuda'] else 'cpu')

    optimizer = torch.optim.Adam(model.parameters(), lr=Config['lr']) #rmsprop, adam

    criterion = torch.nn.MSELoss()

    ratio = [int(len(dataset)*0.9), len(dataset)-int(len(dataset)*0.9)]
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