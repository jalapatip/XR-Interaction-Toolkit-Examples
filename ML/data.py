import torch
import os
import pandas as pd
import numpy as np
from sklearn.preprocessing import MinMaxScaler

from utils import Config

class CSVDataset(torch.utils.data.Dataset):
    def __init__(self, root_path, output_type=Config['data_type']):
        files = os.listdir(root_path)
        files = [f for f in files if f.endswith('.csv')]
        all_data=[]
        for file in files:
            data = np.array([pd.read_csv(os.path.join(root_path,file)).iloc[:,1:41]])
            all_data.append(data)
        self.data = np.concatenate(all_data, axis=0).squeeze(0)
        # print(self.data.shape)
        self.scaler = MinMaxScaler()
        self.scaler.fit(self.data)
        self.scaled_data = self.scaler.transform(self.data)
        self.output_type = output_type
    
    def __len__(self):
        return len(self.data)
    
    def __getitem__(self, idx):
        row = self.scaled_data[idx]
        
        headPosx = row[0]
        headPosy = row[1]
        headPosz = row[2]
        headRotx = row[3]
        headRoty = row[4]
        headRotz = row[5]
        headRotQx = row[6]
        headRotQy = row[7]
        headRotQz = row[8]
        headRotQw = row[9]

        handRPosx = row[10]
        handRPosy = row[11]
        handRPosz = row[12]
        handRRotx = row[13]
        handRRoty = row[14]
        handRRotz = row[15]
        handRRotQx = row[16]
        handRRotQy = row[17]
        handRRotQz = row[18]
        handRRotQw = row[19]

        handLPosx = row[20]
        handLPosy = row[21]
        handLPosz = row[22]
        handLRotx = row[23]
        handLRoty = row[24]
        handLRotz = row[25]
        handLRotQx = row[26]
        handLRotQy = row[27]
        handLRotQz = row[28]
        handLRotQw = row[29]

        tracker1Posx = row[30]
        tracker1Posy = row[31]
        tracker1Posz = row[32]
        tracker1Rotx = row[33]
        tracker1Roty = row[34]
        tracker1Rotz = row[35]
        tracker1RotQx = row[36]
        tracker1RotQy = row[37]
        tracker1RotQz = row[38]
        tracker1RotQw = row[39]

        if self.output_type=='euler':
            return (
                torch.FloatTensor([headPosx, headPosy, headPosz, headRotx, headRoty, headRotz,
                    handRPosx, handRPosy, handRPosz, handRRotx, handRRoty, handRRotz,
                    handLPosx, handLPosy, handLPosz, handLRotx, handLRoty, handLRotz,
                    ]),
                torch.FloatTensor([tracker1Posx, tracker1Posy, tracker1Posz, tracker1Rotx, tracker1Roty, 
                    tracker1Rotz])
            )
        elif self.output_type=='quaternion':
            return (
                np.array([headPosx, headPosy, headPosz, headRotQx, headRotQy, headRotQz, headRotQw,
                    handRPosx, handRPosy, handRPosz, handRRotQx, handRRotQy, handRRotQz, handRRotQw,
                    handLPosx, handLPosy, handLPosz, handLRotQx, handLRotQy, handLRotQz, handLRotQw,
                    ]),
                np.array([tracker1Posx, tracker1Posy, tracker1Posz, tracker1RotQx, tracker1RotQy, 
                    tracker1RotQz, tracker1RotQz, tracker1RotQw])
            )
        elif self.output_type=='both':
            return (
                np.array([headPosx, headPosy, headPosz, headRotx, headRoty, headRotz,headRotQx, headRotQy, headRotQz, headRotQw,
                    handRPosx, handRPosy, handRPosz, handRRotx, handRRoty, handRRotz, handRRotQx, handRRotQy, handRRotQz, handRRotQw,
                    handLPosx, handLPosy, handLPosz, handLRotx, handLRoty, handLRotz, handLRotQx, handLRotQy, handLRotQz, handLRotQw
                    ]),
                np.array([tracker1Posx, tracker1Posy, tracker1Posz, tracker1Rotx, tracker1Roty, 
                    tracker1Rotz, tracker1RotQx, tracker1RotQy, tracker1RotQz, tracker1RotQw])
            )

