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

    private static string _headerString;

    #region Setup
    private void Start()
    {
        ExpName = "Exp2";
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
    }

    public void OnNewPosition(PositionSample sample)
    {
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

        if (_completedGesture)
        {
            //_gestureList.Add(targetGesture);
            data.gesture = targetLocation.ToString();
            _completedGesture = false;
            _gestureCount++;
            RandomizeLocationToGrab();
        }
        else
        {
            //_gestureList.Add(ENUM_XROS_EquipmentGesture.None);
            data.gesture = ENUM_XROS_PeripersonalEquipmentLocations.None.ToString();
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
        string noneString = ENUM_XROS_EquipmentGesture.None.ToString();
        for (int i = dataList.Count - 1; i >= 0; i--)
        {
            DataContainer_Exp1GesturesPosition data = (DataContainer_Exp1GesturesPosition) dataList[i];

            if (!data.gesture.Equals(noneString))
            {
                data.gesture = noneString;
                _gestureCount--;
                return;
            }
        }
    }

    public int GetTotalEntries()
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