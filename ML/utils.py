Config={}

Config['dataset_path']='Data_Exp/Exp1'
Config['model_path']='gestures_run1'
Config['onnx_model_path']='gesture_model_final.onnx'
Config['tensorboard_log']=True
Config['use_cuda']=False

Config['lr']=1e-3
Config['num_epochs']=1000
Config['batch_size']=128
Config['num_workers']=8
Config['data_type'] = 'gesture' # one of "euler/quaternion/both/relative"
