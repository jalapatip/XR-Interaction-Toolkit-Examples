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

    private int _onNewPositionCount = 0;
    private List<PositionSample> _currentStartSample;
    private bool _notContinueKeyType = false;
    GameObject _startReference;
    GameObject _endReference;

    private List<float> _distanceLeftList = new List<float>();
    private List<float> _rotQLeftList = new List<float>();
    private List<float> _distanceRightList = new List<float>();
    private List<float> _rotQRightList = new List<float>();
    private List<string> _isStartGesture = new List<string>();
    private List<LocalPositionSample> _localPositionSamples = new List<LocalPositionSample>();

    private void Start()
    {
        ExpName = "Exp3";
        Core.Ins.DataCollection.RegisterExperiment(this);

        _startReference = GameObject.Find("StartReference");
        _endReference = GameObject.Find("EndReference");
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

        _notContinueKeyType = true;
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
        //print("=============================================================");
        //_onNewPositionCount++;
        //print("-----OnNewPositionCount: " + _onNewPositionCount + "-----");

        //int _lastPositionsCount = Core.Ins.XRManager.GetLastPositionCount();
        //print("-----LastPositionsCount: " + _lastPositionsCount + "-----");

        //var _lastPosition = Core.Ins.XRManager.GetLastPositionSamples(1);
        //var sb = new StringBuilder();
        //sb.Append(_lastPosition[0]);
        //print("-----Data: " + sb.ToString() + "-----");
        //print("=============================================================");
        if (!_isRecording)
        {
            return;
        }
        if (_entriesCount % 2 == 0)
        {
            _handList.Add("left");
        }
        else
        {
            _handList.Add("right");
        }
        if (_completedKeyType)
        {
            print("Done adding key " + _targetKey);
            //_keyList.Add(_targetKey);
            if (_targetKey == ",")
            {
                _keyList.Add("/");
            }
            else
            {
                _keyList.Add(_targetKey);
            }
            _startedKeyType = false;
            _completedKeyType = false;
            if (_entriesCount % 2 == 0)
            {
                _targetKey = _rightHandKeys[_rand.Next(_rightHandKeys.Count)];
            }
            else
            {
                _targetKey = _leftHandKeys[_rand.Next(_leftHandKeys.Count)];
            }

            List<PositionSample> _currentEndSample = Core.Ins.XRManager.GetLastPositionSamples(1);

            //Vector3 _vecLeft = _currentEndSample[0].handLPos - _currentStartSample[0].handLPos;
            //Vector3 _vecRight = _currentEndSample[0].handRPos - _currentStartSample[0].handRPos;
            //float _distanceLeft = _vecLeft.magnitude;
            //float _distanceRight = _vecRight.magnitude;

            //float _rotQLeft = Quaternion.Angle(_currentEndSample[0].handLRotQ, _currentStartSample[0].handLRotQ);
            //float _rotQRight = Quaternion.Angle(_currentEndSample[0].handRRotQ, _currentStartSample[0].handRRotQ);

            //_distanceLeftList.Add(_distanceLeft);
            //_distanceRightList.Add(_distanceRight);

            //_rotQLeftList.Add(_rotQLeft);
            //_rotQRightList.Add(_rotQRight);

            if (_entriesCount % 2 == 0)
            {
                //left
                _endReference.transform.position = _currentEndSample[0].handLPos;
                _endReference.transform.rotation = _currentEndSample[0].handLRotQ;
            }
            else
            {
                //right
                _endReference.transform.position = _currentEndSample[0].handRPos;
                _endReference.transform.rotation = _currentEndSample[0].handRRotQ;
            }

            _endReference.transform.SetParent(_startReference.transform, true);
            LocalPositionSample data = new LocalPositionSample
            {
                localPos = _endReference.transform.localPosition,
                localRot = _endReference.transform.localEulerAngles,
                localRotQ = _endReference.transform.localRotation
            };
            _localPositionSamples.Add(data);
            _endReference.transform.SetParent(null, true);

            _entriesCount++;

            _currentStartSample.Clear();
        }
        else if (_startedKeyType)
        {
            print("Adding key " + _targetKey);
            //_keyList.Add(_targetKey);
            if (_targetKey == ",")
            {
                _keyList.Add("/");
            }
            else
            {
                _keyList.Add(_targetKey);
            }

            //_distanceLeftList.Add(0.0f);
            //_distanceRightList.Add(0.0f);

            //_rotQLeftList.Add(0.0f);
            //_rotQRightList.Add(0.0f);

            List<PositionSample> _currentEndSample = Core.Ins.XRManager.GetLastPositionSamples(1);

            if (_entriesCount % 2 == 0)
            {
                //left
                _endReference.transform.position = _currentEndSample[0].handLPos;
                _endReference.transform.rotation = _currentEndSample[0].handLRotQ;
            }
            else
            {
                //right
                _endReference.transform.position = _currentEndSample[0].handRPos;
                _endReference.transform.rotation = _currentEndSample[0].handRRotQ;
            }

            _endReference.transform.SetParent(_startReference.transform, true);
            LocalPositionSample data = new LocalPositionSample
            {
                localPos = _endReference.transform.localPosition,
                localRot = _endReference.transform.localEulerAngles,
                localRotQ = _endReference.transform.localRotation
            };
            _localPositionSamples.Add(data);
            _endReference.transform.SetParent(null, true);
        }
        else
        {
            _keyList.Add("none");

            //_distanceLeftList.Add(0.0f);
            //_distanceRightList.Add(0.0f);

            //_rotQLeftList.Add(0.0f);
            //_rotQRightList.Add(0.0f);

            LocalPositionSample data = new LocalPositionSample
            {
                localPos = new Vector3(0.0f, 0.0f, 0.0f),
                localRot = new Vector3(0.0f, 0.0f, 0.0f),
                localRotQ = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)
            };
            _localPositionSamples.Add(data);
        }

        if (!_completedKeyType && _startedKeyType && _notContinueKeyType)
        {
            _notContinueKeyType = false;
            _currentStartSample = Core.Ins.XRManager.GetLastPositionSamples(1);

            _isStartGesture.Add("True");

            //if (_startReference)
            //{
            //    print("-----Get reference!-----");
            //}
            //else
            //{
            //    Dev.LogError("-----No reference!-----");
            //}

            if (_entriesCount % 2 == 0)
            {
                //left
                _startReference.transform.position = _currentStartSample[0].handLPos;
                _startReference.transform.rotation = _currentStartSample[0].handLRotQ;
            }
            else
            {
                //right
                _startReference.transform.position = _currentStartSample[0].handRPos;
                _startReference.transform.rotation = _currentStartSample[0].handRRotQ;
            }
        }
        else
        {
            _isStartGesture.Add("False");
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
            sb.Append(_localPositionSamples[i]);
            sb.Append(_keyList[i]);
            sb.Append(",");
            sb.Append(_handList[i]);
            sb.Append(",");
            //sb.Append(_distanceLeftList[i].ToString());
            //sb.Append(",");
            //sb.Append(_distanceRightList[i].ToString());
            //sb.Append(",");
            //sb.Append(_rotQLeftList[i].ToString());
            //sb.Append(",");
            //sb.Append(_rotQRightList[i].ToString());
            //sb.Append(",");
            sb.Append(_isStartGesture[i]);
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
                             + nameof(LocalPositionSample.localPos) + "x,"
                             + nameof(LocalPositionSample.localPos) + "y,"
                             + nameof(LocalPositionSample.localPos) + "z,"
                             + nameof(LocalPositionSample.localRot) + "x,"
                             + nameof(LocalPositionSample.localRot) + "y,"
                             + nameof(LocalPositionSample.localRot) + "z,"
                             + nameof(LocalPositionSample.localRotQ) + "x,"
                             + nameof(LocalPositionSample.localRotQ) + "y,"
                             + nameof(LocalPositionSample.localRotQ) + "z,"
                             + nameof(LocalPositionSample.localRotQ) + "w,"
                             + "key,hand,startGesture";
        }

        return _headerString;
    }
}
public struct LocalPositionSample
{
    public Vector3 localPos;
    public Vector3 localRot;
    public Quaternion localRotQ;

    public override string ToString()
    {
        var toReturn = localPos.x + "," + localPos.y + "," + localPos.z + ","
                       + localRot.x + "," + localRot.y + "," + localRot.z + ","
                       + localRotQ.x + "," + localRotQ.y + "," + localRotQ.z + "," + localRotQ.w + ",";
        return toReturn;
    }
}