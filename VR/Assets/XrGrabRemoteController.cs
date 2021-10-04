using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XrGrabRemoteController : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    InputDevice m_RightController;
    InputDevice m_LeftController;

    private void OnEnable()
    {
        if (!_grabInteractable)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }

        //_grabInteractable.onActivate.AddListener(OnActivated);
        //_grabInteractable.onDeactivate.AddListener(OnDeactivated);
        _grabInteractable.onSelectEnter.AddListener(OnSelectedEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectedExit);
        InputDevices.deviceConnected += RegisterDevices;
    }

    private void OnDisable()
    {
        //_grabInteractable.onActivate.RemoveListener(OnActivated);
        //_grabInteractable.onDeactivate.RemoveListener(OnDeactivated);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectedEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectedExit);

        InputDevices.deviceConnected -= RegisterDevices;
    }

    private XRBaseInteractor _currentSelectedInteractor;

    protected void OnSelectedEnter(XRBaseInteractor obj)
    {
        _currentSelectedInteractor = obj;
        //TODO Disable ray controller's ray
        //TODO Disable teleport controller
    }

    protected void OnSelectedExit(XRBaseInteractor obj)
    {
        _currentSelectedInteractor = null;
        //TODO Enable ray controller's ray
        //TODO Enable teleport controller
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void RegisterDevices(InputDevice connectedDevice)
    {
        if (connectedDevice.isValid)
        {
#if UNITY_2019_3_OR_NEWER
            if ((connectedDevice.characteristics & InputDeviceCharacteristics.Left) != 0)
#else
            if (connectedDevice.role == InputDeviceRole.LeftHanded)
#endif
            {
                m_LeftController = connectedDevice;
                //m_LeftControllerState.ClearAll();
                //m_LeftControllerState.SetState(ControllerStates.Select);
            }
#if UNITY_2019_3_OR_NEWER
            else if ((connectedDevice.characteristics & InputDeviceCharacteristics.Right) != 0)
#else
            else if (connectedDevice.role == InputDeviceRole.RightHanded)
#endif
            {
                m_RightController = connectedDevice;
                //m_RightControllerState.ClearAll();
                //m_RightControllerState.SetState(ControllerStates.Select);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (_grabInteractable.selectingInteractor.isSelectActive)
        // {
        //     print("Is Selected");
        //
        //     _grabInteractable.selectingInteractor.
        //     if ()
        //     {
        //         _grabInteractable.selectingInteractor.IsPressed(InputHelpers.Button.PrimaryButton, out bool bMenuButtonPressed1);
        //     }
        // }

        if (_grabInteractable.isSelected)
        {
            //Core.Ins.XRManager.
            m_LeftController.IsPressed(InputHelpers.Button.Primary2DAxisClick, out bool b1IsClicked);
            m_LeftController.IsPressed(InputHelpers.Button.PrimaryAxis2DUp, out bool bLeftUp);
            m_LeftController.IsPressed(InputHelpers.Button.PrimaryAxis2DDown, out bool bLeftDown);
            m_LeftController.IsPressed(InputHelpers.Button.PrimaryAxis2DLeft, out bool bLeftLeft);
            m_LeftController.IsPressed(InputHelpers.Button.PrimaryAxis2DRight, out bool bLeftRight);
            if (b1IsClicked)
            {
                print("PrimaryAxis2DUp: " + bLeftUp);
                print("PrimaryAxis2DDown: " + bLeftDown);
                print("PrimaryAxis2DLeft: " + bLeftLeft);
                print("PrimaryAxis2DRight: " + bLeftRight);
                
            }
            
            m_RightController.IsPressed(InputHelpers.Button.Primary2DAxisClick, out bool b2IsClicked);
            m_RightController.IsPressed(InputHelpers.Button.PrimaryAxis2DUp, out bool b2LeftUp);
            m_RightController.IsPressed(InputHelpers.Button.PrimaryAxis2DDown, out bool b2LeftDown);
            m_RightController.IsPressed(InputHelpers.Button.SecondaryAxis2DLeft, out bool b2LeftLeft);
            m_RightController.IsPressed(InputHelpers.Button.PrimaryAxis2DLeft, out bool b2LeftRight);
            if (b2IsClicked)
            {
                print("SecondaryAxis2DDown: " + b2LeftUp);
                print("SecondaryAxis2DDown: " + b2LeftDown);
                print("SecondaryAxis2DLeft: " + b2LeftLeft);
                print("SecondaryAxis2DRight: " + b2LeftRight);
            }
        }
    }

    void OnSelected()
    {
        // if (m_LeftController.isValid)
        // {
        //     //m_LeftController.IsPressed(InputHelpers.Button.PrimaryButton, out bool bMenuButtonPressed1);
        //     //m_LeftController.IsPressed(InputHelpers.Button.MenuButton, out bool bMenuButtonPressed1);
        //     m_RightController.IsPressed(InputHelpers.Button.PrimaryButton, out bool bMenuButtonPressed2);
        //     m_LeftController.IsPressed(InputHelpers.Button.Trigger, out bool bTriggerButtonPressed1);
        //     m_RightController.IsPressed(InputHelpers.Button.Trigger, out bool bTriggerButtonPressed2);
        //     print("left: " + m_LeftController.manufacturer);
        //     print("left: " + m_LeftController.name);
        //     print("right: " + m_RightController.manufacturer);
        //     if (bMenuButtonPressed1)
        //     {              
        //         Debug.Log("Menu Button1 pressed");
        //         Core.Ins.SystemMenu.ToggleMenu(XROSMenuTypes.Menu_General);
        //         //gameMenu.OpenMenu("Menu_General");//press the menu button on the left controller to open general menu.
        //     }
        //     if (bMenuButtonPressed2)
        //     {
        //         print("Menu Button2 pressed");
        //     }
        // }
    }
}