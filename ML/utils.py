Config={}

Config['dataset_path']='data'
Config['model_path']='lstm_run1'
Config['onnx_model_path']='model_final.onnx'
Config['tensorboard_log']=True
Config['use_cuda']=True

Config['lr']=1e-3
Config['num_epochs']=1000
Config['batch_size']=128
Config['num_workers']=8
Config['data_type'] = 'hacklstm' # one of "euler/quaternion/both/relative"
