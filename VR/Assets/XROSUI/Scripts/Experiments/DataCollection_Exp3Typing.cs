using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = System.Random;

/// <summary>
/// Exp 3 is the Text Entry
/// We track the position and rotation for headset, left hand controller, right hand controller
/// We ask the user to "type" different keys
/// </summary>
public class DataCollection_Exp3Typing : DataCollection_ExpBase, IWriteToFile
{
    public XRGrabInteractable grabInteractable;
    
    // List of entire data for the experiment
    private List<string> _keyList = new List<string>();
    private List<string> _handList = new List<string>();

    private bool _startedKeyType = false;
    private bool _completedKeyType = false;
    private int _entriesCount = 0;
    private Random _rand = new Random();
    private string _targetKey;
    private List<string> _leftHandKeys = new List<string> {"Q", "W", "E", "R", "T", "A", "S", "D", "F", "G", "Z", "X", "C", "V", "B"};
    private List<string> _rightHandKeys = new List<string> {"Y", "U", "I", "O", "P", "H", "J", "K", "L", ";", "N", "M", ",", ".", "?"};

    private static string _headerString;
    
    private void Start()
    {
        ExpName = "Exp3";
    }
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        _targetKey = _leftHandKeys[_rand.Next(_leftHandKeys.Count)];
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
        _startedKeyType = true;
        //print(Core.Ins.XRManager.CheckInteractorType(arg0));
        //print(Core.Ins.XRManager.CheckLeftOrRightController(arg0));
    }

    public void EndGesture(XRBaseInteractor xrBaseInteractor)
    {
        _completedKeyType = true;
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
        if (_completedKeyType)
        {
            print("Done adding key " + _targetKey);
            _keyList.Add(_targetKey);
            _startedKeyType = false;
            _completedKeyType = false;
            if (_entriesCount % 2 == 0)
            {
                _handList.Add("left");
                _targetKey = _rightHandKeys[_rand.Next(_rightHandKeys.Count)];
            }
            else
            {
                _handList.Add("right");
                _targetKey = _leftHandKeys[_rand.Next(_leftHandKeys.Count)];
            }

            _entriesCount++;
        }
        else if (_startedKeyType)
        {
            print("Adding key " + _targetKey);
            _keyList.Add(_targetKey);
        }
        else
        {
            _keyList.Add("null");
        }
    }

    // public override string OutputFileName()
    // {
    //     return "Exp3" + "_"  + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".csv";
    // }

    public override string OutputData()
    {
        var positions = Core.Ins.XRManager.GetLastPositionSamples(_keyList.Count);
        print("Outputting data");
        var sb = new StringBuilder();
        sb.Append(HeaderToString());
        for (int i = 0; i < _keyList.Count; i++)
        {
            sb.Append(positions[i]);
            sb.Append(_keyList[i]);
            sb.Append(",");
            sb.Append(_handList[i]);
        }
        return sb.ToString();
    }

    public void RemoveLastGesture()
    {
        for (int i = _keyList.Count - 1; i > -1; i--)
        {
            if (_keyList[i] != "null")
            {
                while (_keyList[i] != "null")
                {
                    _keyList[i] = "null";
                    i--;
                }

                return;
            }
        }
    }

    public override int GetTotalEntries()
    {
        return _entriesCount;
    }

    public string GetTargetKey()
    {
        return _targetKey;
    }

    public override string GetGoalString()
    {
        string hand;
        if (_entriesCount % 2 == 0)
        {
            hand = "left";
        }
        else
        {
            hand = "right";
        }
        return "Enter Key: " + _targetKey + " with your " + hand + " hand\nTotal Entries: " + _entriesCount;
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
                             + "key,hand";
        }

        return _headerString;
    }
}
