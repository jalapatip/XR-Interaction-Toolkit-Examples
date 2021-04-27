using System;
using System.Collections.Generic;
using System.IO;
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
public class TypingDemo_Exp3 : DataCollection_ExpBase, IWriteToFile
{
    public XRGrabInteractable grabInteractable;
    
    // List of entire data for the experiment
    private List<string> _keyList = new List<string>();
    private List<string> _handList = new List<string>();
    private List<string> _requestedKeyList = new List<string>();
    private List<string> _enteredKeyList = new List<string>();
    private List<string> _sentenceList = new List<string>();

    private bool _startedKeyType = false;
    private bool _completedKeyType = false;
    private Random _rand = new Random();
    private string _targetKey;
    private string _targetSentence;
    private int _targetKeyIndex;
    private int _entriesCount = 0;
    private List<string> _leftHandKeys = new List<string> {"Q", "W", "E", "R", "T", "A", "S", "D", "F", "G", "Z", "X", "C", "V", "B"};
    private List<string> _rightHandKeys = new List<string> {"Y", "U", "I", "O", "P", "H", "J", "K", "L", ";", "N", "M", ",", ".", "?"};

    private static string _headerString;
    
    public NNModel leftModelSource;
    public NNModel rightModelSource;
    public TextAsset leftScalerSource;
    public TextAsset rightScalerSource;
    public TextAsset leftLabelSource;
    public TextAsset rightLabelSource;
    public TextAsset sentenceSource;
    private IWorker _leftWorker;
    private IWorker _rightWorker;
    private Dictionary<string, Scaler> _leftScalers = new Dictionary<string, Scaler>();
    private Dictionary<string, Scaler> _rightScalers = new Dictionary<string, Scaler>();
    private Dictionary<int, string>_leftLabelDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> _rightLabelDictionary = new Dictionary<int, string>();

    private void Start()
    {
        ExpName = "Exp3Demo";
    }
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        print("HERE");
        grabInteractable.onSelectEnter.AddListener(StartGesture);
        grabInteractable.onSelectExit.AddListener(EndGesture);
        Controller_XR.EVENT_NewPosition += OnNewPosition;

        StringReader reader = new StringReader(sentenceSource.text);
        string sentence = reader.ReadLine();
        while (sentence != null)
        {
            print(sentence);
            _sentenceList.Add(sentence);
            sentence = reader.ReadLine();
        }

        _targetSentence = _sentenceList[_rand.Next(_targetSentence.Count())];
        _targetKeyIndex = 0;
        _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();
        while (!_leftHandKeys.Contains(_targetKey) && !_rightHandKeys.Contains(_targetKey))
        {
            _targetKeyIndex++;
            _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();
        }
        
