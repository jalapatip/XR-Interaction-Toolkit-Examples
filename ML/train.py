import torch
import torch.nn as nn
import torch.nn.functional as F
from torch.utils.tensorboard import SummaryWriter

import copy 
from tqdm import tqdm
import os

from utils import Config
from model import Regressor
from data import CSVDataset

def train(dataloader, dataset_sizes, model, criterion, optimizer, device, num_epochs=Config['num_epochs'], batch_size=Config['batch_size']):

    os.makedirs(Config['model_path'], exist_ok=True)
    os.makedirs(os.path.join(Config['model_path'],'logs'),exist_ok=True)
    os.makedirs(os.path.join(Config['model_path'],'checkpoints'),exist_ok=True)
    model.to(device)

    best_loss = 100.0
    best_wts = copy.deepcopy(model.state_dict())

    if Config['tensorboard_log']:
        writer = SummaryWriter(Config['model_path'])
    
    for epoch in range(num_epochs):
        
        for phase in ['train', 'valid']:
            if phase=='train':
                model.train()
            else:
                model.eval()
            
            running_loss = 0.0

            for inputs, labels in tqdm(dataloader[phase]):
                # print(inputs.shape, labels.shape)
                with torch.set_grad_enabled(True):

                    optimizer.zero_grad()
                    inputs = inputs.to(device)
                    labels = labels.to(device)

                    outputs = model(inputs)
                    # print(criterion(outputs, labels).shape)
                    print(f'Loss: {criterion(outputs, labels)}')
                    print(outputs, labels)
                    loss = criterion(outputs, labels)
                    # loss = torch.sum(torch.square(outputs-labels), axis=1)/outputs.size()[1]
                    # loss = torch.sum(loss)/loss.shape[0]
                    if Config['use_cuda']:
                        l2_regularization = torch.tensor(0.).cuda()
                    else:
                        l2_regularization = torch.tensor(0.)
                    
                    for param in model.parameters():
                        l2_regularization += torch.norm(param, 2)**2

                    loss += 1e-5 * l2_regularization

                    if phase =='train':
                        loss.backward()
                        optimizer.step()
                    running_loss += loss.item()*inputs.size(0)
            epoch_loss = running_loss/dataset_sizes[phase]
            print(f'Epoch {epoch} Loss: {loss:.4f}')
            if epoch_loss < best_loss and phase=='valid':
                best_wts = copy.deepcopy(model.state_dict())
            writer.add_scalar(phase+'_loss', epoch_loss, global_step=epoch)
        if (epoch+1)%5==0:
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

if __name__ == '__main__':
    dataset = CSVDataset(root_path=Config['dataset_path'])
    # print(dataset[0][0].shape, dataset[0][1].shape)


    model = Regressor(input_size=18, output_size=6)

    device = torch.device('cuda:0' if torch.cuda.is_available() and Config['use_cuda'] else 'cpu')

    optimizer = torch.optim.SGD(model.parameters(), lr=Config['lr'])

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