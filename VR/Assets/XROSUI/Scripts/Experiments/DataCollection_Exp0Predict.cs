using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// https://docs.unity.cn/Packages/com.unity.barracuda@1.0/manual/GettingStarted.html
/// </summary>
public enum WaistPrediction
{
    ERot,
    QRot,
    BothRot,
    ERotRelative,
    QRotRelative,
    BothRotRelative,
    hackLSTM,
    LSTM
}

public class DataCollection_Exp0Predict : DataCollection_ExpBase
{
    public WaistPrediction waistPrediction;

    public GameObject _head;
    public GameObject _handR;
    public GameObject _handL;
    public bool startPlaying = false;

    public NNModel modelSource;
    public TextAsset scalerSource;
    private IWorker _worker;
    private Dictionary<string, Scaler> _scalers = new Dictionary<string, Scaler>();

    // Start is called before the first frame update
    private void Start()
    {        
        ExpName = "Exp0Predict";
        ReloadXrDevices();

        var model = ModelLoader.Load(modelSource);
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

        Scalers scalers = JsonUtility.FromJson<Scalers>(scalerSource.text);
        foreach (Scaler scaler in scalers.scalers)
        {
            _scalers.Add(scaler.type, scaler);
        }

        _isRecording = startPlaying;
    }

    private void ReloadXrDevices()
    {
        Dev.Log("Reload Xr Devices");
        if (!_head)
        {
            _head = Core.Ins.XRManager.GetXrCamera().gameObject;
            _handR = Core.Ins.XRManager.GetRightDirectControllerGO();
            _handL = Core.Ins.XRManager.GetLeftDirectController();
        }
    }

    public override void StartRecording()
    {
        base.StartRecording();
        ReloadXrDevices();
    }

    public override void StopRecording()
    {
        base.StopRecording();
    }

