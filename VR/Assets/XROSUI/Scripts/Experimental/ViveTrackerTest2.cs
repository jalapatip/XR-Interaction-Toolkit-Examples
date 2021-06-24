using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//https://docs.unity3d.com/Manual/xr_input.html
/// <summary>
/// This is a brute force way of setting different gameObjects to the tracked Vive Devices.
/// To use it, you'd find out the name of the tracked device and assign it int he inspector.
/// </summary>
public class ViveTrackerTest2 : MonoBehaviour
{
    public InputDevice device;

    public bool UseTrackerPosition = true;
    public bool UseTrackerRotation = true;
    public string nameOfDeviceToTrack = "OpenVR Controller(Vive Controller MV) - Left";
    void Awake()
    {
        //Core.Ins.XRManager.RegisterTracker(this.gameObject);
    }
    
    // Start is called before the first frame update
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



    // Update is called once per frame
    void Update()
    {
        if (UseTrackerPosition)
        {
            var hasPositionValue = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out var position);
            if (hasPositionValue)
            {
                this.transform.localPosition = position;
            }
        }

        if (UseTrackerRotation)
        {
            Quaternion rotation;
            bool hasRotationValue = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out rotation);

            if (hasRotationValue)
            {
                this.transform.localRotation = rotation;
            }
        }
    }
}
