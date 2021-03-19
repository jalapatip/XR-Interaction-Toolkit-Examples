import torch
import os
import pandas as pd
import numpy as np
from sklearn.preprocessing import MinMaxScaler

from utils import Config
class LSTMCSVDataset(torch.utils.data.Dataset):
    def __init__(self, root_path, output_type=Config['data_type'], look_back=10, step_value = 1):
        files = os.listdir(root_path)
        files = [f for f in files if f.endswith('.csv')]
        all_data=[]
       
        for file in files:
            csv_data = pd.read_csv(os.path.join(root_path,file)).iloc[:,2:42]
            
            csv_data['relativeHandRPosx'] = csv_data.headPosx-csv_data.HandRPosx
            csv_data['relativeHandRPosy'] = csv_data['headPosy']-csv_data['HandRPosy']
            csv_data['relativeHandRPosz'] = csv_data['headPosz']-csv_data['HandRPosz']
            csv_data['relativeHandLPosx'] = csv_data['headPosx']-csv_data['handLPosx']
            csv_data['relativeHandLPosy'] = csv_data['headPosy']-csv_data['handLPosy']
            csv_data['relativeHandLPosz'] = csv_data['headPosz']-csv_data['handLPosz']
            csv_data['relativeTracker1Posx'] = csv_data['headPosx']-csv_data['tracker1Posx']
            csv_data['relativeTracker1Posy'] = csv_data['headPosy']-csv_data['tracker1Posy']
            csv_data['relativeTracker1Posz'] = csv_data['headPosz']-csv_data['tracker1Posz']
            
            data = np.array([csv_data])
            all_data.append(data)
        self.data = np.concatenate(all_data, axis=1).squeeze(0)
        self.scaler = MinMaxScaler(feature_range=(-1,1))
        self.scaler.fit(self.data)
        self.scaled_data = self.scaler.transform(self.data)
        self.lstm_data = []
        for i in range(len(self.scaled_data)-look_back-1):
            a = self.scaled_data[i:(i+look_back):step_value]
            self.lstm_data.append(a)
        self.lstm_data = np.array(self.lstm_data)
        self.output_type = output_type
        self.look_back = look_back
        self.step_value = step_value
    def __len__(self):
        return self.lstm_data.shape[0]
    
    def __getitem__(self, idx):
        row = self.lstm_data[idx]
        headPosx = row[:, 0]
        headPosy = row[:,1]
        headPosz = row[:,2]
        headRotx = row[:,3]
        headRoty = row[:,4]
        headRotz = row[:,5]
        headRotQx = row[:,6]
        headRotQy = row[:,7]
        headRotQz = row[:,8]
        headRotQw = row[:,9]

        handRPosx = row[:,10]
        handRPosy = row[:,11]
        handRPosz = row[:,12]
        handRRotx = row[:,13]
        handRRoty = row[:,14]
        handRRotz = row[:,15]
        handRRotQx = row[:,16]
        handRRotQy = row[:,17]
        handRRotQz = row[:,18]
        handRRotQw = row[:,19]

        handLPosx = row[:,20]
        handLPosy = row[:,21]
        handLPosz = row[:,22]
        handLRotx = row[:,23]
        handLRoty = row[:,24]
        handLRotz = row[:,25]
        handLRotQx = row[:,26]
        handLRotQy = row[:,27]
        handLRotQz = row[:,28]
        handLRotQw = row[:,29]

        tracker1Posx = row[:,30]
        tracker1Posy = row[:,31]
        tracker1Posz = row[:,32]
        tracker1Rotx = row[:,33]
        tracker1Roty = row[:,34]
        tracker1Rotz = row[:,35]
        tracker1RotQx = row[:,36]
        tracker1RotQy = row[:,37]
        tracker1RotQz = row[:,38]
        tracker1RotQw = row[:,39]

        relativeHandRPosx = row[:,40]
        relativeHandRPosy = row[:,41]
        relativeHandRPosz = row[:,42]
        relativeHandLPosx = row[:,43]
        relativeHandLPosy = row[:,44]
        relativeHandLPosz = row[:,45]
        relativeTracker1Posx = row[:,46]
        relativeTracker1Posy = row[:,47]
        relativeTracker1Posz = row[:,48]

        if self.output_type=='euler':
            return (
                torch.FloatTensor(np.stack([headPosx, headPosy, headPosz, headRotx, headRoty, headRotz,
                    handRPosx, handRPosy, handRPosz, handRRotx, handRRoty, handRRotz,
                    handLPosx, handLPosy, handLPosz, handLRotx, handLRoty, handLRotz,
                    ],axis=-1)),
                torch.FloatTensor(np.stack([tracker1Posx, tracker1Posy, tracker1Posz, tracker1Rotx, tracker1Roty, 
                    tracker1Rotz],axis=-1))
            )
        elif self.output_type=='quaternion':
            return (
                torch.FloatTensor(np.stack([headPosx, headPosy, headPosz, headRotQx, headRotQy, headRotQz, headRotQw,
                    handRPosx, handRPosy, handRPosz, handRRotQx, handRRotQy, handRRotQz, handRRotQw,
                    handLPosx, handLPosy, handLPosz, handLRotQx, handLRotQy, handLRotQz, handLRotQw,
                    ],axis=-1)),
                torch.FloatTensor(np.stack([tracker1Posx, tracker1Posy, tracker1Posz, tracker1RotQx, tracker1RotQy, 
                    tracker1RotQz, tracker1RotQw],axis=-1))
            )
        elif self.output_type=='both':
            return (
                torch.FloatTensor(np.stack([headPosx, headPosy, headPosz, headRotx, headRoty, headRotz,headRotQx, headRotQy, headRotQz, headRotQw,
                    handRPosx, handRPosy, handRPosz, handRRotx, handRRoty, handRRotz, handRRotQx, handRRotQy, handRRotQz, handRRotQw,
                    handLPosx, handLPosy, handLPosz, handLRotx, handLRoty, handLRotz, handLRotQx, handLRotQy, handLRotQz, handLRotQw
                    ],axis=-1)),
                torch.FloatTensor(np.stack([tracker1Posx, tracker1Posy, tracker1Posz, tracker1Rotx, tracker1Roty, 
                    tracker1Rotz, tracker1RotQx, tracker1RotQy, tracker1RotQz, tracker1RotQw],axis=-1))
            )
        elif self.output_type=='relative' or self.output_type=='relative_svm' or self.output_type=='hacklstm':
            return (
                torch.FloatTensor(np.stack([headPosy, headRotx, headRoty, headRotz,
                    relativeHandRPosx, relativeHandRPosy, relativeHandRPosz, handRRotx, handRRoty, handRRotz,
                    relativeHandLPosx, relativeHandLPosy, relativeHandLPosz, handLRotx, handLRoty, handLRotz,
                    ],axis=-1)),
                torch.FloatTensor(np.stack([relativeTracker1Posx, relativeTracker1Posy, relativeTracker1Posz,
                    tracker1Rotx, tracker1Roty, tracker1Rotz],axis=-1))
            )

