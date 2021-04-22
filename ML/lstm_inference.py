from utils import Config
import torch
import os
import pandas as pd
import numpy as np
from model import LSTMRegressor
from data import LSTMCSVDataset
from sklearn.preprocessing import MinMaxScaler
import csv
import json
pd.options.mode.chained_assignment = None  # default='warn' ##


# public float Transform(float input)
#     {
#         return input * scale + min;
#     }

#     public float InverseTransform(float input)
#     {
#         return (input - min) / scale;
#     }



if __name__=='__main__':
    src_file = r'C:\Users\CSCI-538-HP-Z240-lll\Desktop\UnityProjects\XR-Interaction-Toolkit-Examples\VR\Assets\XROSUI\ML_Model\Exp0_ 2021-02-19-02-12-11 - Duplicates Removed.csv'
    scaler_file = r'C:\Users\CSCI-538-HP-Z240-lll\Desktop\UnityProjects\XR-Interaction-Toolkit-Examples\VR\Assets\XROSUI\ML_Model\lstm_scaler.json'
    scaler = json.load(open(scaler_file,'r'))

    model = LSTMRegressor(input_size=19, output_size=7)
    device = torch.device('cuda:0' if torch.cuda.is_available() and Config['use_cuda'] else 'cpu')
    model.load_state_dict(torch.load(r'C:\Users\CSCI-538-HP-Z240-lll\Desktop\UnityProjects\XR-Interaction-Toolkit-Examples\VR\Assets\XROSUI\ML_Model\lstm_model_run7_884.pth'))
    model.to(device)
    model.eval()


    scalerDict = dict()
    for param in scaler['scalers']:
        scalerDict.update({param['type']: [param['min'], param['scale']]})
    # print(scaler)
    # print(scalerDict)
    lookback = 10
    step_value = 1
    csv_data = pd.read_csv(src_file).iloc[:, 1:41]

    csv_data['relativeHandRPosx'] = csv_data['headPosx'] - csv_data['handRPosx']
    csv_data['relativeHandRPosy'] = csv_data['headPosy'] - csv_data['handRPosy']
    csv_data['relativeHandRPosz'] = csv_data['headPosz'] - csv_data['handRPosz']
    csv_data['relativeHandLPosx'] = csv_data['headPosx'] - csv_data['handLPosx']
    csv_data['relativeHandLPosy'] = csv_data['headPosy'] - csv_data['handLPosy']
    csv_data['relativeHandLPosz'] = csv_data['headPosz'] - csv_data['handLPosz']
    csv_data['relativeTracker1Posx'] = csv_data['headPosx'] - csv_data['tracker1Posx']
    csv_data['relativeTracker1Posy'] = csv_data['headPosy'] - csv_data['tracker1Posy']
    csv_data['relativeTracker1Posz'] = csv_data['headPosz'] - csv_data['tracker1Posz']

    rows = []
    out_file = open('out.csv','w')

    predictedData = pd.DataFrame(
        columns=['relativeTracker1Posx', 'relativeTracker1Posy', 'relativeTracker1Posz', 'tracker1Rotx',
                 'tracker1Roty', 'tracker1Rotz'])

    for i in range(len(csv_data)-lookback-1):
        a = csv_data.iloc[i:(i+lookback):step_value, :]

        orig = np.array(a.copy())
        for key in list(a.keys()):
            a[key] = a[key] * scalerDict[key][1] + scalerDict[key][0]
        a = a[['headPosy','headRotQx','headRotQy','headRotQz', 'headRotQw','relativeHandRPosx','relativeHandRPosy','relativeHandRPosz','handRRotQx', 'handRRotQy', 'handRRotQz', 'handRRotQw', 'relativeHandLPosx', 'relativeHandLPosy', 'relativeHandLPosz','handLRotQx','handLRotQy','handLRotQz', 'handLRotQw']]


        record = np.array(a)
        with torch.set_grad_enabled(False):
            model.h1, model.h2 = model.init_hidden(1,'cuda:0')
            outputs = model(torch.FloatTensor(record).to(device).unsqueeze(0))
        #print(outputs.shape)
        outputs = torch.squeeze(outputs)
        #print(outputs.shape)
        last_output = outputs[9,:]
        #print(last_output.shape)


        last_output[0] = (last_output[0] - scalerDict['relativeTracker1Posx'][0]) / scalerDict['relativeTracker1Posx'][1]
        last_output[1] = (last_output[1] - scalerDict['relativeTracker1Posy'][0]) / scalerDict['relativeTracker1Posy'][1]
        last_output[2] = (last_output[2] - scalerDict['relativeTracker1Posz'][0]) / scalerDict['relativeTracker1Posz'][1]
        last_output[3] = (last_output[3] - scalerDict['tracker1RotQx'][0]) / scalerDict['tracker1RotQx'][1]
        last_output[4] = (last_output[4] - scalerDict['tracker1RotQy'][0]) / scalerDict['tracker1RotQy'][1]
        last_output[5] = (last_output[5] - scalerDict['tracker1RotQz'][0]) / scalerDict['tracker1RotQz'][1]
        last_output[6] = (last_output[6] - scalerDict['tracker1RotQw'][0]) / scalerDict['tracker1RotQw'][1]

        #print(last_output)
        batch_df = pd.DataFrame(last_output.cpu().numpy().reshape(1, 7),
                                columns=['relativeTracker1Posx', 'relativeTracker1Posy', 'relativeTracker1Posz',
                                         'tracker1RotQx', 'tracker1RotQy', 'tracker1RotQz', 'tracker1RotQw'])
        #print(batch_df.to_string(index = False))
        predictedData = predictedData.append(batch_df)

        #
        # outputs[:,:,0] = outputs[:,:,0]-scalerDict['relativeTracker1Posx'][0]/scalerDict['relativeTracker1Posx'][1]
        # outputs[:,:,1] = outputs[:,:,1]-scalerDict['relativeTracker1Posy'][0]/scalerDict['relativeTracker1Posy'][1]
        # outputs[:,:,2] = outputs[:,:,2]-scalerDict['relativeTracker1Posz'][0]/scalerDict['relativeTracker1Posz'][1]
        # outputs[:,:,3] = outputs[:,:,3]-scalerDict['tracker1Rotx'][0]/scalerDict['tracker1Rotx'][1]
        # outputs[:,:,4] = outputs[:,:,4]-scalerDict['tracker1Roty'][0]/scalerDict['tracker1Roty'][1]
        # outputs[:,:,5] = outputs[:,:,5]-scalerDict['tracker1Rotz'][0]/scalerDict['tracker1Rotz'][1]

        # for i in range(lookback):
        #     # print(outputs[0,i,:].cpu().numpy().tolist())
        #     for data in orig[i]:
        #         out_file.write(str(data)+',')
        #     for data in outputs[0,9,:]: #changed from outputs[0,i,:] to outputs[0,9,:]
        #         out_file.write(str(data.cpu().numpy().tolist())+',')
        #     out_file.write('\n')



    predictedData.to_csv(r'output.csv', index = False)
    #out_file.close()





