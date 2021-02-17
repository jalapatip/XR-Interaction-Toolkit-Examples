using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ViveTrackerTest : MonoBehaviour
{
    // Start is called before the first frame update
    
    public InputDevice tracker;

    void Awake()
    {
        Core.Ins.XRManager.RegisterTracker(this.gameObject);
    }
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            // Debug.Log(string.Format("Device found with name '{0}' and char '{1}'", device.name,
            //     device.characteristics.ToString()));
            //
            if (device.characteristics == (InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.TrackingReference))
            {
//                print("Found a lighthouse2!");
                //Device Position
                //Device Rotation
                //Device Velocity
                //Device Angular Velocity
                //TrackingState
                //IsTracked
                //GetDeviceInfo(device);
            }
            //
            // if (device.characteristics ==
            //     (InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand))
            // {
            //     print("Found a right controller!");
            // }
            // if (device.characteristics ==
            //     (InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand))
            // {
            //     print("Found a Left controller!");
            // }
            //         
            if (device.characteristics == InputDeviceCharacteristics.TrackedDevice)
            {
                print("Found a tracker!");
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

//OpenVR Controller(Vive Controller MV) - Right
    }

    private void GetDeviceInfo(InputDevice inputDevice)
    {
        if (inputDevice != null)
        {
            
            print("FEATURE LIST of " + inputDevice.name);
            var featureList = new List<InputFeatureUsage>();
            var getFeatureListSuccess = inputDevice.TryGetFeatureUsages(featureList);
            print("get feature list success? " +getFeatureListSuccess + " size: " + featureList.Count);
            foreach (var v in featureList)
            {
                print(v.name.ToString());
                print(v.type.ToString());
                var a = v.type;
            }
        }
    }


    //https://docs.unity3d.com/Manual/xr_input.html

    // Update is called once per frame
    void Update()
    {
        var getPosition = tracker.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out var position);
        if (getPosition)
        {
            this.transform.localPosition = position;
        }

        Quaternion rotation;
        bool GetRotation = tracker.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out rotation);
        
        if (GetRotation)
        {
            this.transform.localRotation = rotation;
        }
    }
}