        var leftModel = ModelLoader.Load(leftModelSource);
        _leftWorker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, leftModel);

        Scalers leftScalers = JsonUtility.FromJson<Scalers>(leftScalerSource.text);
        foreach (Scaler scaler in leftScalers.scalers)
        {
            _leftScalers.Add(scaler.type, scaler);
        }
        
        TypingKeys typingKeys = JsonUtility.FromJson<TypingKeys>(leftLabelSource.text);
        foreach (TypingKey typingKey in typingKeys.typingKeys)
        {
            _leftLabelDictionary.Add(int.Parse(typingKey.key), typingKey.typingKey);
        }
        
        var rightModel = ModelLoader.Load(rightModelSource);
        _rightWorker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, rightModel);

        Scalers rightScalers = JsonUtility.FromJson<Scalers>(rightScalerSource.text);
        foreach (Scaler scaler in rightScalers.scalers)
        {
            _rightScalers.Add(scaler.type, scaler);
        }
        
        typingKeys = JsonUtility.FromJson<TypingKeys>(rightLabelSource.text);
        foreach (TypingKey typingKey in typingKeys.typingKeys)
        {
            _rightLabelDictionary.Add(int.Parse(typingKey.key), typingKey.typingKey);
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
        if (_leftHandKeys.Contains(_targetKey))
        {
            _handList.Add("left");
        }
        else
        {
            _handList.Add("right");
        }
        if (_completedKeyType)
        {
            _keyList.Add(_targetKey);
            _requestedKeyList.Add(_targetKey);
            _startedKeyType = false;
            _completedKeyType = false;

            if (_leftHandKeys.Contains(_targetKey))
            {
                Tensor inputTensor = CreateLeftTensor();
                _leftWorker.Execute(inputTensor);
                var output = _leftWorker.PeekOutput();
                var labelScoreArray = output.ToReadOnlyArray();
                var predictedKey = labelScoreArray.ToList().IndexOf(labelScoreArray.Max());
                _enteredKeyList.Add(_leftLabelDictionary[predictedKey]);
            }
            else
            {
                Tensor inputTensor = CreateRightTensor();
                _rightWorker.Execute(inputTensor);
                var output = _rightWorker.PeekOutput();
                var labelScoreArray = output.ToReadOnlyArray();
                var predictedKey = labelScoreArray.ToList().IndexOf(labelScoreArray.Max());
                _enteredKeyList.Add(_rightLabelDictionary[predictedKey]);
            }

            _targetKeyIndex++;
            if (_targetKeyIndex == _targetSentence.Length)
            {
                _targetSentence = _sentenceList[_rand.Next(_targetSentence.Count())];
                _targetKeyIndex = 0;
                _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();
            }
            while (!_leftHandKeys.Contains(_targetKey) && !_rightHandKeys.Contains(_targetKey))
            {
                _targetKeyIndex++;
                if (_targetKeyIndex == _targetSentence.Length)
                {
                    _targetSentence = _sentenceList[_rand.Next(_targetSentence.Count())];
                    _targetKeyIndex = 0;
                }
                _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();
            }
            
            _entriesCount++;
        }
        else if (_startedKeyType)
        {
            _keyList.Add(_targetKey);
        }
        else
        {
            _keyList.Add("none");
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
            if (_keyList[i] == ",")
            {
                sb.Append("\"");
            }
            sb.Append(_keyList[i]);
            
            if (_keyList[i] == ",")
            {
                sb.Append("\"");
            }
            sb.Append(",");
            sb.Append(_handList[i]);
        }
        return sb.ToString();
    }

    public void RemoveLastGesture()
    {
        for (int i = _keyList.Count - 1; i > -1; i--)
        {
            if (_keyList[i] != "none")
            {
                while (_keyList[i] != "none")
                {
                    _keyList[i] = "none";
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
        return "Please type the following sentence: " + _targetSentence + "\nCurrent key:" + _targetKey + "\n\nRequested Keys:\n" + requestedString + "\nDetected Keys:\n" + enteredString;
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
    
    private Tensor CreateLeftTensor()
    {
        PositionSample position = Core.Ins.XRManager.GetLastPositionSamples(1)[0];

        return new Tensor(1, 9, new float[9]{
            _leftScalers["relativeHandLPosx"].Transform(position.headPos.x - position.handLPos.x),
            _leftScalers["relativeHandLPosy"].Transform(position.headPos.y - position.handLPos.y),
            _leftScalers["relativeHandLPosz"].Transform(position.headPos.z - position.handLPos.z),
            _leftScalers["headRotx"].Transform(position.headRot.x),
            _leftScalers["headRoty"].Transform(position.headRot.y),
            _leftScalers["headRotz"].Transform(position.headRot.z),
            _leftScalers["handLRotx"].Transform(position.handLRot.x),
            _leftScalers["handLRoty"].Transform(position.handLRot.y),
            _leftScalers["handLRotz"].Transform(position.handLRot.z)
        });
    }
    
    private Tensor CreateRightTensor()
    {
        PositionSample position = Core.Ins.XRManager.GetLastPositionSamples(1)[0];

        return new Tensor(1, 9, new float[9]{
            _rightScalers["relativeHandRPosx"].Transform(position.headPos.x - position.handRPos.x),
            _rightScalers["relativeHandRPosy"].Transform(position.headPos.y - position.handRPos.y),
            _rightScalers["relativeHandRPosz"].Transform(position.headPos.z - position.handRPos.z),
            _rightScalers["headRotx"].Transform(position.headRot.x),
            _rightScalers["headRoty"].Transform(position.headRot.y),
            _rightScalers["headRotz"].Transform(position.headRot.z),
            _rightScalers["handRRotx"].Transform(position.handRRot.x),
            _rightScalers["handRRoty"].Transform(position.handRRot.y),
            _rightScalers["handRRotz"].Transform(position.handRRot.z)
        });
    }
}

[System.Serializable]
public class TypingKey
{
    public string key;
    public string typingKey;
}

[System.Serializable]
public class TypingKeys
{
    public TypingKey[] typingKeys;
}
