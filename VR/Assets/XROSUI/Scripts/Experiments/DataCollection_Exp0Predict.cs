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
[Serializable]
public class DataContainer_Exp0Predict
{
    public Vector3 headPos;
    public Quaternion headRotQ; //Quaternion
    public Vector3 handRPos;
    public Quaternion handRRotQ;
    public Vector3 handLPos;
    public Quaternion handLRotQ;
    public Vector3 tracker1Pos;
    public Quaternion tracker1RotQ;
}

public enum WaistPrediction {ERot, QRot, BothRot}

public class DataCollection_Exp0Predict : DataCollection_ExpBase
{
    public WaistPrediction waistPrediction; 
    
    private GameObject _head;
    private GameObject _handR;
    private GameObject _handL;
    
    public NNModel modelSource;
    private IWorker _worker;
        
    // Start is called before the first frame update
    private void Start()
    {
        ExpName = "Exp0Predict";
        ReloadXrDevices();
        
        var model = ModelLoader.Load(modelSource);
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    private void ReloadXrDevices()
    {
        Dev.Log("Reload Xr Devices");
        _head = Core.Ins.XRManager.GetXrCamera().gameObject;
        _handR = Core.Ins.XRManager.GetRightDirectController();
        _handL = Core.Ins.XRManager.GetLeftDirectController();
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
        //if (!_isRecording)
//            return;

        var data = new DataContainer_Exp0Predict
        {
            headPos = _head.transform.position,
            headRotQ = _head.transform.rotation,
            handRPos = _handR.transform.position,
            handRRotQ = _handR.transform.rotation,
            handLPos = _handL.transform.position,
            handLRotQ = _handL.transform.rotation,
        };

        Tensor inputTensor = null;
        switch (waistPrediction)
        {
            case WaistPrediction.ERot:
                inputTensor = CreateTensorUsingERot(_head.transform.position, _head.transform.eulerAngles, _handR.transform.position, _handR.transform.eulerAngles, _handL.transform.position, _handL.transform.eulerAngles);
                break;
            case WaistPrediction.QRot:
                inputTensor = CreateTensorUsingQRot(data);
                inputTensor = CreateTensorUsingQRot2(_head.transform.position, _head.transform.rotation, _handR.transform.position, _handR.transform.rotation, _handL.transform.position, _handL.transform.rotation);
                break;
            case WaistPrediction.BothRot:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (inputTensor != null)
        {
            //Powen: Is there a way to check whether the tensor given to the _worker is of the appropriate size?
            _worker.Execute(inputTensor);

            var output = _worker.PeekOutput();
            print("This is the prediction output: " + output);
            var tensorArray = output.ToReadOnlyArray();
     
            UseTensor1(tensorArray);
            //UseTensor2(array);
        
            inputTensor.Dispose();
            output.Dispose();
        }
    }

    //CreateTensor1 is the one Sloan originally had
    //CreateTensor2 is Quaternion rptatopm only where we avoid using a data structure (presumably better performance)
    //CreateTensor3 is Vector3 rotation only 
    private Tensor CreateTensorUsingQRot(DataContainer_Exp0Predict data)
    {
        return new Tensor(1, 21, new float[21]
        {
            data.headPos.x, data.headPos.y, data.headPos.z,
            data.headRotQ.x, data.headRotQ.y, data.headRotQ.z, data.headRotQ.w, 
            data.handRPos.x, data.handRPos.y, data.handRPos.z, 
            data.handRRotQ.x, data.handRRotQ.y, data.handRRotQ.z, data.handRRotQ.w, 
            data.handLPos.x, data.handLPos.y, data.handLPos.z, 
            data.handLRotQ.x, data.handLRotQ.y, data.handLRotQ.z, data.handLRotQ.w
        });
    }

    private Tensor CreateTensorUsingQRot2(Vector3 headPos, Quaternion headRotQ, Vector3 handRPos, Quaternion handRRotQ, Vector3 handLPos, Quaternion handLRotQ)
    {
        return new Tensor(1, 21, new float[21]
        {
            headPos.x, headPos.y, headPos.z,
            headRotQ.x, headRotQ.y, headRotQ.z, headRotQ.w, 
            handRPos.x, handRPos.y, handRPos.z, 
            handRRotQ.x, handRRotQ.y, handRRotQ.z, handRRotQ.w, 
            handLPos.x, handLPos.y, handLPos.z, 
            handLRotQ.x, handLRotQ.y, handLRotQ.z, handLRotQ.w
        });
    }

    private Tensor CreateTensorUsingERot(Vector3 headPos, Vector3 headRot, Vector3 handRPos, Vector3 handRRot, Vector3 handLPos, Vector3 handLRot)
    {
        return new Tensor(1, 18, new float[18]
        {
            headPos.x, headPos.y, headPos.z,
            headRot.x, headRot.y, headRot.z, 
            handRPos.x, handRPos.y, handRPos.z, 
            handRRot.x, handRRot.y, handRRot.z, 
            handLPos.x, handLPos.y, handLPos.z, 
            handLRot.x, handLRot.y, handLRot.z,
        });
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
    
    private void UseTensor1(float[] tensorArray)
    {
        var newPosition = new Vector3(tensorArray[0], tensorArray[1], tensorArray[2]);
        this.gameObject.transform.position = newPosition;
        
        var newQuaternion = new Quaternion(tensorArray[3],tensorArray[4], tensorArray[5], tensorArray[7]);
        this.gameObject.transform.rotation = newQuaternion;
//        print("Pos: " + newPosition);
//        print("Rot: " + newQuaternion);
    }

    // Update is called once per frame
    public override void Update()
    {
        //DebugUpdate();
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