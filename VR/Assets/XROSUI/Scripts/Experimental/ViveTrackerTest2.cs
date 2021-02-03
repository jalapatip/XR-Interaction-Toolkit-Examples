using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ViveTrackerTest2 : MonoBehaviour
{
    // Start is called before the first frame update
    
    public InputDevice device;

    public string nameOfDeviceToTrack = "OpenVR Controller(Vive Controller MV) - Left";
    void Awake()
    {
    }
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var d in inputDevices)
        {
            // Debug.Log(string.Format("Device found with name '{0}' and char '{1}'", d.name,
            //     d.characteristics.ToString()));

            if (d.name == nameOfDeviceToTrack)
            {
                device = d;
                Debug.Log("Tracker set as " + d.name);
            }
        }
        
        // if (tracker != null)
        // {
        //     print("FEATURE LIST of " + tracker.name);
        //     List<InputFeatureUsage> featureList = new List<InputFeatureUsage>();
        //     bool getFeatureListSuccess = tracker.TryGetFeatureUsages(featureList);
        //     print("get feature list success? " +getFeatureListSuccess + " size: " + featureList.Count);
        //     foreach (var v in featureList)
        //     {
        //         print(v.name.ToString());
        //         print(v.type.ToString());
        //         var a = v.type;
        //     }
        //     
        // }
    }


    //https://docs.unity3d.com/Manual/xr_input.html

    // Update is called once per frame
    void Update()
    {
        var getPosition = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out var position);
        
        if (getPosition)
        {
            this.transform.localPosition = position;
        }

        var getRotation = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out var rotation);
        
        if (getRotation)
        {
            this.transform.localRotation = rotation;
        }
    }
}
