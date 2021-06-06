import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.spatial.transform import Rotation as R


#this script is used to put the data recorded using brekel into the same format as data recorded in unity. Can test the script  by comparing the data in brekelFile and unityFile.
#brekelFile and unityFile contain the same data recorded simultaneously using brekel and unity. (slight differences in when each program started recording get handled below)
#after applying transformations to the brekel data (and making one transformation to the unity data), the resulting BrekelR and UnityR dataframes should be about the same.
brekelFile = r'C:\Users\14157\Desktop\BrekelTesting\BrekelOutput_unprocessed.csv'
unityFile = r'C:\Users\14157\Desktop\BrekelTesting\UnityOutput_unprocessed.csv'

brekel = pd.read_csv(brekelFile)
unity = pd.read_csv(unityFile)

brekel = brekel[brekel.timestamp > 14.75]  #brekel started recording data about 14.75s before unity
brekel = brekel[brekel['timestamp'] < 117.13] #brekel finished recording after unity

brekelR = pd.DataFrame()

brekelR['timestamp'] = brekel['timestamp'] - 14.75

brekelR['headPosx'] = brekel['HMD0_tx']
brekelR['headPosy'] = brekel['HMD0_ty']
brekelR['headPosz'] = -brekel['HMD0_tz']
brekelR['headRotx'] = (brekel['HMD0_rx'] * -1 + 360) % 360
brekelR['headRoty'] = (brekel['HMD0_ry'] * -1 + 360) % 360
brekelR['headRotz'] = (brekel['HMD0_rz'] + 360) % 360
brekelR[['headRotQx', 'headRotQy', 'headRotQz', 'headRotQw']] = np.array(
    (R.from_euler('xyz', brekelR[['headRotx', 'headRoty', 'headRotz']], degrees=True)).as_quat())


brekelR['handRPosx'] = brekel['HMD0_tx'] - brekel['controller1_tx']
brekelR['handRPosy'] = brekel['HMD0_ty'] - brekel['controller1_ty']
brekelR['handRPosz'] = -(brekel['HMD0_tz'] - brekel['controller1_tz'])
brekelR['handRRotx'] = ((brekel['HMD0_rx'] - brekel['controller1_rx']) * -1 + 360) % 360
brekelR['handRRoty'] = ((brekel['HMD0_ry'] - brekel['controller1_ry']) * -1 + 360) % 360
brekelR['handRRotz'] = ((brekel['HMD0_rz'] - brekel['controller1_rz']) + 360) % 360
brekelR[['handRRotQx', 'handRRotQy', 'handRRotQz', 'handRRotQw']] = np.array(
    (R.from_euler('xyz', brekelR[['handRRotx', 'handRRoty', 'handRRotz']], degrees=True)).as_quat())

brekelR['handLPosx'] = brekel['HMD0_tx'] - brekel['controller2_tx']
brekelR['handLPosy'] = brekel['HMD0_ty'] - brekel['controller2_ty']
brekelR['handLPosz'] = -(brekel['HMD0_tz'] - brekel['controller2_tz'])
brekelR['handLRotx'] = ((brekel['HMD0_rx'] - brekel['controller2_rx']) * -1 + 360) % 360
brekelR['handLRoty'] = ((brekel['HMD0_ry'] - brekel['controller2_ry']) * -1 + 360) % 360
brekelR['handLRotz'] = ((brekel['HMD0_rz'] - brekel['controller2_rz']) + 360) % 360
brekelR[['handLRotQx', 'handLRotQy', 'handLRotQz', 'handLRotQw']] = np.array(
    (R.from_euler('xyz', brekelR[['handLRotx', 'handLRoty', 'handLRotz']], degrees=True)).as_quat())

brekelR['tracker1Posx'] = brekel['HMD0_tx'] - brekel['generic5_tx']
brekelR['tracker1Posy'] = brekel['HMD0_ty'] - brekel['generic5_ty']
brekelR['tracker1Posz'] = -(brekel['HMD0_tz'] - brekel['generic5_tz'])
brekelR['tracker1Rotx'] = ((brekel['HMD0_rx'] - brekel['generic5_rx']) * -1 + 360) % 360
brekelR['tracker1Roty'] = ((brekel['HMD0_ry'] - brekel['generic5_ry']) * -1 + 360) % 360
brekelR['tracker1Rotz'] = ((brekel['HMD0_rz'] - brekel['generic5_rz']) + 360) % 360
brekelR[['tracker1RotQx', 'tracker1RotQy', 'tracker1RotQz', 'tracker1RotQw']] = np.array(
    (R.from_euler('xyz', brekelR[['tracker1Rotx', 'tracker1Roty', 'tracker1Rotz']], degrees=True)).as_quat())

brekelR[['headPosx', 'headPosy', 'headPosz', 'handRPosx', 'handRPosy', 'handRPosz', 'handLPosx','handLPosy','handLPosz','tracker1Posx','tracker1Posy','tracker1Posz']] = brekelR[['headPosx', 'headPosy', 'headPosz', 'handRPosx', 'handRPosy', 'handRPosz', 'handLPosx','handLPosy','handLPosz','tracker1Posx','tracker1Posy','tracker1Posz']] / 100

unityR = pd.DataFrame()

unityR['timestamp'] = unity['timestamp'] - 4.89

unityR['headPosx'] = unity['headPosx']
unityR['headPosy'] = unity['headPosy']
unityR['headPosz'] = unity['headPosz']
unityR['headRotx'] = unity['headRotx']
unityR['headRoty'] = unity['headRoty']
unityR['headRotz'] = unity['headRotz']

