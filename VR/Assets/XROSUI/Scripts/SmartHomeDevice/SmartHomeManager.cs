using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SmartHomeManager : DataCollection_ExpBase
{
    private List<SmartHomeDevice> _StationarySHDList =  new List<SmartHomeDevice>();
    private List<SmartHomeDevice> _MobileExocentricList =  new List<SmartHomeDevice>();
    private List<SmartHomeDevice> _ExocentricDeviceList =  new List<SmartHomeDevice>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            print("SHM: 8 key up");
            var list = Core.Ins.XRManager.GetLastPositionSamples(5);
            print(list[0].ToString());
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            print("SHM: 9 key up");
            foreach (var shd in _StationarySHDList)
            {
                print(shd.GetJsonString());
            }
        }
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
