using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using VRKeyboard.Utils;

public delegate void Delegate_NewExperimentReady();

public class DeviceCollection : IWriteToFile
{
    public List<SmartHomeDevice> DeviceList = new List<SmartHomeDevice>();

    public string OutputFileName()
    {
        return "StationarySHDList.csv";
    }

    public string OutputData()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(SmartHomeDevice.HeaderToString());
        foreach (var a in DeviceList)
        {
            sb.Append(a.ToString());
        }

        return sb.ToString();
    }
}

public class SmarthomeTarget
{
    private SmartHomeDevice _shd;

    private string _instructions;

    public SmarthomeTarget(string s, SmartHomeDevice shd)
    {
        _instructions = s;
        _shd = shd;
    }

    public string ToInstructionStrings()
    {
        return _instructions;
    }

    public string TargetString()
    {
        return _shd._applianceType.ToString();
    }

    public int TargetId()
    {
        return _shd.gameObject.GetInstanceID();
    }
}

public class SmartHomeManager : DataCollection_ExpBase
{
    public static event Delegate_NewExperimentReady EVENT_NewExperimentReady;

    private DeviceCollection _stationaryShdList = new DeviceCollection();

    //private List<SmartHomeDevice> _StationarySHDList = new List<SmartHomeDevice>();
    private List<SmartHomeDevice> _MobileExocentricList = new List<SmartHomeDevice>();
    private List<SmartHomeDevice> _ExocentricDeviceList = new List<SmartHomeDevice>();

    //tracks if we are in the middle of performing a 'gesture'. something meaningful we are tracking with this experiment.
    private bool _completedGesture = false;

    //how many gestures were done
    private int _gestureCount = 0;

    public List<SmarthomeTarget> targets = new List<SmarthomeTarget>();

    //Does not show up
    public Dictionary<string, GameObject> targetDictionary = new Dictionary<string, GameObject>();

    #region Setup

    public List<string> objectiveList = new List<string>();
    private bool _startedGesture = false;

    void OnEnable()
    {
        ExpName = "SmartHome Exp";
        Core.Ins.DataCollection.RegisterExperiment(this);
        Controller_XR.EVENT_NewPosition += OnNewPosition;
        Manager_Microphone.Event_NewDictationResult += OnNewDictationResult;
    }

