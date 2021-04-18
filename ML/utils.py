Config={}

Config['dataset_path']='Data_Exp/Exp0'
Config['model_path']='models/lstm_alldata_run6'
Config['onnx_model_path']='exp0_lstm_model_final.onnx'
Config['tensorboard_log']=True
Config['use_cuda']=True

Config['lr']=1e-3
Config['num_epochs']=1000
Config['batch_size']=256
Config['num_workers']=2
Config['data_type'] = 'relative' # one of "euler/quaternion/both/relative"
