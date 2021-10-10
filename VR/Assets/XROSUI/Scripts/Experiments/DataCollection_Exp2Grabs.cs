using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

/// <summary>
/// Exp 2 is the Peripersonal Equipment Slot
/// We track the position and rotation for headset, left hand controller, right hand controller
/// We ask the user to grab different equipment slots in the peripersonal space
/// </summary>
public class DataCollection_Exp2Grabs : DataCollection_ExpBase, IWriteToFile
{
    // To be set in Unity to determine which gesture we are currently collecting data for
    [FormerlySerializedAs("gesture")]
    public ENUM_XROS_PeripersonalEquipmentLocations targetLocation = ENUM_XROS_PeripersonalEquipmentLocations._0300;

    private bool _completedGesture = false;
    private int _gestureCount = 0;
    private bool _startGesture = false;
    
    private static string _headerString;

    #region Setup
    private void Start()
    {
        ExpName = "Exp2";
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
        _startGesture = true;
    }

    public void EndGesture()
    {
        _completedGesture = true;
        _startGesture = false; 
    }

    public override void LateUpdate()
    {
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    public void OnNewPosition(PositionSample sample)
    {
        if (!_isRecording)
        {
            return;
        }
        var data = new DataContainer_Exp2Peripersonal()
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
            tracker1Pos = sample.tracker1Pos,
            tracker1Rot = sample.tracker1Rot,
            tracker1RotQ = sample.tracker1RotQ
        };

        if (_startGesture)
        {
            data.gesture = targetLocation.ToString();
        }
        else
        {
            data.gesture = ENUM_XROS_PeripersonalEquipmentLocations.None.ToString();
        }
        if (_completedGesture)
        {
            _completedGesture = false;
            _gestureCount++;
            RandomizeLocationToGrab();
        }
        
        dataList.Add(data);
    }


    public void ChangeExperimentType(ENUM_XROS_PeripersonalEquipmentLocations gesturesToRecord)
    {
        Dev.Log("Collecting gesture of type " + gesturesToRecord);
        this.targetLocation = gesturesToRecord;
    }

    public override void RemoveLastEntry()
    {
        if (_gestureCount <= 0) return;
        bool remove_flag = false; 
        for (int i = dataList.Count - 1; i >= 0; i--)
        {
            DataContainer_Exp2Peripersonal data = (DataContainer_Exp2Peripersonal) dataList[i];
            string noneString = ENUM_XROS_EquipmentGesture.None.ToString();
            if (!data.gesture.Equals(noneString))
            {
                while (!data.gesture.Equals(noneString) && i >= 0)
                {
                    data.gesture = noneString;
                    data = (DataContainer_Exp2Peripersonal) dataList[i - 1];
                    i = i - 1; 
                }
                remove_flag = true; 
            }
            if (remove_flag) break;
        }
        _gestureCount--;

    }

    public override int GetTotalEntries()
    {
        return _gestureCount;
    }

    public override string OutputHeaderString()
    {
        return DataContainer_Exp2Peripersonal.HeaderToString();
    }

    public void RandomizeLocationToGrab()
    {
        targetLocation = (ENUM_XROS_PeripersonalEquipmentLocations) Random.Range(1, 10);
    }

    public override string GetGoalString()
    {
        return "Current Type: " + this.targetLocation.ToString() + "\n" + this.GetTotalEntries();
    }
}