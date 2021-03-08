import torch
import torch.nn as nn
import torch.nn.functional as F


class Regressor(nn.Module):
    def __init__(self, input_size, output_size):
        super(Regressor, self).__init__()
        self.input_size = input_size
        self.output_size = output_size

        self.fc1 = nn.Linear(self.input_size, 256)
        self.bn1 = nn.BatchNorm1d(256)
        self.fc2 = nn.Linear(256, 32)
        self.bn2 = nn.BatchNorm1d(32)
        self.fc3 = nn.Linear(32, self.output_size)

    def forward(self, x):
        x = F.relu(self.fc1(x))
        x = F.relu(self.fc2(x))
        return nn.Sigmoid()(self.fc3(x))

class SVMRegressor(nn.Module):
    def __init__(self, input_size, output_size):
        super(SVMRegressor, self).__init__()
        self.input_size = input_size
        self.output_size = output_size

        self.svm = nn.Linear(input_size, output_size)
    def forward(self, x):
        x = nn.Sigmoid()(self.svm(x))
        return x

class CNNRegressor(nn.Module):
    # def __init__(self, input_size, output_size):
    pass

class LSTMRegressor(nn.Module):
    def __init__(self, input_size, output_size):
        super(LSTMRegressor,self).__init__()
        self.input_size = input_size
        self.output_size = output_size

        self.lstm_1 = nn.LSTM(self.input_size, 500, batch_first=True)
        # self.bn_1 = nn.BatchNorm1d(500)
        self.dropout_1 = nn.Dropout(0.3)
        self.lstm_2 = nn.LSTM(500, 100, batch_first=True)
        # self.bn_2 = nn.BatchNorm1d(100)
        self.out = nn.Linear(100, output_size)
        # self.fc = nn.Linear(self.hidden_size, self.output_size)

    def init_hidden(self, batch_size, device='cpu'):
        return [torch.zeros(1, batch_size, 500).to(device), torch.zeros(1, batch_size, 100).to(device)]
    
    def forward(self, x, device='cpu'):
        batch_size = x.shape[0]
        h1, h2 = self.init_hidden(batch_size, device)
        # print(x)
        out, h1 = self.lstm_1(x)
        # print(out)
        # out = self.bn_1(out)
        out = self.dropout_1(out)
        # print(out)'
        out = nn.Tanh()(out)
        out, h2 = self.lstm_2(out)
        # out = self.bn_2(out)
        # print(out)
        out = self.out(out)
        return nn.Tanh()(out)

class Classifier(nn.Module):
    def __init__(self, input_size, output_size):
        super(Classifier, self).__init__()
        self.input_size = input_size
        self.output_size = output_size

        self.fc1 = nn.Linear(self.input_size, 512)
        self.bn1 = nn.BatchNorm1d(512)
        self.fc2 = nn.Linear(512, 128)
        self.bn2 = nn.BatchNorm1d(128)
        self.fc3 = nn.Linear(128, 64)
        self.bn3 = nn.BatchNorm1d(64)
        self.fc4 = nn.Linear(64, self.output_size)

    def forward(self, x):
        x = F.relu(self.bn1(self.fc1(x)))
        x = F.relu(self.bn2(self.fc2(x)))
        x = F.relu(self.bn3(self.fc3(x)))
        return self.fc4(x)
