using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DataCollection_ExpGestures : DataCollection_ExpBase, IWriteToFile
{
    // To be set in Unity to determine which gesture we are currently collecting data for
    public ENUM_XROS_EquipmentGesture gesture;
    public XRGrabInteractable grabInteractable;
    
    // List of entire data for the experiment
    private List<ENUM_XROS_EquipmentGesture> _gestureList = new List<ENUM_XROS_EquipmentGesture>();

    private bool _completedGesture = false;
    private int _gestureCount = 0;

    private static string _headerString;
    
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        grabInteractable.onSelectEnter.AddListener(StartGesture);
        grabInteractable.onSelectExit.AddListener(EndGesture);
        Controller_XR.EVENT_NewPosition += OnNewPosition;
    }

    private void OnDisable()
    {
        grabInteractable.onSelectEnter.RemoveListener(StartGesture);
        grabInteractable.onSelectExit.RemoveListener(EndGesture);
        Controller_XR.EVENT_NewPosition -= OnNewPosition;
    }

    private void StartGesture(XRBaseInteractor arg0)
    {
    }

    public override void LateUpdate()
    {
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    public void OnNewPosition()
    {
        if (_completedGesture)
        {
            _gestureList.Add(gesture);
            _completedGesture = false;
            _gestureCount++;
        }
        else
        {
            _gestureList.Add(ENUM_XROS_EquipmentGesture.None);
        }
    }

    public override string OutputFileName()
    {
        return "Exp1" + "_"  + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".csv";
    }

    public override string OutputData()
    {
        var positions = Core.Ins.XRManager.GetLastPositionSamples(_gestureList.Count);
        print("Outputting data");
        var sb = new StringBuilder();
        sb.Append(HeaderToString());
        for (int i = 0; i < _gestureList.Count; i++)
        {
            sb.Append(positions[i]);
            sb.Append(_gestureList[i]);
        }
        return sb.ToString();
    }

    public void EndGesture(XRBaseInteractor xrBaseInteractor)
    {
        _completedGesture = true;
    }

    public void ChangeExperimentType(ENUM_XROS_EquipmentGesture gesturesToRecord)
    {
        Dev.Log("Collecting gesture of type " + gesturesToRecord);
        this.gesture = gesturesToRecord;
    }

    public void RemoveLastGesture()
    {
        // todo: In gestureList find the last non-None gesture and set it to None
    }

    public int GetTotalEntries()
    {
        return _gestureCount;
    }
    
    public static string HeaderToString()
    {
        if (_headerString == null)
        {
            _headerString = nameof(PositionSample.timestamp) + ","
                             + nameof(PositionSample.headPos) + "x,"
                             + nameof(PositionSample.headPos) + "y,"
                             + nameof(PositionSample.headPos) + "z,"
                             + nameof(PositionSample.headRot) + "x,"
                             + nameof(PositionSample.headRot) + "y,"
                             + nameof(PositionSample.headRot) + "z,"
                             + nameof(PositionSample.headRotQ) + "x,"
                             + nameof(PositionSample.headRotQ) + "y,"
                             + nameof(PositionSample.headRotQ) + "z,"
                             + nameof(PositionSample.headRotQ) + "w,"
                             + nameof(PositionSample.handRPos) + "x,"
                             + nameof(PositionSample.handRPos) + "y,"
                             + nameof(PositionSample.handRPos) + "z,"
                             + nameof(PositionSample.handRRot) + "x,"
                             + nameof(PositionSample.handRRot) + "y,"
                             + nameof(PositionSample.handRRot) + "z,"
                             + nameof(PositionSample.handRRotQ) + "x,"
                             + nameof(PositionSample.handRRotQ) + "y,"
                             + nameof(PositionSample.handRRotQ) + "z,"
                             + nameof(PositionSample.handRRotQ) + "w,"
                             + nameof(PositionSample.handLPos) + "x,"
                             + nameof(PositionSample.handLPos) + "y,"
                             + nameof(PositionSample.handLPos) + "z,"
                             + nameof(PositionSample.handLRot) + "x,"
                             + nameof(PositionSample.handLRot) + "y,"
                             + nameof(PositionSample.handLRot) + "z,"
                             + nameof(PositionSample.handLRotQ) + "x,"
                             + nameof(PositionSample.handLRotQ) + "y,"
                             + nameof(PositionSample.handLRotQ) + "z,"
                             + nameof(PositionSample.handLRotQ) + "w,"
                             + nameof(gesture);
        }

        return _headerString;
    }
}
