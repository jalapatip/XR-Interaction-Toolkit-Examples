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
    hackLSTM
}

[System.Serializable]
public class Scaler
{
    // Note: Variables must be public for the JSONUtility to load correctly
    public string type;
    public float min;
    public float scale;
    public float data_min;
    public float data_max;
    public float data_range;
    public int n_samples_seen;

    public float Transform(float input)
    {
        return input * scale + min;
    }

    public float InverseTransform(float input)
    {
        return (input - min) / scale;
    }
}

[System.Serializable]
public class Scalers
{
    public Scaler[] scalers;
}

public class DataCollection_Exp0Predict : DataCollection_ExpBase
{
    public WaistPrediction waistPrediction;

    public GameObject _head;
    public GameObject _handR;
    public GameObject _handL;

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
    }

    private void ReloadXrDevices()
    {
        Dev.Log("Reload Xr Devices");
        if (!_head)
        {
            _head = Core.Ins.XRManager.GetXrCamera().gameObject;
            _handR = Core.Ins.XRManager.GetRightDirectController();
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

                inputTensor = CreateTensorUsingERotForHackLSTM(_head.transform.localPosition,
                    _head.transform.eulerAngles, _head.transform.localPosition - _handR.transform.localPosition,
                    _handR.transform.eulerAngles, _head.transform.localPosition - _handL.transform.localPosition,
                    _handL.transform.eulerAngles);
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
                    UseTensorLSTM(tensorArray);
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
        {
            //_scalers["headPosx"].Transform(headPos.x), _scalers["headPosy"].Transform(headPos.y), _scalers["headPosz"].Transform(headPos.z),
            _scalers["headPosy"].Transform(headPos.y),
            _scalers["headRotx"].Transform(headRot.x), _scalers["headRoty"].Transform(headRot.y),
            _scalers["headRotz"].Transform(headRot.z),
            _scalers["relativeHandRPosx"].Transform(handRPos.x), _scalers["relativeHandRPosy"].Transform(handRPos.y),
            _scalers["relativeHandRPosz"].Transform(handRPos.z),
            _scalers["handRRotx"].Transform(handRRot.x), _scalers["handRRoty"].Transform(handRRot.y),
            _scalers["handRRotz"].Transform(handRRot.z),
            _scalers["relativeHandLPosx"].Transform(handLPos.x), _scalers["relativeHandLPosy"].Transform(handLPos.y),
            _scalers["relativeHandLPosz"].Transform(handLPos.z),
            _scalers["handLRotx"].Transform(handLRot.x), _scalers["handLRoty"].Transform(handLRot.y),
            _scalers["handLRotz"].Transform(handLRot.z)
        });
    }


    private Tensor CreateTensorUsingERotForHackLSTM(Vector3 headPos, Vector3 headRot, Vector3 handRPos,
        Vector3 handRRot, Vector3 handLPos, Vector3 handLRot)
    {
        // public LinkedList<Vector3> headPostArray = new LinkedList<Vector3>();
        // private LinkedList<Vector3> headRotArray = new LinkedList<Vector3>();
        // private LinkedList<Vector3> handRPostArray = new LinkedList<Vector3>();
        // private LinkedList<Vector3> handRRotArray = new LinkedList<Vector3>();
        // private LinkedList<Vector3> handLPostArray = new LinkedList<Vector3>();
        // private LinkedList<Vector3> handLRotArray = new LinkedList<Vector3>();
        //     

        var headPostNode = headPostArray.First;
        var headRotNode = headRotArray.First;
        var handRPostNode = handRPostArray.First;
        var handRRotNode = handRRotArray.First;
        var handLPostNode = handLPostArray.First;
        var handLRotNode = handLRotArray.First;

        float[] _array = new float[80];
        int i = 79;
        for (int j = 0; j < 10; j++)
        {
            headPos = headPostNode.Value;
            headRot = headRotNode.Value;
            handRPos = handRPostNode.Value;
            handRRot = handRRotNode.Value;
            handLPos = handLPostNode.Value;
            handLRot = handLRotNode.Value;

            if (j % 2 ==0)
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
    private void UseTensorLSTM(float[] tensorArray)
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

    public LinkedList<Vector3> headPostArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> headRotArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> handRPostArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> handRRotArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> handLPostArray = new LinkedList<Vector3>();

    private LinkedList<Vector3> handLRotArray = new LinkedList<Vector3>();

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            this._isRecording = true;
        }
        TrackNewData();
    }

    private void TrackNewData()
    {
        headPostArray.AddLast(_head.transform.localPosition);
        headRotArray.AddLast(_head.transform.eulerAngles);
        handRPostArray.AddLast(_handR.transform.localPosition);
        handRRotArray.AddLast(_handR.transform.eulerAngles);
        handLPostArray.AddLast(_handL.transform.localPosition);
        handLRotArray.AddLast(_handL.transform.eulerAngles);


        if (headPostArray.Count > 10)
        {
            var node = headPostArray.First;
            var toPrint = "";
            while (node != null)
            {
                toPrint += node.Value +" - ";
                node = node.Next;
            }
            print(toPrint);
            headPostArray.RemoveFirst();
        }
        if (headRotArray.Count > 10)
            headRotArray.RemoveFirst();
        if (handRPostArray.Count > 10)
            handRPostArray.RemoveFirst();
        if (handRRotArray.Count > 10)
            handRRotArray.RemoveFirst();
        if (handLPostArray.Count > 10)
            handLPostArray.RemoveFirst();
        if (handLRotArray.Count > 10)
            handLRotArray.RemoveFirst();
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