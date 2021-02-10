using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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

public class DataCollection_Exp0Predict : DataCollection_ExpBase
{
    public List<string> Headers = new List<string>();
    public List<GameObject> GOs = new List<GameObject>();

    private GameObject head;
    private GameObject handR;
    private GameObject handL;
    
    public NNModel modelSource;
    private IWorker worker;
        
    // Start is called before the first frame update
    private void Start()
    {
        ExpName = "Exp0Predict";
        ReloadXrDevices();
        
        var model = ModelLoader.Load(modelSource);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        /*
        var inputTensor = new Tensor(1, 2, new float[2] {0, 0});
        worker.Execute(inputTensor);

        var output = worker.PeekOutput();
        print("This is the output: " + output);
            
        inputTensor.Dispose();
        output.Dispose();
        worker.Dispose();
        */
    }

    private void ReloadXrDevices()
    {
        Debug.Log("Reload Xr Devices");
        head = Core.Ins.XRManager.GetXrCamera().gameObject;
        handR = Core.Ins.XRManager.GetRightDirectController();
        handL = Core.Ins.XRManager.GetLeftDirectController();
        //dataList = new List<DataContainer_Exp0>();
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
            headPos = head.transform.position,
            headRotQ = head.transform.rotation,
            handRPos = handR.transform.position,
            handRRotQ = handR.transform.rotation,
            handLPos = handL.transform.position,
            handLRotQ = handL.transform.rotation,
        };

        var inputTensor = new Tensor(1, 21, new float[21]
        {
            data.headPos.x, data.headPos.y, data.headPos.z,
            data.headRotQ.x, data.headRotQ.y, data.headRotQ.z, data.headRotQ.w, data.handRPos.x, data.handRPos.y,
            data.handRPos.z, data.handRRotQ.x, data.handRRotQ.y, data.handRRotQ.z, data.handRRotQ.w, data.handLPos.x,
            data.handLPos.y, data.handLPos.z, data.handLRotQ.x, data.handLRotQ.y, data.handLRotQ.z, data.handLRotQ.w
        });
        worker.Execute(inputTensor);

        var output = worker.PeekOutput();
        print("This is the prediction output: " + output);
        var array = output.ToReadOnlyArray();
        
        var newPosition = new Vector3(array[0], array[1], array[2]);
        this.gameObject.transform.position = newPosition;
        print("Pos: " + newPosition);
        var newQuaternion = new Quaternion(array[3],array[4], array[5], array[7]);
        this.gameObject.transform.rotation = newQuaternion;
        print("Rot: " + newQuaternion);
        
        inputTensor.Dispose();
        output.Dispose();
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