class CSVDataset(torch.utils.data.Dataset):
    def __init__(self, root_path, output_type=Config['data_type']):
        files = os.listdir(root_path)
        files = [f for f in files if f.endswith('.csv')]
        all_data=[]
       
        for file in files:
            print(file)
            csv_data = pd.read_csv(os.path.join(root_path,file)).iloc[:,1:41]
            
            csv_data['relativeHandRPosx'] = csv_data.headPosx-csv_data.handRPosx
            csv_data['relativeHandRPosy'] = csv_data['headPosy']-csv_data['handRPosy']
            csv_data['relativeHandRPosz'] = csv_data['headPosz']-csv_data['handRPosz']
            csv_data['relativeHandLPosx'] = csv_data['headPosx']-csv_data['handLPosx']
            csv_data['relativeHandLPosy'] = csv_data['headPosy']-csv_data['handLPosy']
            csv_data['relativeHandLPosz'] = csv_data['headPosz']-csv_data['handLPosz']
            csv_data['relativeTracker1Posx'] = csv_data['headPosx']-csv_data['tracker1Posx']
            csv_data['relativeTracker1Posy'] = csv_data['headPosy']-csv_data['tracker1Posy']
            csv_data['relativeTracker1Posz'] = csv_data['headPosz']-csv_data['tracker1Posz']
            csv_data.to_csv('relative_columns.csv', index=False)
            data = np.array([csv_data])
            all_data.append(data)
        self.data = np.concatenate(all_data, axis=1).squeeze(0)
        self.scaler = MinMaxScaler(feature_range=(0,1))
        self.scaler.fit(self.data)
        self.scaled_data = self.scaler.transform(self.data)
        print(self.scaled_data.shape)

        self.output_type = output_type
    
    def __len__(self):
        return len(self.data)
    
    def __getitem__(self, idx):
        keys = ['headPosx', 'headPosy', 'headPosz', 'headRotx', 'headRoty', 'headRotz', 'headRotQx', 'headRotQy', 'headRotQz', 'headRotQw', 'handRPosx', 'handRPosy', 'handRPosz', 'handRRotx', 'handRRoty', 'handRRotz', 'handRRotQx', 'handRRotQy', 'handRRotQz', 'handRRotQw', 'handLPosx', 'handLPosy', 'handLPosz', 'handLRotx', 'handLRoty', 'handLRotz', 'handLRotQx', 'handLRotQy', 'handLRotQz', 'handLRotQw', 'tracker1Posx', 'tracker1Posy', 'tracker1Posz', 'tracker1Rotx', 'tracker1Roty', 'tracker1Rotz', 'tracker1RotQx', 'tracker1RotQy', 'tracker1RotQz', 'tracker1RotQw', 'relativeHandRPosx', 'relativeHandRPosy', 'relativeHandRPosz', 'relativeHandLPosx', 'relativeHandLPosy', 'relativeHandLPosz', 'relativeTracker1Posx', 'relativeTracker1Posy', 'relativeTracker1Posz']
        row = self.scaled_data[idx]
        # print(row)
        # print(keys.index('headPosx'))
        headPosx = row[keys.index('headPosx')]
        headPosy = row[keys.index('headPosy')]
        headPosz = row[keys.index('headPosz')]
        headRotx = row[keys.index('headRotx')]
        headRoty = row[keys.index('headRoty')]
        headRotz = row[keys.index('headRotz')]
        headRotQx = row[keys.index('headRotQx')]
        headRotQy = row[keys.index('headRotQy')]
        headRotQz = row[keys.index('headRotQz')]
        headRotQw = row[keys.index('headRotQw')]

        handRPosx = row[keys.index('handRPosx')]
        handRPosy = row[keys.index('handRPosy')]
        handRPosz = row[keys.index('handRPosz')]
        handRRotx = row[keys.index('handRRotx')]
        handRRoty = row[keys.index('handRRoty')]
        handRRotz = row[keys.index('handRRotz')]
        handRRotQx = row[keys.index('handRRotQx')]
        handRRotQy = row[keys.index('handRRotQy')]
        handRRotQz = row[keys.index('handRRotQz')]
        handRRotQw = row[keys.index('handRRotQw')]

        handLPosx = row[keys.index('handLPosx')]
        handLPosy = row[keys.index('handLPosy')]
        handLPosz = row[keys.index('handLPosz')]
        handLRotx = row[keys.index('handLRotx')]
        handLRoty = row[keys.index('handLRoty')]
        handLRotz = row[keys.index('handLRotz')]
        handLRotQx = row[keys.index('handLRotQx')]
        handLRotQy = row[keys.index('handLRotQy')]
        handLRotQz = row[keys.index('handLRotQz')]
        handLRotQw = row[keys.index('handLRotQw')]

        tracker1Posx = row[keys.index('tracker1Posx')]
        tracker1Posy = row[keys.index('tracker1Posy')]
        tracker1Posz = row[keys.index('tracker1Posz')]
        tracker1Rotx = row[keys.index('tracker1Rotx')]
        tracker1Roty = row[keys.index('tracker1Roty')]
        tracker1Rotz = row[keys.index('tracker1Rotz')]
        tracker1RotQx = row[keys.index('tracker1RotQx')]
        tracker1RotQy = row[keys.index('tracker1RotQy')]
        tracker1RotQz = row[keys.index('tracker1RotQz')]
        tracker1RotQw = row[keys.index('tracker1RotQw')]

        relativeHandRPosx = row[keys.index('relativeHandRPosx')]
        relativeHandRPosy = row[keys.index('relativeHandRPosy')]
        relativeHandRPosz = row[keys.index('relativeHandRPosz')]
        relativeHandLPosx = row[keys.index('relativeHandLPosx')]
        relativeHandLPosy = row[keys.index('relativeHandLPosy')]
        relativeHandLPosz = row[keys.index('relativeHandLPosz')]
        relativeTracker1Posx = row[keys.index('relativeTracker1Posx')]
        relativeTracker1Posy = row[keys.index('relativeTracker1Posy')]
        relativeTracker1Posz = row[keys.index('relativeTracker1Posz')]


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
                torch.FloatTensor([headPosx, headPosy, headPosz, headRotQx, headRotQy, headRotQz, headRotQw,
                    handRPosx, handRPosy, handRPosz, handRRotQx, handRRotQy, handRRotQz, handRRotQw,
                    handLPosx, handLPosy, handLPosz, handLRotQx, handLRotQy, handLRotQz, handLRotQw,
                    ]),
                torch.FloatTensor([tracker1Posx, tracker1Posy, tracker1Posz, tracker1RotQx, tracker1RotQy, 
                    tracker1RotQz, tracker1RotQw])
            )
        elif self.output_type=='both':
            return (
                torch.FloatTensor([headPosx, headPosy, headPosz, headRotx, headRoty, headRotz,headRotQx, headRotQy, headRotQz, headRotQw,
                    handRPosx, handRPosy, handRPosz, handRRotx, handRRoty, handRRotz, handRRotQx, handRRotQy, handRRotQz, handRRotQw,
                    handLPosx, handLPosy, handLPosz, handLRotx, handLRoty, handLRotz, handLRotQx, handLRotQy, handLRotQz, handLRotQw
                    ]),
                torch.FloatTensor([tracker1Posx, tracker1Posy, tracker1Posz, tracker1Rotx, tracker1Roty, 
                    tracker1Rotz, tracker1RotQx, tracker1RotQy, tracker1RotQz, tracker1RotQw])
            )
        elif self.output_type=='relative' or self.output_type=='relative_svm':
            # return (
            #     torch.FloatTensor([headPosy, headRotx, headRoty, headRotz,
            #         relativeHandRPosx, relativeHandRPosy, relativeHandRPosz, handRRotx, handRRoty, handRRotz,
            #         relativeHandLPosx, relativeHandLPosy, relativeHandLPosz, handLRotx, handLRoty, handLRotz,
            #         ]),
            #     torch.FloatTensor([relativeTracker1Posx, relativeTracker1Posy, relativeTracker1Posz,
            #         tracker1Rotx, tracker1Roty, tracker1Rotz])
            # )
            return (
                torch.FloatTensor(np.stack([headPosy, headRotQx, headRotQy, headRotQz, headRotQw,
                    relativeHandRPosx, relativeHandRPosy, relativeHandRPosz, handRRotQx, handRRotQy, handRRotQz, handRRotQw,
                    relativeHandLPosx, relativeHandLPosy, relativeHandLPosz, handLRotQx, handLRotQy, handLRotQz, handLRotQw
                    ],axis=-1)),
                torch.FloatTensor(np.stack([relativeTracker1Posx, relativeTracker1Posy, relativeTracker1Posz, #],axis=-1))
                    tracker1RotQx, tracker1RotQy, tracker1RotQz, tracker1RotQw],axis=-1))
            )


