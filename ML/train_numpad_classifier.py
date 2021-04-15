import torch
import torch.nn as nn
import torch.nn.functional as F
from torch.utils.tensorboard import SummaryWriter

import copy
from tqdm import tqdm
import os
import json
import sys

from utils import Config
from model import Classifier
from data import NumpadTypingCSVDataset


def train(dataloader, dataset_sizes, model, criterion, optimizer, device, num_epochs=Config['num_epochs'],
          batch_size=Config['batch_size']):
    model.to(device)

    best_loss = 100.0
    best_wts = copy.deepcopy(model.state_dict())
    dummy_input = None

    if Config['tensorboard_log']:
        writer = SummaryWriter(Config['model_path'])

    for epoch in range(num_epochs):
        for phase in ['train', 'valid']:
            if phase == 'train':
                model.train()
            else:
                model.eval()

            running_loss = 0.0
            running_accuracy = 0.0

            for inputs, labels in tqdm(dataloader[phase]):
                with torch.set_grad_enabled(True):

                    optimizer.zero_grad()
                    inputs = inputs.to(device)
                    labels = labels.to(device)

                    if dummy_input is None:
                        dummy_input = inputs[0].unsqueeze(0)

                    outputs = model(inputs)

                    loss = criterion(outputs, labels)
                    accuracy = multi_acc(outputs, labels)

                    if Config['use_cuda']:
                        l2_regularization = torch.tensor(0.).cuda()
                    else:
                        l2_regularization = torch.tensor(0.)

                    for param in model.parameters():
                        l2_regularization += torch.norm(param, 2) ** 2

                    loss += 1e-5 * l2_regularization

                    if phase == 'train':
                        loss.backward()
                        optimizer.step()

                    running_loss += loss.item()
                    running_accuracy += accuracy.item()

            epoch_loss = running_loss / len(dataloader[phase])
            epoch_accuracy = running_accuracy / len(dataloader[phase])

            print(f'Epoch {epoch} Phase: {phase} Loss: {epoch_loss:.4f} Accuracy: {epoch_accuracy:.4f}')
            if epoch_loss < best_loss and phase == 'valid':
                best_loss = epoch_loss
                best_wts = copy.deepcopy(model.state_dict())
            writer.add_scalar(phase + '_loss', epoch_loss, global_step=epoch)
            writer.add_scalar(phase + '_accuracy', epoch_accuracy, global_step=epoch)

        if (epoch + 1) % 5 == 0:
            torch.onnx.export(model,
                              dummy_input,
                              os.path.join(
                                  Config['model_path'],
                                  f'checkpoints/model_{epoch}.onnx'
                              ),
                              export_params=True
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
                      dummy_input,
                      os.path.join(
                          Config['model_path'],
                          'checkpoints/model_final.onnx'
                      ),
                      export_params=True
                      )


def multi_acc(y_pred, y_test):
    y_pred_softmax = torch.log_softmax(y_pred, dim=1)
    _, y_pred_tags = torch.max(y_pred_softmax, dim=1)

    correct_pred = (y_pred_tags == y_test).float()
    acc = correct_pred.sum() / len(correct_pred)

    acc = (torch.round(acc * 10**3) / (10**3)) * 100

    return acc


if __name__ == '__main__':
    os.makedirs(Config['model_path'], exist_ok=True)
    os.makedirs(os.path.join(Config['model_path'], 'logs'), exist_ok=True)
    os.makedirs(os.path.join(Config['model_path'], 'checkpoints'), exist_ok=True)

    dataset = NumpadTypingCSVDataset(root_path=Config['dataset_path'])

    with open(os.path.join(Config['model_path'], 'scaler.json'), 'w') as f:
        scaler = {'scalers': []}
        for idx, header in enumerate(dataset.features):
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

    with open(os.path.join(Config['model_path'], 'labels.json'), 'w') as f:
        labels = {'numpadKeys': []}
        for key, numpadKey in dataset.labels.items():
            labels['numpadKeys'].append({
                'key': key,
                'numpadKey': numpadKey
            })
        json.dump(labels, f)

    model = Classifier(input_size=30, output_size=10)

    device = torch.device('cuda:0' if torch.cuda.is_available() and Config['use_cuda'] else 'cpu')

    #optimizer = torch.optim.SGD(model.parameters(), lr=Config['lr'])  # rmsprop, adam
    optimizer = torch.optim.Adam(model.parameters(), lr=Config['lr'])

    criterion = torch.nn.CrossEntropyLoss()

    ratio = [int(len(dataset) * 0.8), len(dataset) - int(len(dataset) * 0.8)]
    train_dataset, valid_dataset = torch.utils.data.random_split(dataset, ratio)

    train_loader = torch.utils.data.DataLoader(
        train_dataset,
        batch_size=Config['batch_size'],
        shuffle=True,
        num_workers=Config['num_workers']
    )

    valid_loader = torch.utils.data.DataLoader(
        valid_dataset,
        batch_size=Config['batch_size'],
        shuffle=False,
        num_workers=Config['num_workers']
    )
    dataloaders = {
        'train': train_loader,
        'valid': valid_loader
    }

    dataset_sizes = {
        'train': len(train_dataset),
        'valid': len(valid_dataset)
    }

    train(
        dataloaders,
        dataset_sizes,
        model,
        criterion,
        optimizer,
        device
    )
