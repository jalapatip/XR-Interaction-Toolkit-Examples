import torch

from utils import Config
from model import Regressor
from data import CSVDataset
if __name__=='__main__':
    dataset = CSVDataset(root_path=Config['dataset_path'])
    if Config['data_type']=='euler':
        model = Regressor(input_size=18, output_size=6)
    elif Config['data_type']=='quaternion':
        model = Regressor(input_size=21, output_size=7)
    elif Config['data_type']=='both':
        model = Regressor(input_size=30, output_size=10)
    elif Config['data_type']=='relative':
        model = Regressor(input_size=16, output_size=6)

    device = torch.device('cuda:0' if torch.cuda.is_available() and Config['use_cuda'] else 'cpu')
    model.load_state_dict(torch.load('/home/adityan/Studies/CSCI527/relative_fixed/run6/checkpoints/model_final.pth'))
    model.to(device)
    
    test_loader = torch.utils.data.DataLoader(
                        dataset,
                        batch_size = Config['batch_size'],
                        shuffle=True,
                        num_workers = Config['num_workers']
                    )

    for inputs, labels in test_loader:
        with torch.set_grad_enabled(False):
            inputs = inputs.to(device)
            labels = labels.to(device)

            outputs = model(inputs)

            print ('Expected: ', labels)
            print('Predicted: ', outputs)
