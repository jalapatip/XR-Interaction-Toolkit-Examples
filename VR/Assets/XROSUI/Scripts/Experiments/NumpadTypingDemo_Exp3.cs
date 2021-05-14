using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = System.Random;

/// <summary>
/// Exp 3 is the Text Entry
/// We track the position and rotation for headset, left hand controller, right hand controller
/// We ask the user to "type" different keys
/// </summary>
public class NumpadTypingDemo_Exp3 : DataCollection_ExpBase, IWriteToFile
{
    public XRGrabInteractable grabInteractable;
    
    // List of entire data for the experiment
    private List<int> _keyList = new List<int>();
    private List<int> _enteredKeyList = new List<int>();
    private List<int> _requestedKeyList = new List<int>();

    private bool _startedKeyType = false;
    private bool _completedKeyType = false;
    private Random _rand = new Random();
    private int _targetKey;
    private int _entriesCount = 0;

    private static string _headerString;
    
    public NNModel modelSource;
    public TextAsset scalerSource;
    private IWorker _worker;
    private Dictionary<string, Scaler> _scalers = new Dictionary<string, Scaler>();
    
    private void Start()
    {
        ExpName = "Exp3";
    }
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        _targetKey = _rand.Next(0, 10);
        grabInteractable.onSelectEnter.AddListener(StartGesture);
        grabInteractable.onSelectExit.AddListener(EndGesture);
        Controller_XR.EVENT_NewPosition += OnNewPosition;
        
        var model = ModelLoader.Load(modelSource);
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

        Scalers scalers = JsonUtility.FromJson<Scalers>(scalerSource.text);
        foreach (Scaler scaler in scalers.scalers)
        {
            _scalers.Add(scaler.type, scaler);
        }
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
            _requestedKeyList.Add(_targetKey);
            _startedKeyType = false;
            _completedKeyType = false;
            _targetKey = _rand.Next(0, 10);
            _entriesCount++;
            
            Tensor inputTensor = CreateTensor();
            _worker.Execute(inputTensor);
            var output = _worker.PeekOutput();
            var labelScoreArray = output.ToReadOnlyArray();
            var predictedKey = labelScoreArray.ToList().IndexOf(labelScoreArray.Max());
            _enteredKeyList.Add(predictedKey);
        }
        else if (_startedKeyType)
        {
            print("Adding key " + _targetKey);
            _keyList.Add(_targetKey);
        }
        else
        {
            _keyList.Add(-1);
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
        }
        return sb.ToString();
    }

    public void RemoveLastGesture()
    {
        // todo: In gestureList find the last non-None gesture and set it to None
    }

    public override int GetTotalEntries()
    {
        return _entriesCount;
    }

    public int GetTargetKey()
    {
        return _targetKey;
    }

    public override string GetGoalString()
    {
        var requestedString = "";
        foreach (var requestedKey in _requestedKeyList)
        {
            requestedString += requestedKey + " ";
        }

        var enteredString = "";
        foreach (var enteredKey in _enteredKeyList)
        {
            enteredString += enteredKey + " ";
        }
        return "Enter Key: " + _targetKey + "\nTotal Entries: " + _entriesCount + "\n\nRequested Keys:\n" + requestedString + "\nDetected Keys:\n" + enteredString;
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
                             + "key";
        }

        return _headerString;
    }
    
    private Tensor CreateTensor()
    {
        PositionSample position = Core.Ins.XRManager.GetLastPositionSamples(1)[0];

        return new Tensor(1, 15, new float[15]{
            _scalers["relativeHandRPosx"].Transform(position.headPos.x - position.handRPos.x),
            _scalers["relativeHandRPosy"].Transform(position.headPos.y - position.handRPos.y),
            _scalers["relativeHandRPosz"].Transform(position.headPos.z - position.handRPos.z),
            _scalers["relativeHandLPosx"].Transform(position.headPos.x - position.handLPos.x),
            _scalers["relativeHandLPosy"].Transform(position.headPos.y - position.handLPos.y),
            _scalers["relativeHandLPosz"].Transform(position.headPos.z - position.handLPos.z),
            _scalers["headRotx"].Transform(position.headRot.x),
            _scalers["headRoty"].Transform(position.headRot.y),
            _scalers["headRotz"].Transform(position.headRot.z),
            _scalers["handRRotx"].Transform(position.handRRot.x),
            _scalers["handRRoty"].Transform(position.handRRot.y),
            _scalers["handRRotz"].Transform(position.handRRot.z),
            _scalers["handRRotx"].Transform(position.handRRot.x),
            _scalers["handRRoty"].Transform(position.handRRot.y),
            _scalers["handRRotz"].Transform(position.handRRot.z)
        });
    }
}
