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
    /*private string _currentWord = "";
    private string _correctedWord = "";
    private string _correctedSentence = "";*/
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
    private float _startTime = 0.0f;
    private float _endTime = 0.0f;
    private float _numWords = 1.0f;
    
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

    private List<PositionSample> _currentStartSample;
    private bool _notContinueKeyType = false;
    GameObject _startReference;
    GameObject _endReference;

    private List<string> _isStartGesture = new List<string>();
    private List<LocalPositionSample> _localPositionSamples = new List<LocalPositionSample>();
    private List<string> _enteredKeyListOutput = new List<string>();

    private void Start()
    {
        ExpName = "Exp3Demo";
        Core.Ins.DataCollection.RegisterExperiment(this);

        _startReference = GameObject.Find("StartReference");
        _endReference = GameObject.Find("EndReference");
    }
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        grabInteractable.onSelectEnter.AddListener(StartGesture);
        grabInteractable.onSelectExit.AddListener(EndGesture);
        Controller_XR.EVENT_NewPosition += OnNewPosition;

        StringReader reader = new StringReader(sentenceSource.text);
        string sentence = reader.ReadLine();
        while (sentence != null)
        {
            _sentenceList.Add(sentence);
            sentence = reader.ReadLine();
        }

        _targetSentence = _sentenceList[_rand.Next(_sentenceList.Count())];
        //_targetSentence = "I don't want to.";
        _targetKeyIndex = 0;
        _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();
        while (!_leftHandKeys.Contains(_targetKey) && !_rightHandKeys.Contains(_targetKey))
        {
            _enteredKeyList.Add(_targetKey);
            _targetKeyIndex++;
            _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();
        }
        
        //SpellChecker.init(sentenceSource.text);
        
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

        _notContinueKeyType = true;
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
            //_keyList.Add(_targetKey);
            if (_targetKey == ",")
            {
                _keyList.Add("/");
            }
            else
            {
                _keyList.Add(_targetKey);
            }
            _requestedKeyList.Add(_targetKey);
            //if (_targetKey == ",")
            //{
            //    _requestedKeyList.Add("/");
            //}
            //else
            //{
            //    _requestedKeyList.Add(_targetKey);
            //}
            _startedKeyType = false;
            _completedKeyType = false;

            PositionSample _currentEndSample = Core.Ins.XRManager.GetLastPositionSamples(1)[0];

            if (_leftHandKeys.Contains(_targetKey))
            {
                //left
                _endReference.transform.position = _currentEndSample.handLPos;
                _endReference.transform.rotation = _currentEndSample.handLRotQ;
            }
            else
            {
                //right
                _endReference.transform.position = _currentEndSample.handRPos;
                _endReference.transform.rotation = _currentEndSample.handRRotQ;
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

            _currentStartSample.Clear();

            if (_leftHandKeys.Contains(_targetKey))
            {
                Tensor inputTensor = CreateLeftTensor(data);
                _leftWorker.Execute(inputTensor);
                var output = _leftWorker.PeekOutput();
                var labelScoreArray = output.ToReadOnlyArray();
                var predictedKey = labelScoreArray.ToList().IndexOf(labelScoreArray.Max());
                //_enteredKeyList.Add(_leftLabelDictionary[predictedKey]);
                string predictedKeyString = _leftLabelDictionary[predictedKey];
                if (predictedKeyString == "/")
                {
                    _enteredKeyList.Add(",");
                }
                else
                {
                    _enteredKeyList.Add(predictedKeyString);
                }
                //_currentWord += _leftLabelDictionary[predictedKey];
                _enteredKeyListOutput.Add(_leftLabelDictionary[predictedKey]);
            }
            else
            {
                Tensor inputTensor = CreateRightTensor(data);
                _rightWorker.Execute(inputTensor);
                var output = _rightWorker.PeekOutput();
                var labelScoreArray = output.ToReadOnlyArray();
                var predictedKey = labelScoreArray.ToList().IndexOf(labelScoreArray.Max());
                //_enteredKeyList.Add(_rightLabelDictionary[predictedKey]);
                string predictedKeyString = _rightLabelDictionary[predictedKey];
                if (predictedKeyString == "/")
                {
                    _enteredKeyList.Add(",");
                }
                else
                {
                    _enteredKeyList.Add(predictedKeyString);
                }
                //_currentWord += _rightLabelDictionary[predictedKey];
                _enteredKeyListOutput.Add(_rightLabelDictionary[predictedKey]);
            }
            
            //_correctedWord = SpellChecker.correct(_currentWord).ToUpper();

            _targetKeyIndex++;
            if (_targetKeyIndex == _targetSentence.Length)
            {
                _targetSentence = _sentenceList[_rand.Next(_targetSentence.Count())];
                _targetKeyIndex = 0;
                _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();
            }
            _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();

            if (_targetKey == " ")
            {
                _numWords += 1.0f;
                /*_correctedSentence += (" " + _correctedWord);
                _currentWord = "";
                _correctedWord = "";*/
            }
            
            while (!_leftHandKeys.Contains(_targetKey) && !_rightHandKeys.Contains(_targetKey))
            {
                _enteredKeyList.Add(_targetKey);
                _targetKeyIndex++;
                if (_targetKeyIndex == _targetSentence.Length)
                {
                    _targetSentence = _sentenceList[_rand.Next(_targetSentence.Count())];
                    _targetKeyIndex = 0;
                }
                _targetKey = _targetSentence[_targetKeyIndex].ToString().ToUpper();
            }
            
            _entriesCount++;
            if (_startTime == 0.0)
            {
                _startTime = Time.time;
            }

            _endTime = Time.time;
        }
        else if (_startedKeyType)
        {
            //_keyList.Add(_targetKey);
            if (_targetKey == ",")
            {
                _keyList.Add("/");
            }
            else
            {
                _keyList.Add(_targetKey);
            }
            _enteredKeyListOutput.Add("empty");

            PositionSample _currentEndSample = Core.Ins.XRManager.GetLastPositionSamples(1)[0];

            if (_leftHandKeys.Contains(_targetKey))
            {
                //left
                _endReference.transform.position = _currentEndSample.handLPos;
                _endReference.transform.rotation = _currentEndSample.handLRotQ;
            }
            else
            {
                //right
                _endReference.transform.position = _currentEndSample.handRPos;
                _endReference.transform.rotation = _currentEndSample.handRRotQ;
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
            _enteredKeyListOutput.Add("empty");

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

            if (_leftHandKeys.Contains(_targetKey))
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
            sb.Append(_enteredKeyListOutput[i]);
            sb.Append(",");
            sb.Append(_handList[i]);
            sb.Append(",");
            //if (_keyList[i] == ",")
            //{
            //    sb.Append("\"");
            //}
            //sb.Append(_keyList[i]);

            //if (_keyList[i] == ",")
            //{
            //    sb.Append("\"");
            //}
            //sb.Append(",");
            sb.Append(_isStartGesture[i]);
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

        return "Please type the following sentence:\n" + _targetSentence + "\nCurrent key: " + _targetKey +
               "\n\nDetected Entry:\n" + enteredString + 
               /*"\nCurrent Word: " + _currentWord + " Corrected Word: " + _correctedWord +
               "\nCorrected Sentence: " + _correctedSentence +*/
               "\nWPM = n/a";
//               "\nWPM = " + _numWords / ((_endTime - _startTime) / 60.0);
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
                             + "key,enteredKey,hand,startGesture";
        }

        return _headerString;
    }
    
    private Tensor CreateLeftTensor(LocalPositionSample localStart)
    {
        //PositionSample position = Core.Ins.XRManager.GetLastPositionSamples(1)[0];

        //return new Tensor(1, 9, new float[9]{
        //    _leftScalers["relativeHandLPosx"].Transform(position.headPos.x - position.handLPos.x),
        //    _leftScalers["relativeHandLPosy"].Transform(position.headPos.y - position.handLPos.y),
        //    _leftScalers["relativeHandLPosz"].Transform(position.headPos.z - position.handLPos.z),
        //    _leftScalers["headRotx"].Transform(position.headRot.x),
        //    _leftScalers["headRoty"].Transform(position.headRot.y),
        //    _leftScalers["headRotz"].Transform(position.headRot.z),
        //    _leftScalers["handLRotx"].Transform(position.handLRot.x),
        //    _leftScalers["handLRoty"].Transform(position.handLRot.y),
        //    _leftScalers["handLRotz"].Transform(position.handLRot.z)
        //});

        return new Tensor(1, 7, new float[7]{
            _leftScalers["localPosx"].Transform(localStart.localPos.x),
            _leftScalers["localPosy"].Transform(localStart.localPos.y),
            _leftScalers["localPosz"].Transform(localStart.localPos.z),
            _leftScalers["localRotQx"].Transform(localStart.localRotQ.x),
            _leftScalers["localRotQy"].Transform(localStart.localRotQ.y),
            _leftScalers["localRotQz"].Transform(localStart.localRotQ.z),
            _leftScalers["localRotQw"].Transform(localStart.localRotQ.w)
        });
    }
    
    private Tensor CreateRightTensor(LocalPositionSample localStart)
    {
        //PositionSample position = Core.Ins.XRManager.GetLastPositionSamples(1)[0];

        //return new Tensor(1, 9, new float[9]{
        //    _rightScalers["relativeHandRPosx"].Transform(position.headPos.x - position.handRPos.x),
        //    _rightScalers["relativeHandRPosy"].Transform(position.headPos.y - position.handRPos.y),
        //    _rightScalers["relativeHandRPosz"].Transform(position.headPos.z - position.handRPos.z),
        //    _rightScalers["headRotx"].Transform(position.headRot.x),
        //    _rightScalers["headRoty"].Transform(position.headRot.y),
        //    _rightScalers["headRotz"].Transform(position.headRot.z),
        //    _rightScalers["handRRotx"].Transform(position.handRRot.x),
        //    _rightScalers["handRRoty"].Transform(position.handRRot.y),
        //    _rightScalers["handRRotz"].Transform(position.handRRot.z)
        //});

        return new Tensor(1, 7, new float[7]{
            _rightScalers["localPosx"].Transform(localStart.localPos.x),
            _rightScalers["localPosy"].Transform(localStart.localPos.y),
            _rightScalers["localPosz"].Transform(localStart.localPos.z),
            _rightScalers["localRotQx"].Transform(localStart.localRotQ.x),
            _rightScalers["localRotQy"].Transform(localStart.localRotQ.y),
            _rightScalers["localRotQz"].Transform(localStart.localRotQ.z),
            _rightScalers["localRotQw"].Transform(localStart.localRotQ.w)
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
