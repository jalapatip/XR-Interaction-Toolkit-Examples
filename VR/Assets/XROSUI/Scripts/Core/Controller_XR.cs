using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class Controller_XR : MonoBehaviour
{
    [Tooltip("Set to true if this scene uses Xr")]
    public bool IsUsingXr;
    
    private GameObject _xrRig;
    private Camera _xrCamera;
    private GameObject _leftController;
    private GameObject _rightController;
    private GameObject _leftRayController;
    private GameObject _leftDirectController;
    private GameObject _leftTeleportController;
    private GameObject _rightRayController;
    private GameObject _rightDirectController;
    private GameObject _rightTeleportController;
    private GameObject _tracker;
    
    private ControllerManager_XROS controllerManager;

    #region Setup
    private void Awake()
    {
        Setup();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Setup();
    }

    private void Setup()
    {
        //Use Camera.main as default if there's none.
        
        //GetXrCamera();
        _xrCamera = Camera.main;

        if (!_xrRig)
        {
            //Dev.LogError("No XRRIG registered, attempting to substitute with XRRIG_XROS");
            _xrRig = GameObject.Find("XRRig_XROS");
            if (!_xrRig)
            {
                Dev.LogError("Cannot find XRRIG_XROS", Dev.LogCategory.XR);
            }
            else
            {
                ControllerManager_XROS controllerManager_XROS = _xrRig.GetComponent<ControllerManager_XROS>();
                if (controllerManager_XROS)
                {
                    _leftRayController = controllerManager_XROS.leftBaseController;
                    _leftDirectController = controllerManager_XROS.leftDirectController;
                    _leftTeleportController = controllerManager_XROS.leftTeleportController;
                    _rightRayController = controllerManager_XROS.rightRayController;
                    _rightDirectController = controllerManager_XROS.rightBaseController;
                    _rightTeleportController = controllerManager_XROS.rightTeleportController;
                }
                else
                {
                    Dev.LogWarning("ControllerManager does not exist", Dev.LogCategory.XR);
                }
            }
        }
    }

    #endregion Setup

    #region Update
    // Update is called once per frame
    private void Update()
    {
        //DebugUpdate();
    }

    //Track Debug Inputs here
    //https://docs.google.com/spreadsheets/d/1NMH43LMlbs5lggdhq4Pa4qQ569U1lr_O7HSHESEantU/edit#gid=0
    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            print(UnityEngine.XR.XRSettings.loadedDeviceName);
            var list = new List<InputDevice>();
            InputDevices.GetDevices(list);

            print(list.Count);
            foreach (var inputDevice in list)
            {
                print(inputDevice.name);
            }

            //print(list.ToString());
        }
    }
    #endregion Update

    #region Getters

    public Camera GetXrCamera()
    {
        if (!_xrCamera)
        {
            Dev.LogError("No XR Camera registered, attempting to substitute with main camera");
            _xrCamera = Camera.main;
        }

        return _xrCamera;
    }

    public GameObject GetXrRig()
    {
        return _xrRig;
    }

    public GameObject GetLeftRayController()
    {
        return _leftRayController;
    }

    public GameObject GetLeftDirectController()
    {
        return _leftDirectController;
    }

    public GameObject GetLeftTeleportController()
    {
        return _leftTeleportController;
    }

    public GameObject GetRightRayController()
    {
        return _rightRayController;
    }

    public GameObject GetRightDirectController()
    {
        return _rightDirectController;
    }

    public GameObject GetRightTeleportController()
    {
        return _rightTeleportController;
    }

    #endregion Getters

    #region Vibration

    public void SendHapticImpulse(ENUM_XROS_VibrationLevel level, float d)
    {
        if (controllerManager != null)
        {
            controllerManager.SendHapticImpulse(level, d);
        }
    }

    public void SendHapticBuffer(int freq)
    {
        if (controllerManager != null)
        {
            controllerManager.SendHapticBuffer(freq);
        }
    }

    #endregion Vibration

    #region Register Methods

    public void RegisterCamera(Camera newCamera)
    {
        _xrCamera = newCamera;
    }

    public void RegisterXRRig(GameObject go)
    {
        Dev.Log("XR Camera Registered as " + go.name);
        _xrRig = go;
    }

    public void RegisterControllerManager(ControllerManager_XROS cm)
    {
        controllerManager = cm;
    }

    #endregion Register Methods

    public void RegisterTracker(GameObject o)
    {
        _tracker = o;
    }

    public GameObject GetTracker()
    {
        return _tracker;
    }
}