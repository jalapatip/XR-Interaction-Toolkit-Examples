using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// Exp 0 is the waist tracking experiment.
/// We track the position and rotation for headset, left hand controller, right hand controller, and the tracker in the waist.
/// </summary>
public class DataCollection_Exp0 : DataCollection_ExpBase, IWriteToFile
{
    private GameObject _head;
    private GameObject _handR;
    private GameObject _handL;
    private GameObject _tracker1;

    // Start is called before the first frame update
    private void Start()
    {
        ExpName = "Exp0";
        Core.Ins.DataCollection.RegisterExperiment(this);
        ReloadXrDevices();
    }

    private void ReloadXrDevices()
    {
        Dev.Log("Reload Xr Devices");
        _head = Core.Ins.XRManager.GetXrCamera().gameObject;
        _handR = Core.Ins.XRManager.GetRightDirectControllerGO();
        _handL = Core.Ins.XRManager.GetLeftDirectController();
        _tracker1 = Core.Ins.XRManager.GetTracker();
    }

    public override void LateUpdate()
    {
        if (!_tracker1)
        {
            _tracker1 = Core.Ins.XRManager.GetTracker();
        }

        if (!_isRecording)
            return;

        var data = new DataContainer_Exp0
        {
            timestamp = Time.time,
            headPos = _head.transform.localPosition,
            headRot = _head.transform.eulerAngles,
            headRotQ = _head.transform.rotation,
            handRPos = _handR.transform.localPosition,
            handRRot = _handR.transform.eulerAngles,
            handRRotQ = _handR.transform.rotation,
            handLPos = _handL.transform.localPosition,
            handLRot = _handL.transform.eulerAngles,
            handLRotQ = _handL.transform.rotation,
            tracker1Pos = _tracker1.transform.localPosition,
            tracker1Rot = _tracker1.transform.eulerAngles,
            tracker1RotQ = _tracker1.transform.rotation
        };

        //print(data.ToString());      // print data to console live
        dataList.Add(data);
    }
    
    public override string OutputHeaderString()
    {
        return DataContainer_Exp0.HeaderToString();
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