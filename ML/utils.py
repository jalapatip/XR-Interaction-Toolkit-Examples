Config={}

Config['dataset_path']='data'
Config['model_path']='run0'
Config['onnx_model_path']='model0.onnx'
Config['tensorboard_log']=True
Config['use_cuda']=True

Config['lr']=1e-3
Config['num_epochs']=1000
Config['batch_size']=512
Config['num_workers']=4
Config['data_type'] = 'quaternion' # one of "euler/quaternion/both"
