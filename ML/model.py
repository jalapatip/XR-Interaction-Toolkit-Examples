import torch
import torch.nn as nn
import torch.nn.functional as F


class Regressor(nn.Module):
    def __init__(self, input_size, output_size):
        super(Regressor, self).__init__()
        self.input_size = input_size
        self.output_size = output_size

        self.fc1 = nn.Linear(self.input_size, 16)
        self.bn1 = nn.BatchNorm1d(16)
        self.fc2 = nn.Linear(16, 8)
        self.bn2 = nn.BatchNorm1d(8)
        self.fc3 = nn.Linear(8, self.output_size)

    def forward(self, x):
        x = F.relu(self.fc1(x))
        x = F.relu(self.fc2(x))
        return nn.Sigmoid()(self.fc3(x))

class CNNRegressor(nn.Module):
    # def __init__(self, input_size, output_size):
    pass

class LSTMRegressor(nn.Module):
    def __init__(self, input_size, hidden_size, output_size):
        super(LSTMRegressor,self).__init__()
        self.input_size = input_size
        self.hidden_size = hidden_size
        self.output_size = output_size

        self.rnn = nn.GRU(self.input_size, self.hidden_size, batch_first=True)
        self.fc = nn.Linear(self.hidden_size, self.output_size)

    def init_hidden(self, batch_size, device='cpu'):
        return torch.zeros(1, batch_size, self.hidden_size).to(device)
    
    def forward(self, x, device='cpu'):
        batch_size = x.shape[0]
        h = self.init_hidden(batch_size, device)
        out, h = self.rnn(x,h)
        out = self.fc(out)
        return out