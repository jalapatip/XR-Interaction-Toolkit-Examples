using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// There may be a better way to directly get access to the button pushes on the big round button.
/// Most likely it will require writing a base class similar to XrGrabInteractable.
/// For now, this is a hacky way to achieve the same thing.
/// 
/// </summary>
public class XrGrabRemoteController : MonoBehaviour
{
//Given these values are public, changes to these values will immediately be overriden by the values in inspector.
//Modify these values in the inspector.
//movement speed forward or backward
    public float movementSpeed = 5f;

    //rotation speed of the controlledobject
    public float rotationSpeed = 90f;
    public GameObject remoteControlledObject;

    protected XRGrabInteractable _grabInteractable;

    //Use to get direct key access
    InputDevice _rightController;
    InputDevice _leftController;

    private void OnEnable()
    {
        if (!_grabInteractable)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }

        _grabInteractable.onActivate.AddListener(OnActivated);
        //_grabInteractable.onDeactivate.AddListener(OnDeactivated);
        _grabInteractable.onSelectEnter.AddListener(OnSelectedEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectedExit);

        InputDevices.deviceConnected += RegisterDevices;
    }

    private void OnDisable()
    {
        _grabInteractable.onActivate.RemoveListener(OnActivated);
        //_grabInteractable.onDeactivate.RemoveListener(OnDeactivated);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectedEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectedExit);

        InputDevices.deviceConnected -= RegisterDevices;
    }

    private XRBaseInteractor _currentSelectedInteractor;

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
                _leftController = connectedDevice;
                //m_LeftControllerState.ClearAll();
                //m_LeftControllerState.SetState(ControllerStates.Select);
            }
#if UNITY_2019_3_OR_NEWER
            else if ((connectedDevice.characteristics & InputDeviceCharacteristics.Right) != 0)
#else
            else if (connectedDevice.role == InputDeviceRole.RightHanded)
#endif
            {
                _rightController = connectedDevice;
                //m_RightControllerState.ClearAll();
                //m_RightControllerState.SetState(ControllerStates.Select);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (_grabInteractable.isSelected)
        {
            _leftController.IsPressed(InputHelpers.Button.Primary2DAxisClick, out bool leftIsClicked);
            _leftController.IsPressed(InputHelpers.Button.PrimaryAxis2DUp, out bool leftUp);
            _leftController.IsPressed(InputHelpers.Button.PrimaryAxis2DDown, out bool leftDown);
            _leftController.IsPressed(InputHelpers.Button.PrimaryAxis2DLeft, out bool leftLeft);
            _leftController.IsPressed(InputHelpers.Button.PrimaryAxis2DRight, out bool leftRight);
            if (leftIsClicked)
            {
                // print("PrimaryAxis2DUp: " + leftUp);
                // print("PrimaryAxis2DDown: " + leftDown);
                // print("PrimaryAxis2DLeft: " + leftLeft);
                // print("PrimaryAxis2DRight: " + leftRight);

                Vector3 forwardValue = Vector3.zero;

                if (leftUp)
                {
                    forwardValue = remoteControlledObject.transform.forward;
                }
                if (leftDown)
                {
                    forwardValue = -remoteControlledObject.transform.forward;
                }
                remoteControlledObject.transform.position += forwardValue * movementSpeed * Time.deltaTime;
                
                //Powen: I tried putting left and right on the same hand, but its very easy to hit up and left at the same time. 
                //In the end I put it on the other controller.
                // int rotateValue = 0;
                // if (bLeftLeft)
                // {
                //     rotateValue = 1;
                // }
                //
                // if (bLeftRight)
                // {
                //     rotateValue = -1;
                // }
                // this.transform.RotateAround(Vector3.up, rotateValue * rotationSpeed * Time.deltaTime);
            }

            _rightController.IsPressed(InputHelpers.Button.Primary2DAxisClick, out bool rightIsClicked);
            _rightController.IsPressed(InputHelpers.Button.PrimaryAxis2DUp, out bool rightUp);
            _rightController.IsPressed(InputHelpers.Button.PrimaryAxis2DDown, out bool rightDown);
            _rightController.IsPressed(InputHelpers.Button.PrimaryAxis2DLeft, out bool rightLeft);
            _rightController.IsPressed(InputHelpers.Button.PrimaryAxis2DRight, out bool rightRight);
            
            if (rightIsClicked)
            {
                int rotateValue = 0;
                // print("SecondaryAxis2DDown: " + rightUp);
                // print("SecondaryAxis2DDown: " + rightDown);
                // print("SecondaryAxis2DLeft: " + rightLeft);
                // print("SecondaryAxis2DRight: " + rightRight);
                if (rightLeft)
                {
                    rotateValue = -1;
                }

                if (rightRight)
                {
                    rotateValue = 1;
                }

                remoteControlledObject.transform.Rotate(Vector3.up, rotateValue * rotationSpeed * Time.deltaTime);
            }
        }
    }

    
    protected void OnSelectedEnter(XRBaseInteractor obj)
    {
        _currentSelectedInteractor = obj;
        //POWEN TODO Disable ray controller's ray
        //POWEN TODO Disable teleport controller
    }

    void OnSelected()
    {
    
    }
    
    protected void OnSelectedExit(XRBaseInteractor obj)
    {
        _currentSelectedInteractor = null;
        //POWEN TODO Enable ray controller's ray
        //POWEN TODO Enable teleport controller
    }

    protected void OnActivated(XRBaseInteractor obj)
    {
    }
    


}