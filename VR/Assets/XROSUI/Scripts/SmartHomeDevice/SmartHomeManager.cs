using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VRKeyboard.Utils;

public delegate void Delegate_NewExperimentReady();

public class SmartHomeManager : DataCollection_ExpBase
{
    public static event Delegate_NewExperimentReady EVENT_NewExperimentReady;
    
    private List<SmartHomeDevice> _StationarySHDList = new List<SmartHomeDevice>();
    private List<SmartHomeDevice> _MobileExocentricList = new List<SmartHomeDevice>();
    private List<SmartHomeDevice> _ExocentricDeviceList = new List<SmartHomeDevice>();

    void OnEnable()
    {
        Core.Ins.DataCollection.RegisterExperiment(this);
        EVENT_NewExperimentReady?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        var a = this;
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha8, () =>
        {
            var list = Core.Ins.XRManager.GetLastPositionSamples(5);
            print(list[0].ToString());
        });
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha9, () =>
        {
            foreach (var shd in _StationarySHDList)
            {
                print(shd.GetJsonString());
            }
        });
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha7, () =>
        {
            print("Hello");
        });
        
        // Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(SmartHomeManager), KeyCode.Alpha7, () =>
        // {
        //     print("Hello2");
        // });
    }
    
    //Exocentric Equipment such as Oven, Refrigerator, Light
    public void RegisterStationaryDevice(SmartHomeDevice smartHomeDevice)
    {
//        print("Register: " + smartHomeDevice.name);
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