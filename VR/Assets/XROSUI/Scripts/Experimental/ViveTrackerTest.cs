using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ViveTrackerTest : MonoBehaviour
{
    // Start is called before the first frame update
    
    public InputDevice tracker;
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and char '{1}'", device.name,
                device.characteristics.ToString()));

            if (device.characteristics == InputDeviceCharacteristics.TrackedDevice)
            {
                tracker = device;
            }
        }

        foreach (var device in inputDevices)
        {
            if (device.characteristics == InputDeviceCharacteristics.TrackedDevice)
            {
                tracker = device;
            }
        }

        
        print("FEATURE LIST of " + tracker.name);
        List<InputFeatureUsage> featureList = new List<InputFeatureUsage>();
        bool getFeatureListSuccess = tracker.TryGetFeatureUsages(featureList);
        print("get feature list success? " +getFeatureListSuccess + " size: " + featureList.Count);
        foreach (var v in featureList)
        {
            print(v.name.ToString());
            print(v.type.ToString());
            var a = v.type;
        }
    }


    //https://docs.unity3d.com/Manual/xr_input.html

    // Update is called once per frame
    void Update()
    {
        Vector3 position; 
        bool GetPosition = tracker.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out position);
        if (GetPosition)
        {
            this.transform.position = position;
        }

        Quaternion rotation;
        bool GetRotation = tracker.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out rotation);
        
        if (GetRotation)
        {
            this.transform.rotation = rotation;
        }
    }
}
