using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class DataContainer_Exp0
{
    public float timestamp;
    public Vector3 headPos;
    public Vector3 headRot; //Euler Angles
    public Quaternion headRotQ; //Quaternion
    public Vector3 HandRPos { get; set; }
    public Vector3 handRRot;
    public Quaternion handRRotQ;
    public Vector3 handLPos;
    public Vector3 handLRot;
    public Quaternion handLRotQ;
    public Vector3 tracker1Pos;
    public Vector3 tracker1Rot;
    public Quaternion tracker1RotQ;
}

public class DataCollection_Exp0 : DataCollection_ExpBase
{
    
    
    public List<string> Headers = new List<string>();
    public List<GameObject> GOs = new List<GameObject>();

    private List<DataContainer_Exp0> dataList = new List<DataContainer_Exp0>();

    private GameObject head;
    private GameObject handR;
    private GameObject handL;
    private GameObject tracker1;

    // Start is called before the first frame update
    private void Start()
    {
        ExpName = "Exp0";
        ReloadXrDevices();
    }

    private void ReloadXrDevices()
    {
        Debug.Log("Reload Xr Devices");
        head = Core.Ins.XRManager.GetXrCamera().gameObject;
        handR = Core.Ins.XRManager.GetRightDirectController();
        handL = Core.Ins.XRManager.GetLeftDirectController();
        tracker1 = Core.Ins.XRManager.GetTracker();
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
        if (!tracker1)
        {
            tracker1 = Core.Ins.XRManager.GetTracker();
        }

        if (!_isRecording)
            return;

        var data = new DataContainer_Exp0
        {
            timestamp = Time.time,
            headPos = head.transform.position,
            headRot = head.transform.eulerAngles,
            headRotQ = head.transform.rotation,
            HandRPos = handR.transform.position,
            handRRot = handR.transform.eulerAngles,
            handRRotQ = handR.transform.rotation,
            handLPos = handL.transform.position,
            handLRot = handL.transform.eulerAngles,
            handLRotQ = handL.transform.rotation,
            tracker1Pos = tracker1.transform.position,
            tracker1Rot = tracker1.transform.eulerAngles,
            tracker1RotQ = tracker1.transform.rotation
        };
        
        
        dataList.Add(data);
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
    
    public override string OutputFileName()
    {
        return ExpName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".csv";
        
    }
    public override string OutputData()
    {
        var sb = new StringBuilder();
        sb.Append(nameof(DataContainer_Exp0.timestamp) + "," +
                  nameof(DataContainer_Exp0.headPos) + "x," +
                  nameof(DataContainer_Exp0.headPos) + "y," +
                  nameof(DataContainer_Exp0.headPos) + "z," +
                  nameof(DataContainer_Exp0.headRot) + "x," +
                  nameof(DataContainer_Exp0.headRot) + "y," +
                  nameof(DataContainer_Exp0.headRot) + "z," +
                  nameof(DataContainer_Exp0.headRotQ) + "x," +
                  nameof(DataContainer_Exp0.headRotQ) + "y," +
                  nameof(DataContainer_Exp0.headRotQ) + "z," +
                  nameof(DataContainer_Exp0.headRotQ) + "w," +
                  nameof(DataContainer_Exp0.HandRPos) + "x," +
                  nameof(DataContainer_Exp0.HandRPos) + "y," +
                  nameof(DataContainer_Exp0.HandRPos) + "z," +
                  nameof(DataContainer_Exp0.handRRot) + "x," +
                  nameof(DataContainer_Exp0.handRRot) + "y," +
                  nameof(DataContainer_Exp0.handRRot) + "z," +
                  nameof(DataContainer_Exp0.handRRotQ) + "x," +
                  nameof(DataContainer_Exp0.handRRotQ) + "y," +
                  nameof(DataContainer_Exp0.handRRotQ) + "z," +
                  nameof(DataContainer_Exp0.handRRotQ) + "w," +
                  nameof(DataContainer_Exp0.handLPos) + "x," +
                  nameof(DataContainer_Exp0.handLPos) + "y," +
                  nameof(DataContainer_Exp0.handLPos) + "z," +
                  nameof(DataContainer_Exp0.handLRot) + "x," +
                  nameof(DataContainer_Exp0.handLRot) + "y," +
                  nameof(DataContainer_Exp0.handLRot) + "z," +
                  nameof(DataContainer_Exp0.handLRotQ) + "x," +
                  nameof(DataContainer_Exp0.handLRotQ) + "y," +
                  nameof(DataContainer_Exp0.handLRotQ) + "z," +
                  nameof(DataContainer_Exp0.handLRotQ) + "w," +
                  nameof(DataContainer_Exp0.tracker1Pos) + "x," +
                  nameof(DataContainer_Exp0.tracker1Pos) + "y," +
                  nameof(DataContainer_Exp0.tracker1Pos) + "z," +
                  nameof(DataContainer_Exp0.tracker1Rot) + "x," +
                  nameof(DataContainer_Exp0.tracker1Rot) + "y," +
                  nameof(DataContainer_Exp0.tracker1Rot) + "z," +
                  nameof(DataContainer_Exp0.tracker1RotQ) + "x," +
                  nameof(DataContainer_Exp0.tracker1RotQ) + "y," +
                  nameof(DataContainer_Exp0.tracker1RotQ) + "z," +
                  nameof(DataContainer_Exp0.tracker1RotQ) + "w,");
        foreach (var d in dataList)
        {
            sb.Append("\n" + d.timestamp + ","
                      + d.headPos.x + ","
                      + d.headPos.y + ","
                      + d.headPos.z + ","
                      + d.headRot.x + ","
                      + d.headRot.y + ","
                      + d.headRot.z + ","
                      + d.headRotQ.x + ","
                      + d.headRotQ.y + ","
                      + d.headRotQ.z + ","
                      + d.headRotQ.w + ","
                      + d.HandRPos.x + ","
                      + d.HandRPos.y + ","
                      + d.HandRPos.z + ","
                      + d.handRRot.x + ","
                      + d.handRRot.y + ","
                      + d.handRRot.z + ","
                      + d.handRRotQ.x + ","
                      + d.handRRotQ.y + ","
                      + d.handRRotQ.z + ","
                      + d.handRRotQ.w + ","
                      + d.handLPos.x + ","
                      + d.handLPos.y + ","
                      + d.handLPos.z + ","
                      + d.handLRot.x + ","
                      + d.handLRot.y + ","
                      + d.handLRot.z + ","
                      + d.handLRotQ.x + ","
                      + d.handLRotQ.y + ","
                      + d.handLRotQ.z + ","
                      + d.handLRotQ.w + ","
                      + d.tracker1Pos.x + ","
                      + d.tracker1Pos.y + ","
                      + d.tracker1Pos.z + ","
                      + d.tracker1Rot.x + ","
                      + d.tracker1Rot.y + ","
                      + d.tracker1Rot.z + ","
                      + d.tracker1RotQ.x + ","
                      + d.tracker1RotQ.y + ","
                      + d.tracker1RotQ.z + ","
                      + d.tracker1RotQ.w);
        }

        return sb.ToString();
    }

    private string CompileDataAsJson()
    {
        var exp0data = JsonUtility.ToJson(dataList);
        return exp0data;
    }
}