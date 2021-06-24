using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//https://docs.unity3d.com/Manual/xr_input.html
/// <summary>
/// This script tries to fetch the position and rotation information of the Vive tracker.
/// It also registers itself to the XRManager.
///
/// This is the script that should be used instead of ViveTrackerTest2.cs
/// </summary>
public class ViveTrackerTest : MonoBehaviour
{
    public InputDevice tracker;
    
    public bool UseTrackerPosition = true;
    public bool UseTrackerRotation = true;
    
    void Awake()
    {
        Core.Ins.XRManager.RegisterTracker(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            // Debug.Log(string.Format("Device found with name '{0}' and char '{1}'", device.name,
            //     device.characteristics.ToString()));
            if (device.characteristics ==
                (InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.TrackingReference))
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
            print("get feature list success? " + getFeatureListSuccess + " size: " + featureList.Count);
            foreach (var v in featureList)
            {
                print(v.name.ToString());
                print(v.type.ToString());
                var a = v.type;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (UseTrackerPosition)
        {
            var hasPositionValue = tracker.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out var position);
            if (hasPositionValue)
            {
                this.transform.localPosition = position;
            }
        }

        if (UseTrackerRotation)
        {
            Quaternion rotation;
            bool hasRotationValue = tracker.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out rotation);

            if (hasRotationValue)
            {
                this.transform.localRotation = rotation;
            }
        }
    }
}