using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VRKeyboard.Utils;

public class SmartHomeManager : DataCollection_ExpBase
{
    private List<SmartHomeDevice> _StationarySHDList = new List<SmartHomeDevice>();
    private List<SmartHomeDevice> _MobileExocentricList = new List<SmartHomeDevice>();
    private List<SmartHomeDevice> _ExocentricDeviceList = new List<SmartHomeDevice>();

    void OnEnable()
    {
        Core.Ins.DataCollection.RegisterExperiment(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        var a = this;

        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha8, () =>
        {
            //var list = Core.Ins.XRManager.GetLastPositionSamples(5);
            //print(list[0].ToString());

            print("SHM: 8 key up");

            //count = 1 how many frames you want to get
            var list = Core.Ins.XRManager.GetLastPositionSamples(1);

            //print(list[0].ToString());
            string dynamicPosition = list[0].ToString();

            string jsonInput = "{\"dynamic_position\":\"(" + dynamicPosition.Substring(1, dynamicPosition.Length - 1) + ")\"}";
            print(jsonInput);

            string jsonResponse = HTTPUtils.ServerCommunicate(jsonInput);
            print(jsonResponse);
        });

        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha9, () =>
        {
            print("SHM: 9 key up");
            print("Count test " + _StationarySHDList.Count);

            string jsonInput = "{\"device_info\":[";

            foreach (var shd in _StationarySHDList)
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

    //Exocentric Equipment such as Oven, Refrigerator, Light
    public void RegisterStationaryDevice(SmartHomeDevice smartHomeDevice)
    {
        _StationarySHDList.Add(smartHomeDevice);
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
}