unityR['handRPosx'] = unity['headPosx'] - unity['HandRPosx']
unityR['handRPosy'] = unity['headPosy'] - unity['HandRPosy']
unityR['handRPosz'] = unity['headPosz'] - unity['HandRPosz']

unityR['handLPosx'] = unity['headPosx'] - unity['handLPosx']
unityR['handLPosy'] = unity['headPosy'] - unity['handLPosy']
unityR['handLPosz'] = unity['headPosz'] - unity['handLPosz']
unityR['handLRotx'] = ((unity['headRotx'] - unity['handLRotx']) + 360) % 360
unityR['handLRoty'] = ((unity['headRoty'] - unity['handLRoty']) + 360) % 360
unityR['handLRotz'] = ((unity['headRotz'] - unity['handLRotz']) + 360) % 360

unityR['tracker1Posx'] = unity['headPosx'] - unity['tracker1Posx']
unityR['tracker1Posy'] = unity['headPosy'] - unity['tracker1Posy']
unityR['tracker1Posz'] = unity['headPosz'] - unity['tracker1Posz']
unityR['tracker1Rotx'] = ((unity['headRotx'] - unity['tracker1Rotx']) + 360) % 360
unityR['tracker1Roty'] = ((unity['headRoty'] - unity['tracker1Roty']) + 360) % 360
unityR['tracker1Rotz'] = ((unity['headRotz'] - unity['tracker1Rotz']) + 360) % 360


# brekelR.plot(x='timestamp', y='headPosy', title = "brekel")
# unityR.plot(x='timestamp', y='headPosy', title = "unity")
# brekelR.plot(x='timestamp', y='headPosz', title = "brekel")
# unityR.plot(x='timestamp', y='headPosz', title = "unity")
# brekelR.plot(x='timestamp', y='handRPosx', title = "brekel")
# unityR.plot(x='timestamp', y='handRPosx', title = "unity")
# brekelR.plot(x='timestamp', y='handRPosy', title = "brekel")
# unityR.plot(x='timestamp', y='handRPosy', title = "unity")
# brekelR.plot(x='timestamp', y='handRPosz', title = "brekel")
# unityR.plot(x='timestamp', y='handRPosz', title = "unity")
# brekelR.plot(x='timestamp', y='handLPosx', title = "brekel")
# unityR.plot(x='timestamp', y='handLPosx', title = "unity")
# brekelR.plot(x='timestamp', y='handLPosy', title = "brekel")
# unityR.plot(x='timestamp', y='handLPosy', title = "unity")
# brekelR.plot(x='timestamp', y='handLPosz', title = "brekel")
# unityR.plot(x='timestamp', y='handLPosz', title = "unity")
# brekelR.plot(x='timestamp', y='tracker1Posx', title = "brekel")
# unityR.plot(x='timestamp', y='tracker1Posx', title = "unity")
# brekelR.plot(x='timestamp', y='tracker1Posy', title = "brekel")
# unityR.plot(x='timestamp', y='tracker1Posy', title = "unity")
# brekelR.plot(x='timestamp', y='tracker1Posz', title = "brekel")
# unityR.plot(x='timestamp', y='tracker1Posz', title = "unity")

# brekelR.plot(x = 'timestamp',y = 'headRotx', title = "brekel")
# unityR.plot(x = 'timestamp',y = 'headRotx', title = "unity")
# brekelR.plot(x='timestamp', y='headRoty', title = "brekel")
# unityR.plot(x='timestamp', y='headRoty', title = "unity")
# brekelR.plot(x='timestamp', y='headRotz', title = "brekel")
# unityR.plot(x='timestamp', y='headRotz', title = "unity")
# brekelR.plot(x='timestamp', y='handRRotx', title = "brekel")
# unityR.plot(x='timestamp', y='handRRotx', title = "unity")
# brekelR.plot(x='timestamp', y='handRRoty', title = "brekel")
# unityR.plot(x='timestamp', y='handRRoty', title = "unity")
# brekelR.plot(x='timestamp', y='handRRotz', title = "brekel")
# unityR.plot(x='timestamp', y='handRRotz', title = "unity")
# brekelR.plot(x='timestamp', y='handLRotx', title = "brekel")
# unityR.plot(x='timestamp', y='handLRotx', title = "unity")
# brekelR.plot(x='timestamp', y='handLRoty', title = "brekel")
# unityR.plot(x='timestamp', y='handLRoty', title = "unity")
# brekelR.plot(x='timestamp', y='handLRotz', title = "brekel")
# unityR.plot(x='timestamp', y='handLRotz', title = "unity")
# brekelR.plot(x='timestamp', y= 'tracker1Rotx', title = "brekel")
# unityR.plot(x='timestamp', y= 'tracker1Rotx', title = "unity")
# brekelR.plot(x='timestamp', y='tracker1Roty', title = "brekel")
# unityR.plot(x='timestamp', y='tracker1Roty', title = "unity")
# brekelR.plot(x='timestamp', y='tracker1Rotz', title = "brekel")
# unityR.plot(x='timestamp', y='tracker1Rotz', title = "unity")

plt.show()

brekelR.to_csv(r'C:\Users\14157\Desktop\BrekelTesting\BrekelOutput.csv',index = False)
unityR.to_csv(r'C:\Users\14157\Desktop\BrekelTesting\UnityOutput.csv', index = False)