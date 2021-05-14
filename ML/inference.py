import torch

from utils import Config
from model import Regressor, Regressorv2
from data import CSVDataset

import json
import pandas as pd
from tqdm import tqdm 
if __name__=='__main__':
    dataset = CSVDataset(root_path=Config['dataset_path'])
    if Config['data_type']=='euler':
        model = Regressor(input_size=18, output_size=6)
    elif Config['data_type']=='quaternion':
        model = Regressor(input_size=21, output_size=7)
    elif Config['data_type']=='both':
        model = Regressor(input_size=30, output_size=10)
    elif Config['data_type']=='relative':
        model = Regressorv2(input_size=19, output_size=7) 

    # csv_data = pd.read_csv('/home/adityan/Studies/CSCI527/alt/XR-Interaction-Toolkit-Examples/ML/Data_Exp/Exp0/BeatSaber_2_processed.csv')

    device = torch.device('cuda:0' if torch.cuda.is_available() and Config['use_cuda'] else 'cpu')
    model.load_state_dict(torch.load('/home/adityan/Studies/CSCI527/alt/XR-Interaction-Toolkit-Examples/ML/models/regressorv2_run2/checkpoints/model_169.pth'))
    model.to(device)
    scaler_file = '/home/adityan/Studies/CSCI527/alt/XR-Interaction-Toolkit-Examples/ML/models/regressorv3/scaler.json'
    scaler = json.load(open(scaler_file,'r'))
    print(scaler)
    scalerDict = dict()
    for param in scaler['scalers']:
        scalerDict.update({param['type']: [param['min'], param['scale']]})

    test_loader = torch.utils.data.DataLoader(
                        dataset,
                        batch_size = 1,
                        shuffle=False,
                        num_workers = Config['num_workers']
                    )

    model.eval()
    out_file = open('out.csv','w')

    predictedData=pd.DataFrame()
    for i, (inputs, labels) in enumerate(test_loader):
        # row = csv_data.iloc[i,1:41]
        # print(row)
        with torch.set_grad_enabled(False):
            inputs = inputs.to(device)
            labels = labels.to(device)

            outputs = model(inputs)

            
            print('Expected: ', labels)
            print('Predicted: ', outputs)
            outputs[:,0] = (outputs[:,0]-scalerDict['relativeTracker1Posx'][0])/scalerDict['relativeTracker1Posx'][1]
            outputs[:,1] = (outputs[:,1]-scalerDict['relativeTracker1Posy'][0])/scalerDict['relativeTracker1Posy'][1]
            outputs[:,2] = (outputs[:,2]-scalerDict['relativeTracker1Posz'][0])/scalerDict['relativeTracker1Posz'][1]
            outputs[:,3] = (outputs[:,3]-scalerDict['tracker1RotQx'][0])/scalerDict['tracker1RotQx'][1]
            outputs[:,4] = (outputs[:,4]-scalerDict['tracker1RotQy'][0])/scalerDict['tracker1RotQy'][1]
            outputs[:,5] = (outputs[:,5]-scalerDict['tracker1RotQz'][0])/scalerDict['tracker1RotQz'][1]
            outputs[:,6] = (outputs[:,6]-scalerDict['tracker1RotQw'][0])/scalerDict['tracker1RotQw'][1]
            # print(outputs.shape)


            batch_df = pd.DataFrame(outputs.cpu().numpy().reshape(1, 7),
                                columns=['relativeTracker1Posx', 'relativeTracker1Posy', 'relativeTracker1Posz', 'tracker1RotQx', 'tracker1RotQy', 'tracker1RotQz', 'tracker1RotQw'])

            predictedData = predictedData.append(batch_df)
            

    predictedData.to_csv('output.csv', index = False)