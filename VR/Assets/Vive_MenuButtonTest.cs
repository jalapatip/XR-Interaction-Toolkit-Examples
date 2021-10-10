using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Vive_MenuButtonTest : MonoBehaviour
{
    public XRNode inputSource;

    private InputDevice _device;

    private Vector2 inputAxis;

    // Start is called before the first frame update
    void Start()
    {
        GetDevice();
    }

    void GetDevice()
    {
        _device = InputDevices.GetDeviceAtXRNode(inputSource);
    }

    private bool bPrimaryButton = false;

    private bool buttonPushed= false;
    // Update is called once per frame
    void Update()
    {
        _device = InputDevices.GetDeviceAtXRNode(inputSource);
        //Debug.Log("name: " + _device.name);
        _device.TryGetFeatureValue(CommonUsages.primaryButton, out bPrimaryButton);

//        Debug.Log("Primary Button: " + bPrimaryButton);
        if (bPrimaryButton && !buttonPushed)
        {
            Core.Ins.Microphone.DictationStart();
            ((SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment()).StartGesture();
            Dev.Log("Start Dictate & Start Gesture " + Time.time);
            Core.Ins.AudioManager.PlaySfx("Beep_SFX");
            buttonPushed = true;
        }
        else if(!bPrimaryButton && buttonPushed)
        {
            Core.Ins.Microphone.DictationStop();
            //((SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment()).EndGesture();
            //gestureStarted = false;
            Debug.Log("Stop Dictate" + Time.time);
            buttonPushed = false;
        }
    }
}