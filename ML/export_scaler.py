from data import CSVDataset
from utils import Config

import json
import os
import argparse
from shutil import copy, rmtree

if __name__=='__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--csv_file', default=None)
    args = parser.parse_args()
    headers = [
        'headPosx', 
        'headPosy', 
        'headPosz', 
        'headRotx', 
        'headRoty', 
        'headRotz', 
        'headRotQx', 
        'headRotQy', 
        'headRotQz', 
        'headRotQw', 
        'handRPosx', 
        'handRPosy', 
        'handRPosz', 
        'handRRotx', 
        'handRRoty', 
        'handRRotz', 
        'handRRotQx', 
        'handRRotQy', 
        'handRRotQz', 
        'handRRotQw',
        'handLPosx', 
        'handLPosy', 
        'handLPosz', 
        'handLRotx', 
        'handLRoty', 
        'handLRotz', 
        'handLRotQx', 
        'handLRotQy', 
        'handLRotQz', 
        'handLRotQw',
        'tracker1Posx', 
        'tracker1Posy', 
        'tracker1Posz', 
        'tracker1Rotx', 
        'tracker1Roty', 
        'tracker1Rotz', 
        'tracker1RotQx', 
        'tracker1RotQy', 
        'tracker1RotQz', 
        'tracker1RotQw',
        ]
    if args.csv_file:
        os.makedirs('tmp/', exist_ok=True)
        copy(args.csv_file, 'tmp/.')
        dataset = CSVDataset(root_path='tmp')
        rmtree('tmp/')
    else:
        dataset = CSVDataset(root_path=Config['dataset_path'])
    with open(os.path.join('scaler.json'), 'w') as f:
        scaler = {'scalers': []}
        for idx, header in enumerate(headers):
            scaler['scalers'].append({
                'type': header,
                'min': dataset.scaler.min_.tolist()[idx],
                'scale':dataset.scaler.scale_.tolist()[idx],
                'data_min':dataset.scaler.data_min_.tolist()[idx],
                'data_max': dataset.scaler.data_max_.tolist()[idx],
                'data_range': dataset.scaler.data_range_.tolist()[idx],
                'n_samples_seen': dataset.scaler.n_samples_seen_
            })
        json.dump(scaler, f)