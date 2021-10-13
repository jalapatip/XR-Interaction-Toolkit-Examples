using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XrButtonUtilizer : MonoBehaviour
{
    public XRNode inputSource;
    private InputDevice _device;


    protected InputFeatureUsage<bool> TargetXrButton;

    private bool _buttonPushed = false;
    private bool _buttonStayPushed = false;

    void GetXrDevice()
    {
        _device = InputDevices.GetDeviceAtXRNode(inputSource);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //_device = InputDevices.GetDeviceAtXRNode(inputSource);

        //Debug.Log("base update");
        GetXrDevice();
        //Debug.Log("name: " + _device.name);
        _device.TryGetFeatureValue(TargetXrButton, out _buttonPushed);

        //Debug.Log(XrButtonBeingTargetted.ToString());
        //Debug.Log("Button pushed: " + buttonPushed);
        //Debug.Log("base Input Source " +inputSource);


        if (_buttonPushed && !_buttonStayPushed)
        {
//            Debug.Log("base onpushed");
            OnPushed();
            _buttonStayPushed = true;
        }
        else if (_buttonPushed && _buttonStayPushed)
        {
//            Debug.Log("base onpushing");
            OnPushing();
        }
        else if (!_buttonPushed && _buttonStayPushed)
        {
//            Debug.Log("base onreleased");
            OnReleased();
            _buttonStayPushed = false;
        }
    }

    protected virtual void OnPushed()
    {
    }

    protected virtual void OnPushing()
    {
    }

    protected virtual void OnReleased()
    {
    }
}