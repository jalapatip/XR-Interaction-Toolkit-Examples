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

    if args.csv_file:
        os.makedirs('tmp/', exist_ok=True)
        copy(args.csv_file, 'tmp/.')
        dataset = CSVDataset(root_path='tmp')
        rmtree('tmp/')
    else:
        dataset = CSVDataset(root_path=Config['dataset_path'])
    with open(os.path.join('scaler.json'), 'w') as f:
        scaler = {
            'min_':dataset.scaler.min_.tolist(),
            'scale_':dataset.scaler.scale_.tolist(),
            'data_min_':dataset.scaler.data_min_.tolist(),
            'data_max_': dataset.scaler.data_max_.tolist(),
            'data_range_': dataset.scaler.data_range_.tolist(),
            'n_samples_seen': dataset.scaler.n_samples_seen_
        }
        json.dump(scaler, f)