using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Exp 1 is the Equipment Gesture
/// We track the position and rotation for headset, left hand controller, right hand controller
/// We ask the user to perform different equipment gestures
/// </summary>
public class DataCollection_Exp1Gestures : DataCollection_ExpBase, IWriteToFile
{
    // To be set in Unity to determine which gesture we are currently collecting data for
    [FormerlySerializedAs("gesture")]
    public ENUM_XROS_EquipmentGesture targetGesture;

    private bool _completedGesture = false;
    private bool _doGesture = false;
    private int _gestureCount = 0;

    #region Setup

    private void Awake()
    {
        ExpName = "Exp1";
        Core.Ins.DataCollection.RegisterExperiment(this);
    }

    private void OnEnable()
    {
        Controller_XR.EVENT_NewPosition += OnNewPosition;
    }

    private void OnDisable()
    {
        Controller_XR.EVENT_NewPosition -= OnNewPosition;
    }

    #endregion Setup

    public void StartGesture()
    {
        _doGesture = true;
    }

    public void EndGesture()
    {
        _completedGesture = true;
    }

    public override void LateUpdate()
    {
    }

    // Update is called once per frame
    public override void Update()
    {
        //DebugUpdate();
    }

    void DebugUpdate()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            print("W");
            print(Core.Ins.DataCollection.GetCurrentExperiment().ExpName);
            print(Core.Ins.DataCollection.GetCurrentExperiment().OutputHeaderString());
        }
    }

    public void OnNewPosition(PositionSample sample)
    {
        // if (_completedGesture)
        // {
        //     _gestureList.Add(targetGesture);
        //     _completedGesture = false;
        //     _gestureCount++;
        // }
        // else
        // {
        //     _gestureList.Add(ENUM_XROS_EquipmentGesture.None);
        // }

        var data = new DataContainer_Exp1GesturesPosition
        {
            timestamp = sample.timestamp,
            headPos = sample.headPos,
            headRot = sample.headRot,
            headRotQ = sample.headRotQ,
            handRPos = sample.handRPos,
            handRRot = sample.handRRot,
            handRRotQ = sample.handRRotQ,
            handLPos = sample.handLPos,
            handLRot = sample.handLRot,
            handLRotQ = sample.handLRotQ,
        };

        if (_doGesture)
        {
            //_gestureList.Add(targetGesture);
            data.gesture = targetGesture.ToString();
            if (_completedGesture)
            {
                _completedGesture = false;
                _doGesture = false;
                _gestureCount++;
            }
        }
        else
        {
            //_gestureList.Add(ENUM_XROS_EquipmentGesture.None);
            data.gesture = ENUM_XROS_EquipmentGesture.None.ToString();
        }

        dataList.Add(data);
    }

    // public override string OutputData()
    // {
    //     var positions = Core.Ins.XRManager.GetLastPositionSamples(_gestureList.Count);
    //     print("Outputting data");
    //     var sb = new StringBuilder();
    //     sb.Append(HeaderToString());
    //     for (int i = 0; i < _gestureList.Count; i++)
    //     {
    //         sb.Append(positions[i]);
    //         sb.Append(_gestureList[i]);
    //     }
    //     return sb.ToString();
    // }

    public void ChangeExperimentType(ENUM_XROS_EquipmentGesture gesturesToRecord)
    {
        Dev.Log("Collecting gesture of type " + gesturesToRecord);
        this.targetGesture = gesturesToRecord;
    }

    public override void RemoveLastEntry()
    {
        string noneString = ENUM_XROS_EquipmentGesture.None.ToString();
        for (int i = dataList.Count - 1; i >= 0; i--)
        {
            DataContainer_Exp1GesturesPosition data = (DataContainer_Exp1GesturesPosition)dataList[i];

            if (!data.gesture.Equals(noneString))
            {
                data.gesture = noneString;
                _gestureCount--;
                return;
            }
        }
    }

    public override string OutputHeaderString()
    {
        return DataContainer_Exp1GesturesPosition.HeaderToString();
    }

    public override int GetTotalEntries()
    {
        return _gestureCount;
    }

    public override string GetGoalString()
    {
        return "Current Type: " + this.targetGesture.ToString() + "\n" + this.GetTotalEntries();
    }
}