class GestureCSVDataset(torch.utils.data.Dataset):
    def __init__(self, root_path, output_type=Config['data_type']):
        files = os.listdir(root_path)
        files = [f for f in files if f.endswith('.csv')]
        all_data = []

        for file in files:
            csv_data = pd.read_csv(os.path.join(root_path, file))
            for i in range(0, 10):
                i = str(i)
                csv_data['relativeHandRPosx' + i] = csv_data['headPosx' + i] - csv_data['handRPosx' + i]
                csv_data['relativeHandRPosy' + i] = csv_data['headPosy' + i] - csv_data['handRPosy' + i]
                csv_data['relativeHandRPosz' + i] = csv_data['headPosz' + i] - csv_data['handRPosz' + i]
                csv_data['relativeHandLPosx' + i] = csv_data['headPosx' + i] - csv_data['handLPosx' + i]
                csv_data['relativeHandLPosy' + i] = csv_data['headPosy' + i] - csv_data['handLPosy' + i]
                csv_data['relativeHandLPosz' + i] = csv_data['headPosz' + i] - csv_data['handLPosz' + i]
            all_data.append(csv_data)
        # Concatenate all data
        self.data = pd.concat(all_data, axis=0, ignore_index=True)
        # Pull out the string gestures before scaling
        gestures = self.data['gesture'].astype('category')
        self.data = self.data.drop(columns='gesture')
        # Scale
        self.scaler = MinMaxScaler(feature_range=(-1, 1))
        self.scaler.fit(self.data)
        self.scaled_data = self.scaler.transform(self.data)
        # Put back the gestures/labels and add column names for easier access in __getitem__
        self.labels = dict(enumerate(gestures.cat.categories))
        self.scaled_data = np.append(self.scaled_data, np.reshape(gestures.cat.codes.values, (-1, 1)), 1)
        self.scaled_data = pd.DataFrame(self.scaled_data,
                                        columns=pd.Index(np.append(self.data.columns.values, 'gesture')))
        self.output_type = output_type

    def __len__(self):
        return len(self.data)

    def __getitem__(self, idx):
        row = self.scaled_data.iloc[idx]

        label = row['gesture']

        relativeHandRPos = []
        relativeHandLPos = []
        headRot = []
        handRRot = []
        handLRot = []
        for i in range(0, 10):
            i = str(i)
            relativeHandRPos.append(row['relativeHandRPosx' + i])
            relativeHandRPos.append(row['relativeHandRPosy' + i])
            relativeHandRPos.append(row['relativeHandRPosz' + i])
            relativeHandLPos.append(row['relativeHandLPosx' + i])
            relativeHandLPos.append(row['relativeHandLPosy' + i])
            relativeHandLPos.append(row['relativeHandLPosz' + i])
            headRot.append(row['headRotx' + i])
            headRot.append(row['headRoty' + i])
            headRot.append(row['headRotz' + i])
            handRRot.append(row['handRRotx' + i])
            handRRot.append(row['handRRoty' + i])
            handRRot.append(row['handRRotz' + i])
            handLRot.append(row['handLRotx' + i])
            handLRot.append(row['handLRoty' + i])
            handLRot.append(row['handLRotz' + i])

        if self.output_type == 'gesture':
            return (
                torch.FloatTensor(relativeHandRPos + relativeHandLPos + headRot + handRRot + handLRot),
                torch.tensor(label, dtype=torch.int64)
            )