    public override void LateUpdate()
    {
        if (!_isRecording)
            return;

        Tensor inputTensor = null;
        switch (waistPrediction)
        {
            case WaistPrediction.ERot:
                inputTensor = CreateTensorUsingERot(_head.transform.position, _head.transform.eulerAngles,
                    _handR.transform.position, _handR.transform.eulerAngles, _handL.transform.position,
                    _handL.transform.eulerAngles);
                break;
            case WaistPrediction.QRot:
                inputTensor = CreateTensorUsingQRot(_head.transform.position, _head.transform.rotation,
                    _handR.transform.position, _handR.transform.rotation, _handL.transform.position,
                    _handL.transform.rotation);
                break;
            case WaistPrediction.BothRot:
                break;
            case WaistPrediction.ERotRelative:
                //inputTensor = CreateTensorUsingERotForRelative(_head.transform.localPosition, _head.transform.localEulerAngles, _head.transform.localPosition-_handR.transform.localPosition, _handR.transform.localEulerAngles, _head.transform.localPosition-_handL.transform.localPosition, _handL.transform.localEulerAngles);
                inputTensor = CreateTensorUsingERotForRelative(_head.transform.localPosition,
                    _head.transform.eulerAngles, _head.transform.localPosition - _handR.transform.localPosition,
                    _handR.transform.eulerAngles, _head.transform.localPosition - _handL.transform.localPosition,
                    _handL.transform.eulerAngles);
                break;
            case WaistPrediction.hackLSTM:

                inputTensor = CreateTensorUsingERotForHackLSTM();
                break;
            case WaistPrediction.LSTM:
                inputTensor = CreateTensorForLSTM();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (inputTensor != null)
        {
            //Powen: Is there a way to check whether the tensor given to the _worker is of the appropriate size?

            _worker.Execute(inputTensor);

            var output = _worker.PeekOutput();
            var tensorArray = output.ToReadOnlyArray();

            switch (waistPrediction)
            {
                case WaistPrediction.ERot:
                    UseTensorERot(tensorArray);
                    break;
                case WaistPrediction.QRot:
                    UseTensorQRot(tensorArray);
                    break;
                case WaistPrediction.BothRot:
                    break;
                case WaistPrediction.ERotRelative:
                    UseTensorERotRelative(tensorArray);
                    break;
                case WaistPrediction.hackLSTM:
                    UseTensorHackLSTM(tensorArray);
                    break;
                case WaistPrediction.LSTM:
                    var headPos = headPosArray.Last.Value;
                    UseTensorLSTM(tensorArray, headPos);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            inputTensor.Dispose();
            output.Dispose();
        }
    }


    private Tensor CreateTensorUsingQRot(Vector3 headPos, Quaternion headRotQ, Vector3 handRPos, Quaternion handRRotQ,
        Vector3 handLPos, Quaternion handLRotQ)
    {
        return new Tensor(1, 21, new float[21]
        {
            _scalers["headPosx"].Transform(headPos.x), _scalers["headPosy"].Transform(headPos.y),
            _scalers["headPosz"].Transform(headPos.z),
            _scalers["headRotQx"].Transform(headRotQ.x), _scalers["headRotQy"].Transform(headRotQ.y),
            _scalers["headRotQz"].Transform(headRotQ.z), _scalers["headRotQw"].Transform(headRotQ.w),
            _scalers["handRPosx"].Transform(handRPos.x), _scalers["handRPosy"].Transform(handRPos.y),
            _scalers["handRPosz"].Transform(handRPos.z),
            _scalers["handRRotQx"].Transform(handRRotQ.x), _scalers["handRRotQy"].Transform(handRRotQ.y),
            _scalers["handRRotQz"].Transform(handRRotQ.z), _scalers["handRRotQw"].Transform(handRRotQ.w),
            _scalers["handLPosx"].Transform(handLPos.x), _scalers["handLPosy"].Transform(handLPos.y),
            _scalers["handLPosz"].Transform(handLPos.z),
            _scalers["handLRotQx"].Transform(handLRotQ.x), _scalers["handLRotQy"].Transform(handLRotQ.y),
            _scalers["handLRotQz"].Transform(handLRotQ.z), _scalers["handLRotQw"].Transform(handLRotQ.w)
        });
    }

    private Tensor CreateTensorUsingERot(Vector3 headPos, Vector3 headRot, Vector3 handRPos, Vector3 handRRot,
        Vector3 handLPos, Vector3 handLRot)
    {
        return new Tensor(1, 18, new float[18]
        {
            _scalers["headPosx"].Transform(headPos.x), _scalers["headPosy"].Transform(headPos.y),
            _scalers["headPosz"].Transform(headPos.z),
            _scalers["headRotx"].Transform(headRot.x), _scalers["headRoty"].Transform(headRot.y),
            _scalers["headRotz"].Transform(headRot.z),
            _scalers["handRPosx"].Transform(handRPos.x), _scalers["handRPosy"].Transform(handRPos.y),
            _scalers["handRPosz"].Transform(handRPos.z),
            _scalers["handRRotx"].Transform(handRRot.x), _scalers["handRRoty"].Transform(handRRot.y),
            _scalers["handRRotz"].Transform(handRRot.z),
            _scalers["handLPosx"].Transform(handLPos.x), _scalers["handLPosy"].Transform(handLPos.y),
            _scalers["handLPosz"].Transform(handLPos.z),
            _scalers["handLRotx"].Transform(handLRot.x), _scalers["handLRoty"].Transform(handLRot.y),
            _scalers["handLRotz"].Transform(handLRot.z)
        });
    }

    private Tensor CreateTensorUsingERotForRelative(Vector3 headPos, Vector3 headRot, Vector3 handRPos,
        Vector3 handRRot, Vector3 handLPos, Vector3 handLRot)
    {
        return new Tensor(1, 16, new float[16]
            //return new Tensor(16, 1, new float[16]
            {
                //_scalers["headPosx"].Transform(headPos.x), _scalers["headPosy"].Transform(headPos.y), _scalers["headPosz"].Transform(headPos.z),
                _scalers["headPosy"].Transform(headPos.y),
                _scalers["headRotx"].Transform(headRot.x), _scalers["headRoty"].Transform(headRot.y),
                _scalers["headRotz"].Transform(headRot.z),
                _scalers["relativeHandRPosx"].Transform(handRPos.x),
                _scalers["relativeHandRPosy"].Transform(handRPos.y),
                _scalers["relativeHandRPosz"].Transform(handRPos.z),
                _scalers["handRRotx"].Transform(handRRot.x), _scalers["handRRoty"].Transform(handRRot.y),
                _scalers["handRRotz"].Transform(handRRot.z),
                _scalers["relativeHandLPosx"].Transform(handLPos.x),
                _scalers["relativeHandLPosy"].Transform(handLPos.y),
                _scalers["relativeHandLPosz"].Transform(handLPos.z),
                _scalers["handLRotx"].Transform(handLRot.x), _scalers["handLRoty"].Transform(handLRot.y),
                _scalers["handLRotz"].Transform(handLRot.z)
            });
    }

    private Tensor CreateTensorUsingERotForHackLSTM()
    {
        var headPostNode = headPosArray.First;
        var headRotNode = headRotArray.First;
        var handRPostNode = handRPosArray.First;
        var handRRotNode = handRRotArray.First;
        var handLPostNode = handLPosArray.First;
        var handLRotNode = handLRotArray.First;

        float[] _array = new float[80];
        int i = 79;
        for (int j = 0; j < 10; j++)
        {
            var headPos = headPostNode.Value;
            var headRot = headRotNode.Value;
            var handRPos = handRPostNode.Value;
            var handRRot = handRRotNode.Value;
            var handLPos = handLPostNode.Value;
            var handLRot = handLRotNode.Value;

            if (j % 2 == 0)
            {
                _array[i--] = _scalers["headPosy"].Transform(headPos.y);
                _array[i--] = _scalers["headRotx"].Transform(headRot.x);
                _array[i--] = _scalers["headRoty"].Transform(headRot.y);
                _array[i--] = _scalers["headRotz"].Transform(headRot.z);
                _array[i--] = _scalers["relativeHandRPosx"].Transform(handRPos.x);
                _array[i--] = _scalers["relativeHandRPosy"].Transform(handRPos.y);
                _array[i--] = _scalers["relativeHandRPosz"].Transform(handRPos.z);
                _array[i--] = _scalers["handRRotx"].Transform(handRRot.x);
                _array[i--] = _scalers["handRRoty"].Transform(handRRot.y);
                _array[i--] = _scalers["handRRotz"].Transform(handRRot.z);
                _array[i--] = _scalers["relativeHandLPosx"].Transform(handLPos.x);
                _array[i--] = _scalers["relativeHandLPosy"].Transform(handLPos.y);
                _array[i--] = _scalers["relativeHandLPosz"].Transform(handLPos.z);
                _array[i--] = _scalers["handLRotx"].Transform(handLRot.x);
                _array[i--] = _scalers["handLRoty"].Transform(handLRot.y);
                _array[i--] = _scalers["handLRotz"].Transform(handLRot.z);
            }

            headPostNode = headPostNode.Next;
            headRotNode = headRotNode.Next;
            handRPostNode = handRPostNode.Next;
            handRRotNode = handRRotNode.Next;
            handLPostNode = handLPostNode.Next;
            handLRotNode = handLRotNode.Next;
        }

        return new Tensor(1, 80, _array);
    }

    private Tensor CreateTensorForLSTM()
    {
        var headPosNode = headPosArray.First;
        //var headRotNode = headRotArray.First;
        var headRotNode = headRotArrayQ.First;
        var handRPosNode = handRPosArray.First;        
        //var handRRotNode = handRRotArray.First;
        var handRRotNode = handRRotArrayQ.First;
        var handLPosNode = handLPosArray.First;
        //var handLRotNode = handLRotArray.First;
        var handLRotNode = handLRotArrayQ.First;

        //10 is historical records
        //19 is the features
        var _array = new float[10 * 19];
        
        //last frame, 
        int i = 0;
        for(int j=0; j < 10; j++)
        {
            var headPos = headPosNode.Value;
            _array[i++] = _scalers["headPosy"].Transform(headPos.y);
            if (headPosNode.Next != null)
            {
                headPosNode = headPosNode.Next;
            }
        }
        for(int j=0; j < 10; j++)
        {
            var headRotNodeLoc = headRotArrayQ.First;
            _array[i++] = _scalers["headRotQx"].Transform(headRotNodeLoc.Value.x);
            if (headRotNodeLoc.Next != null)
            {
                headRotNodeLoc = headRotNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headRotNodeLoc = headRotArrayQ.First;
            _array[i++] = _scalers["headRotQy"].Transform(headRotNodeLoc.Value.y);
            if (headRotNodeLoc.Next != null)
            {
                headRotNodeLoc = headRotNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headRotNodeLoc = headRotArrayQ.First;
            _array[i++] = _scalers["headRotQz"].Transform(headRotNodeLoc.Value.z);
            if (headRotNodeLoc.Next != null)
            {
                headRotNodeLoc = headRotNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headRotNodeLoc = headRotArrayQ.First;
            _array[i++] = _scalers["headRotQw"].Transform(headRotNodeLoc.Value.w);
            if (headRotNodeLoc.Next != null)
            {
                headRotNodeLoc = headRotNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headPos = headPosNode.Value;
            var handRPosNodeLoc = handRPosArray.First;
            var relativeHandRPos = headPos - handRPosNodeLoc.Value;
            _array[i++] = _scalers["relativeHandRPosx"].Transform(relativeHandRPos.x);
            if (handRPosNodeLoc.Next != null)
            {
                handRPosNodeLoc = handRPosNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headPos = headPosNode.Value;
            var handRPosNodeLoc = handRPosArray.First;
            var relativeHandRPos = headPos - handRPosNodeLoc.Value;
            _array[i++] = _scalers["relativeHandRPosy"].Transform(relativeHandRPos.y);
            if (handRPosNodeLoc.Next != null)
            {
                handRPosNodeLoc = handRPosNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headPos = headPosNode.Value;
            var handRPosNodeLoc = handRPosArray.First;
            var relativeHandRPos = headPos - handRPosNodeLoc.Value;
            _array[i++] = _scalers["relativeHandRPosz"].Transform(relativeHandRPos.z);
            if (handRPosNodeLoc.Next != null)
            {
                handRPosNodeLoc = handRPosNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var handRRotQNodeLoc = handRRotArrayQ.First;
            _array[i++] = _scalers["handRRotQx"].Transform(handRRotQNodeLoc.Value.x);
            if (handRRotQNodeLoc.Next != null)
            {
                handRRotQNodeLoc = handRRotQNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var handRRotQNodeLoc = handRRotArrayQ.First;
            _array[i++] = _scalers["handRRotQy"].Transform(handRRotQNodeLoc.Value.y);
            if (handRRotQNodeLoc.Next != null)
            {
                handRRotQNodeLoc = handRRotQNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var handRRotQNodeLoc = handRRotArrayQ.First;
            _array[i++] = _scalers["handRRotQz"].Transform(handRRotQNodeLoc.Value.z);
            if (handRRotQNodeLoc.Next != null)
            {
                handRRotQNodeLoc = handRRotQNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var handRRotQNodeLoc = handRRotArrayQ.First;
            _array[i++] = _scalers["handRRotQw"].Transform(handRRotQNodeLoc.Value.w);
            if (handRRotQNodeLoc.Next != null)
            {
                handRRotQNodeLoc = handRRotQNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headPos = headPosNode.Value;
            var handLPosNodeLoc = handLPosArray.First;
            var relativeHandLPos = headPos - handLPosNodeLoc.Value;
            _array[i++] = _scalers["relativeHandLPosx"].Transform(relativeHandLPos.x);
            if (handLPosNodeLoc.Next != null)
            {
                handLPosNodeLoc = handLPosNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headPos = headPosNode.Value;
            var handLPosNodeLoc = handLPosArray.First;
            var relativeHandLPos = headPos - handLPosNodeLoc.Value;
            _array[i++] = _scalers["relativeHandLPosy"].Transform(relativeHandLPos.y);
            if (handLPosNodeLoc.Next != null)
            {
                handLPosNodeLoc = handLPosNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var headPos = headPosNode.Value;
            var handLPosNodeLoc = handLPosArray.First;
            var relativeHandLPos = headPos - handLPosNodeLoc.Value;
            _array[i++] = _scalers["relativeHandLPosz"].Transform(relativeHandLPos.z);
            if (handLPosNodeLoc.Next != null)
            {
                handLPosNodeLoc = handLPosNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var handLRotQNodeLoc = handLRotArrayQ.First;
            _array[i++] = _scalers["handLRotQx"].Transform(handLRotQNodeLoc.Value.x);
            if (handLRotQNodeLoc.Next != null)
            {
                handLRotQNodeLoc = handLRotQNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var handLRotQNodeLoc = handLRotArrayQ.First;
            _array[i++] = _scalers["handLRotQy"].Transform(handLRotQNodeLoc.Value.y);
            if (handLRotQNodeLoc.Next != null)
            {
                handLRotQNodeLoc = handLRotQNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var handLRotQNodeLoc = handLRotArrayQ.First;
            _array[i++] = _scalers["handLRotQz"].Transform(handLRotQNodeLoc.Value.z);
            if (handLRotQNodeLoc.Next != null)
            {
                handLRotQNodeLoc = handLRotQNodeLoc.Next;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            var handLRotQNodeLoc = handLRotArrayQ.First;
            _array[i++] = _scalers["handLRotQw"].Transform(handLRotQNodeLoc.Value.w);
            if (handLRotQNodeLoc.Next != null)
            {
                handLRotQNodeLoc = handLRotQNodeLoc.Next;
            }
        }

        /*
        for (int j = 0; j < this.GetTotalEntriesToTrack(); j++)
        {
            var headPos = headPosNode.Value;
            var headRot = headRotNode.Value;
            var handRPos = headPos - handRPosNode.Value;
            var handRRot = handRRotNode.Value;
            var handLPos = headPos - handLPosNode.Value;
            var handLRot = handLRotNode.Value;
            
            var evaluate = (j % (this.stepValue+1)) == 0;
//            print("j: " + j + " " + evaluate);
            if (evaluate)
            {
  //              print("i: " + i);
                _array[i++] = _scalers["headPosy"].Transform(headPos.y);
                _array[i++] = _scalers["headRotQx"].Transform(headRot.x);
                _array[i++] = _scalers["headRotQy"].Transform(headRot.y);
                _array[i++] = _scalers["headRotQz"].Transform(headRot.z);
                _array[i++] = _scalers["headRotQw"].Transform(headRot.w);
                _array[i++] = _scalers["relativeHandRPosx"].Transform(handRPos.x);
                _array[i++] = _scalers["relativeHandRPosy"].Transform(handRPos.y);
                _array[i++] = _scalers["relativeHandRPosz"].Transform(handRPos.z);
                _array[i++] = _scalers["handRRotQx"].Transform(handRRot.x);
                _array[i++] = _scalers["handRRotQy"].Transform(handRRot.y);
                _array[i++] = _scalers["handRRotQz"].Transform(handRRot.z);
                _array[i++] = _scalers["handRRotQw"].Transform(handRRot.w);
                _array[i++] = _scalers["relativeHandLPosx"].Transform(handLPos.x);
                _array[i++] = _scalers["relativeHandLPosy"].Transform(handLPos.y);
                _array[i++] = _scalers["relativeHandLPosz"].Transform(handLPos.z);
                _array[i++] = _scalers["handLRotQx"].Transform(handLRot.x);
                _array[i++] = _scalers["handLRotQy"].Transform(handLRot.y);
                _array[i++] = _scalers["handLRotQz"].Transform(handLRot.z);
                _array[i++] = _scalers["handLRotQw"].Transform(handLRot.w);
            }

            if (headPosNode.Next != null)
            {
                headPosNode = headPosNode.Next;
                headRotNode = headRotNode.Next;
                handRPosNode = handRPosNode.Next;
                handRRotNode = handRRotNode.Next;
                handLPosNode = handLPosNode.Next;
                handLRotNode = handLRotNode.Next;
            }
            else
            {
                //print("no more entries");
            }
        }*/
        /// Create a Tensor of shape [N,H,W,C], an array of data `srcData` and an optional debug `name`.
        /// `srcData` must be of size `n*h*w*c`.
        /// 

        //return new Tensor(10, 1, 1, 19, _array);
        // print("Input Tensor Array: " + _array);
        return new Tensor(1, 1, 19, 10, _array);
        
        //AssertionException: Assertion failure. Values are not equal.
        //Expected: 16 == 10
        //return new Tensor(16, 1, 1, 10, _array);
        
        
        //return new Tensor(10, 8, 2, 1, _array);
        //return new Tensor(1, 128, _array);
    }

// private Tensor CreateTensorUsingBothRot(Vector3 headPos, Quaternion headRotQ, Vector3 handRPos, Quaternion handRRotQ, Vector3 handLPos, Quaternion handLRotQ)
// {
//     return new Tensor(1, 21, new float[21]
//     {
//         headPos.x, headPos.y, headPos.z,
//         headRotQ.x, headRotQ.y, headRotQ.z, headRotQ.w, 
//         handRPos.x, handRPos.y, handRPos.z, 
//         handRRotQ.x, handRRotQ.y, handRRotQ.z, handRRotQ.w, 
//         handLPos.x, handLPos.y, handLPos.z, 
//         handLRotQ.x, handLRotQ.y, handLRotQ.z, handLRotQ.w
//     });
// }

    private void UseTensorQRot(float[] tensorArray)
    {
        var newPosition = new Vector3(_scalers["tracker1Posx"].InverseTransform(tensorArray[0]),
            _scalers["tracker1Posy"].InverseTransform(tensorArray[1]),
            _scalers["tracker1Posz"].InverseTransform(tensorArray[2]));
        this.gameObject.transform.position = newPosition;

        var newQuaternion = new Quaternion(_scalers["tracker1RotQx"].InverseTransform(tensorArray[3]),
            _scalers["tracker1RotQy"].InverseTransform(tensorArray[4]),
            _scalers["tracker1RotQz"].InverseTransform(tensorArray[5]),
            _scalers["tracker1RotQw"].InverseTransform(tensorArray[6]));
        this.gameObject.transform.rotation = newQuaternion;
    }

    private void UseTensorERot(float[] tensorArray)
    {
        var newPosition = new Vector3(_scalers["tracker1Posx"].InverseTransform(tensorArray[0]),
            _scalers["tracker1Posy"].InverseTransform(tensorArray[1]),
            _scalers["tracker1Posz"].InverseTransform(tensorArray[2]));
        this.gameObject.transform.position = newPosition;

        var newRotation = Quaternion.Euler(_scalers["tracker1Rotx"].InverseTransform(tensorArray[3]),
            _scalers["tracker1Roty"].InverseTransform(tensorArray[4]),
            _scalers["tracker1Rotz"].InverseTransform(tensorArray[5]));
        this.gameObject.transform.rotation = newRotation;
    }

    private void UseTensorERotRelative(float[] tensorArray)
    {
        var newPosition = new Vector3(_scalers["relativeTracker1Posx"].InverseTransform(tensorArray[0]),
            _scalers["relativeTracker1Posy"].InverseTransform(tensorArray[1]),
            _scalers["relativeTracker1Posz"].InverseTransform(tensorArray[2]));
        this.gameObject.transform.localPosition = _head.transform.localPosition - newPosition;

        var newRotation = Quaternion.Euler(_scalers["tracker1Rotx"].InverseTransform(tensorArray[3]),
            _scalers["tracker1Roty"].InverseTransform(tensorArray[4]),
            _scalers["tracker1Rotz"].InverseTransform(tensorArray[5]));
        this.gameObject.transform.rotation = newRotation;
    }

    private void UseTensorHackLSTM(float[] tensorArray)
    {
        var newPosition = new Vector3(_scalers["relativeTracker1Posx"].InverseTransform(tensorArray[0]),
            _scalers["relativeTracker1Posy"].InverseTransform(tensorArray[1]),
            _scalers["relativeTracker1Posz"].InverseTransform(tensorArray[2]));
        this.gameObject.transform.localPosition = _head.transform.localPosition - newPosition;

        var newRotation = Quaternion.Euler(_scalers["tracker1Rotx"].InverseTransform(tensorArray[3]),
            _scalers["tracker1Roty"].InverseTransform(tensorArray[4]),
            _scalers["tracker1Rotz"].InverseTransform(tensorArray[5]));
        this.gameObject.transform.eulerAngles = newRotation.eulerAngles;
    }

    private void UseTensorLSTM(float[] tensorArray, Vector3 headPos)
    {
        //print("Tensor Array: " + tensorArray[0] + tensorArray[1] + tensorArray[2]);
        var newPosition = new Vector3(_scalers["relativeTracker1Posx"].InverseTransform(tensorArray[0]),
            _scalers["relativeTracker1Posy"].InverseTransform(tensorArray[1]),
            _scalers["relativeTracker1Posz"].InverseTransform(tensorArray[2]));
        //print("New Position: " + newPosition.x + newPosition.y + newPosition.z);
        this.gameObject.transform.localPosition = headPos - newPosition;
        //this.gameObject.transform.position = _head.transform.position - newPosition;
        //this.gameObject.transform.localPosition = newPosition;

        // var newRotation = Quaternion.Euler(_scalers["tracker1Rotx"].InverseTransform(tensorArray[3]),
        //     _scalers["tracker1Roty"].InverseTransform(tensorArray[4]),
        //     _scalers["tracker1Rotz"].InverseTransform(tensorArray[5]));
        // this.gameObject.transform.rotation = newRotation;

        var newRotation = new Quaternion(_scalers["tracker1RotQx"].InverseTransform(tensorArray[3]),
            _scalers["tracker1RotQy"].InverseTransform(tensorArray[4]),
            _scalers["tracker1RotQz"].InverseTransform(tensorArray[5]),
            _scalers["tracker1RotQw"].InverseTransform(tensorArray[6]));
        this.gameObject.transform.eulerAngles = newRotation.eulerAngles;

    }

    private LinkedList<Vector3> headPosArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> headRotArray = new LinkedList<Vector3>();
    private LinkedList<Quaternion> headRotArrayQ = new LinkedList<Quaternion>();
    private LinkedList<Vector3> handRPosArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> handRRotArray = new LinkedList<Vector3>();
    private LinkedList<Quaternion> handRRotArrayQ = new LinkedList<Quaternion>();
    private LinkedList<Vector3> handLPosArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> handLRotArray = new LinkedList<Vector3>();
    private LinkedList<Quaternion> handLRotArrayQ = new LinkedList<Quaternion>();
    
// Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            this._isRecording = true;
        }

        TrackNewData();
    }

    private int stepValue = 1;
    private int historyDataNeeded = 10;

    private int GetTotalEntriesToTrack()
    {
        /*
         * we want 10 entries
        step value of 1
        that means we need to have 20 data points saved
        if we want step value of 2
        we need 10*(2+1);
         */
        return (stepValue + 1) * historyDataNeeded;
        //return 10;
    }

//We go from oldest entry to the latest
//so ideally our linkedlist's head is the oldest and the newest is at the end

    private void TrackNewData()
    {
        headPosArray.AddLast(_head.transform.localPosition);
        headRotArray.AddLast(_head.transform.eulerAngles);
        handRPosArray.AddLast(_handR.transform.localPosition);
        handRRotArray.AddLast(_handR.transform.eulerAngles);
        handLPosArray.AddLast(_handL.transform.localPosition);
        handLRotArray.AddLast(_handL.transform.eulerAngles);
        headRotArrayQ.AddLast(_handR.transform.localRotation);
        handRRotArrayQ.AddLast(_head.transform.localRotation);
        handLRotArrayQ.AddLast(_handL.transform.localRotation);
        

        if (headPosArray.Count > this.GetTotalEntriesToTrack())
        {
            // var node = headPostArray.First;
            // var toPrint = "";
            // while (node != null)
            // {
            //     toPrint += node.Value + " - ";
            //     node = node.Next;
            // }
            //
            // print(toPrint);
            headPosArray.RemoveFirst();
            headRotArray.RemoveFirst();
            headRotArrayQ.RemoveFirst();
            handRPosArray.RemoveFirst();
            handRRotArray.RemoveFirst();
            handRRotArrayQ.RemoveFirst();
            handLPosArray.RemoveFirst();
            handLRotArray.RemoveFirst();
            handLRotArrayQ.RemoveFirst();
        }

        // if (headRotArray.Count > this.GetTotalEntriesToTrack())
        // {
        //     headRotArray.RemoveFirst();
        //     headRotArrayQ.RemoveFirst();
        // }
        // if (handRPostArray.Count > this.GetTotalEntriesToTrack())
        //     handRPostArray.RemoveFirst();
        // if (handRRotArray.Count > this.GetTotalEntriesToTrack())
        // {
        //     handRRotArray.RemoveFirst();
        //     handRRotArrayQ.RemoveFirst();
        // }
        // if (handLPostArray.Count > this.GetTotalEntriesToTrack())
        //     handLPostArray.RemoveFirst();
        // if (handLRotArray.Count > this.GetTotalEntriesToTrack())
        // {
        //     handLRotArray.RemoveFirst();
        //     handLRotArrayQ.RemoveFirst();
        // }
    }

    private void DebugUpdate()
    {
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     StartRecording();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     StopRecording();
        // }
        //
        // //EVENT_NewUser?.Invoke(s);
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     Debug.Log("[Debug] DataCollection: WriteAsCsv");
        // }
        //
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     Debug.Log("[Debug] DataCollection: WriteAsJson");
        // }
    }
}