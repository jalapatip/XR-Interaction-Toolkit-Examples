import torch
from utils import Config
from model import Classifier
from data import GrabCSVDataset
import json
import pandas as pd

if __name__ == '__main__':
    dataset = GrabCSVDataset(root_path=Config['dataset_path']+'/test')
    model = Classifier(input_size=28, output_size=9)
    device = torch.device('cuda:0' if torch.cuda.is_available() and Config['use_cuda'] else 'cpu')
    model.load_state_dict(torch.load(
        '.\models\Exp2\checkpoints\model_final.pth'))
    model.to(device)
    scaler_file = '.\models\Exp2\scaler.json'
    scaler = json.load(open(scaler_file, 'r'))
    scalerDict = dict()
    for param in scaler['scalers']:
        scalerDict.update({param['type']: [param['min'], param['scale']]})

    test_loader = torch.utils.data.DataLoader(
        dataset,
        batch_size=1,
        shuffle=False,
        num_workers=Config['num_workers']
    )

    model.eval()
    count = 0
    predictedData = pd.DataFrame()
    for i, (inputs, labels) in enumerate(test_loader):
        # row = csv_data.iloc[i,1:41]
        # print(row)
        with torch.set_grad_enabled(False):
            inputs = inputs.to(device)
            labels = labels.to(device)
            outputs = model(inputs)
            # print('Expected: ', labels)
            # print('Predicted: ', outputs)
            pred_probab = torch.softmax(outputs, dim=1)
            pred = pred_probab.argmax(1)
            if torch.equal(labels, pred):
                count+=1
    accuracy = count / len(dataset)
    print("Accuracy for test data %d/%d:" %(count, len(dataset)), accuracy)