    private void OnDisable()
    {
        Controller_XR.EVENT_NewPosition -= OnNewPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        EVENT_NewExperimentReady?.Invoke();
        //var a = this;
        //var t = new SmarthomeTarget("Say 'Open' while pointing at the target", "Microwave");
        //targets.Add(t);
        //t = new SmarthomeTarget("Say 'Open' while pointing at the target", "Oven");
        //targets.Add(t);

        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha8, () =>
        {
            //var list = Core.Ins.XRManager.GetLastPositionSamples(5);
            //print(list[0].ToString());

            print("SHM: 8 key up");

            //count = 1 how many frames you want to get
            var list = Core.Ins.XRManager.GetLastPositionSamples(1);

            //print(list[0].ToString());
            string dynamicPosition = list[0].ToString();

            string jsonInput = "{\"dynamic_position\":\"(" + dynamicPosition.Substring(1, dynamicPosition.Length - 1) +
                               ")\"}";
            print(jsonInput);

            string jsonResponse = HTTPUtils.ServerCommunicate(jsonInput);
            print(jsonResponse);
        });

        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha9, () =>
        {
            print("SHM: 9 key up");
            print("Count test " + this._stationaryShdList.DeviceList.Count);

            string jsonInput = "{\"device_info\":[";

            foreach (var shd in this._stationaryShdList.DeviceList)
            {
                print(shd.GetJsonString());

                jsonInput += shd.GetJsonString();
                jsonInput += ",";
            }

            if (jsonInput.Substring(jsonInput.Length - 1).Equals(","))
            {
                jsonInput = jsonInput.Substring(0, jsonInput.Length - 1);
            }

            jsonInput += "]}";

            print(jsonInput);

            //TODO paring the output
            string jsonResponse = HTTPUtils.ServerCommunicate(jsonInput);
            print(jsonResponse);
        });

        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha7, () =>
        {
            print("Hello");

            print("SHM: 7 key up");

            // send utterance
            // TODO: [Inference] put all input info together => send to server (when and how to trigger)
            // TODO: [Collecting] how to collect more data easily
            string jsonInput = "{\"utterance\":\"open the door\"}";

            string jsonResponse = HTTPUtils.ServerCommunicate(jsonInput);
            RASAResult info = JsonUtility.FromJson<ServerResult>(jsonResponse).result;

            print(info.text);
        });

        //Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha7, () =>
        //{
        //    print("Hello2");
        //});
    }

    #endregion Setup

    //Exocentric Equipment such as Oven, Refrigerator, Light
    public void RegisterStationaryDevice(SmartHomeDevice smartHomeDevice)
    {
//        print("Register: " + smartHomeDevice.name);
        this._stationaryShdList.DeviceList.Add(smartHomeDevice);
    }

    //Exocentric Equipment that moves around such as a cleaning robot (Roomba)
    public void RegisterMobileDevice(SmartHomeDevice smartHomeDevice)
    {
        _MobileExocentricList.Add(smartHomeDevice);
    }

    //Egocentric Equipment that moves along with the user, such as Smart glasses
    public void RegisterEgocentricDevice(SmartHomeDevice smartHomeDevice)
    {
        _ExocentricDeviceList.Add(smartHomeDevice);
    }

    public void StartGesture()
    {
        _startedGesture = true;
    }

    public void EndGesture()
    {
        _completedGesture = true;
        _startedGesture = false;
    }

    public void OnNewDictationResult()
    {
        //Play a sound effect so we know there's a result for dictation
        Core.Ins.AudioManager.PlaySfx("Beep_SFX");

        //Make a JSON call to the server
        //            string jsonInput = "{\"utterance\":\"" + Core.Ins.Microphone.GetCurrentUtterance() + "\"}";
//            print(jsonInput);

//            string jsonResponse = HTTPUtils.ServerCommunicate(jsonInput);
//            RASAResult info = JsonUtility.FromJson<ServerResult>(jsonResponse).result;
//          // TODO: return the entity => do comparison
//            print(info.text);

        // foreach (var shd in this._stationaryShdList.DeviceList)
        // {
        //     if (shd.GetApplianceType().ToString().Equals("SHD_Oven"))
        //     {
        //         shd.OpenDevice(true);
        //     }
        // }
        
        _completedGesture = true;
    }

    public void OnNewPosition(PositionSample sample)
    {
        var data = new DataContainer_ExpSmarthome()
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

        data.targetType = targets[_gestureCount].TargetString();
        data.targetId = targets[_gestureCount].TargetId();
        data.utterance = "";

        if (_startedGesture)
        {
            data.gestureStatus = "started";
        }
        
        if (_completedGesture)
        {
            data.gestureStatus = "completed";
            data.utterance = Core.Ins.Microphone.GetCurrentUtterance();
            _startedGesture = false;
            _completedGesture = false;
            _gestureCount++;
            NextTarget();
        }
        else
        {
            //_gestureList.Add(ENUM_XROS_EquipmentGesture.None);

        }


        dataList.Add(data);
    }

    public void SaveDeviceList()
    {
        Core.Ins.DataCollection.SaveGeneralData(this._stationaryShdList);
    }

    public override int GetTotalEntries()
    {
        return _gestureCount;
    }

    public override string OutputHeaderString()
    {
        return DataContainer_ExpSmarthome.HeaderToString();
    }

    public void NextTarget()
    {
    }

    public override string GetGoalString()
    {
        return "Current Target: " + this.targets[_gestureCount].ToInstructionStrings() + "\n" + this.GetTotalEntries();
    }


    public override string OutputFileName()
    {
        return ExpName + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".csv";
    }

    public override void RemoveLastEntry()
    {
        // string noneString = ENUM_XROS_EquipmentGesture.None.ToString();
        // for (int i = dataList.Count - 1; i >= 0; i--)
        // {
        //     DataContainer_Exp1GesturesPosition data = (DataContainer_Exp1GesturesPosition) dataList[i];
        //
        //     if (!data.gesture.Equals(noneString))
        //     {
        //         data.gesture = noneString;
        //         _gestureCount--;
        //         return;
        //     }
        // }
    }

    public void AddTarget(string instructions, SmartHomeDevice shd)
    {
        targets.Add(new SmarthomeTarget(instructions, shd));
    }
}