Config={}

Config['dataset_path']='Data_Exp/Exp1'
Config['model_path']='models/lstm_classifier_and_ad_sl'
Config['onnx_model_path']='exp1_lstm_model_final.onnx'
Config['tensorboard_log']=True
Config['use_cuda']=True

Config['lr']=1e-3
Config['num_epochs']=1000
Config['batch_size']=256
Config['num_workers']=2
Config['data_type'] = 'relative' # one of "euler/quaternion/both/relative"
