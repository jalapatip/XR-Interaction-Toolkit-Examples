import torch
import torch.nn as nn
import torch.nn.functional as F

from utils import Config

class Regressor(nn.Module):
    def __init__(self, input_size, output_size):
        super(Regressor, self).__init__()
        self.input_size = input_size
        self.output_size = output_size

        self.fc1 = nn.Linear(self.input_size, 512)
        self.bn1 = nn.BatchNorm1d(512)
        self.fc2 = nn.Linear(512, 256)
        self.bn2 = nn.BatchNorm1d(256)
        self.fc3 = nn.Linear(256, 128)
        self.bn3 = nn.BatchNorm1d(128)
        self.fc4 = nn.Linear(128, output_size)

    def forward(self, x):
        # print(x.shape)
        x = F.relu(self.bn1(self.fc1(x)))
        x = F.relu(self.bn2(self.fc2(x)))
        x = F.relu(self.bn3(self.fc3(x)))
        return nn.Sigmoid()(self.fc4(x))

class Regressorv2(nn.Module):
    def __init__(self, input_size, output_size):
        super(Regressorv2, self).__init__()
        self.input_size = input_size
        self.output_size = output_size
        self.branching_alpha=2
        self.samples_size = 400000
        self.hidden_size = int((self.samples_size/(self.branching_alpha*(self.input_size+self.output_size)))*0.2)
        print('Hidden size: ', self.hidden_size)

        self.fc1 = nn.Linear(self.input_size, self.hidden_size)
        self.bn1 = nn.BatchNorm1d(self.hidden_size)
        self.fc2 = nn.Linear(self.hidden_size, self.hidden_size)
        self.bn2 = nn.BatchNorm1d(self.hidden_size)
        self.fc3 = nn.Linear(self.hidden_size, self.hidden_size)
        self.bn3 = nn.BatchNorm1d(self.hidden_size)
        self.fc4 = nn.Linear(self.hidden_size, self.hidden_size)
        self.bn4 = nn.BatchNorm1d(self.hidden_size)
        self.fc5 = nn.Linear(self.hidden_size, self.hidden_size)
        self.bn5 = nn.BatchNorm1d(self.hidden_size)
        self.fc6 = nn.Linear(self.hidden_size, self.output_size)
        
    def forward(self, x):
        x = F.relu(self.bn1(self.fc1(x)))
        x = F.relu(self.bn2(self.fc2(x)))
        x = F.relu(self.bn3(self.fc3(x)))
        x = F.relu(self.bn4(self.fc4(x)))
        x = F.relu(self.bn5(self.fc5(x)))
        return nn.Sigmoid()(self.fc6(x))

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

        self.lstm_1 = nn.LSTM(self.input_size, 512, batch_first=True)
        # self.bn_1 = nn.BatchNorm1d(500)
        self.dropout_1 = nn.Dropout(0.1)
        self.lstm_2 = nn.LSTM(512, 256, batch_first=True)
        # self.bn_2 = nn.BatchNorm1d(100)
        self.out = nn.Linear(256, output_size)
        # self.fc = nn.Linear(self.hidden_size, self.output_size)
        self.h1, self.h2 = self.init_hidden(batch_size=Config['batch_size'],device='cuda:0')

    def init_hidden(self, batch_size, device='cpu'):
        return [(torch.zeros(1, batch_size , 512).to(device), torch.zeros(1, batch_size , 512).to(device)),
                (torch.zeros(1, batch_size, 256).to(device), torch.zeros(1, batch_size, 256).to(device))]
    
    def forward(self, x, device='cpu'):
        # print(x.shape)
        # print(self.h1.shape)
        out, self.h1 = self.lstm_1(x, self.h1)
        out = self.dropout_1(out)
        out = nn.Tanh()(out)
        out, self.h2 = self.lstm_2(out, self.h2)
        print(out.shape)
        # out = out.reshape(out.shape[0],-1)
        print(out.shape)
        out = self.out(out)
        return nn.Tanh()(out)


class LSTMRegressorv2(nn.Module):
    def __init__(self, input_size, output_size):
        super(LSTMRegressorv2, self).__init__()
        self.input_size = input_size
        self.output_size = output_size

        self.lstm_1 = nn.LSTM(self.input_size, 100, batch_first=True)
        self.fc1 = nn.Linear(1000, 256)
        self.fc2 = nn.Linear(256, output_size)
        self.h1 = self.init_hidden(batch_size=Config['batch_size'], device='cuda:0')

    def init_hidden(self, batch_size, device='cpu'):
        return [(torch.zeros(1, batch_size, 100).to(device), torch.zeros(1, batch_size, 100).to(device))]

    def forward(self, x, device='cpu'):
        out, self.h1 = self.lstm_1(x, self.h1[0])
        # print(out.shape)
        out = out.reshape(out.shape[0], -1)
        # print(out.shape)
        out = self.fc1(out)
        out = self.fc2(out)
        return out

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
