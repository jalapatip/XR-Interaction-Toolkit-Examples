using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;



public class DataCollection_Exp0Relative : DataCollection_ExpBase
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
        Dev.Log("Reload Xr Devices");
        head = Core.Ins.XRManager.GetXrCamera().gameObject;
        handR = Core.Ins.XRManager.GetRightDirectControllerGO();
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
            handRPos = handR.transform.position,
            handRRot = handR.transform.eulerAngles,
            handRRotQ = handR.transform.rotation,
            handLPos = handL.transform.position,
            handLRot = handL.transform.eulerAngles,
            handLRotQ = handL.transform.rotation,
            tracker1Pos = tracker1.transform.position,
            tracker1Rot = tracker1.transform.eulerAngles,
            tracker1RotQ = tracker1.transform.rotation
        };
        //currentRow.Add(data);
        //print(OutputData(currentRow));                                            
        print(DataContainer_Exp0.HeaderToString());
        print(data.ToString());                                                                        //can print data here to view live
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
        sb.Append(DataContainer_Exp0.HeaderToString());
        foreach (var d in dataList)
        {
            sb.Append(d.ToString());
        }
        return sb.ToString();
    }

    private string CompileDataAsJson()
    {
        var exp0data = JsonUtility.ToJson(dataList);
        return exp0data;
    }
}