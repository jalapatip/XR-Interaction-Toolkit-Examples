using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public delegate void Delegate_NewPosition(PositionSample data);


public enum XrControllerDirection
{
    Left,
    Right,
    Neither
}

public enum XrControllerType
{
    None,
    Direct,
    Ray,
    Teleport
}

public class Controller_XR : MonoBehaviour
{
    [Tooltip("Set to true if this scene uses Xr")]
    public bool IsUsingXr;

    public static event Delegate_NewPosition EVENT_NewPosition;

    private GameObject _xrRigGO;
    private XRRig_XROS _xrRig;
    private Camera _xrCamera;
    private GameObject _leftController;
    private GameObject _rightController;
    private GameObject _leftRayController;
    private GameObject _leftDirectController;
    private GameObject _leftTeleportController;
    private GameObject _rightRayControllerGO;
    private XRRayInteractor _rightRayController;
    private GameObject _rightDirectController;
    private GameObject _rightTeleportController;
    private GameObject _tracker;
    private GameObject _spawnedObjects;

    private ControllerManager_XROS controllerManager;

    private Queue<PositionSample> _lastPositions = new Queue<PositionSample>();
    private int _lastPositionsLimit = 1000000;


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

        if (!_xrRigGO)
        {
            //Dev.LogError("No XRRIG registered, attempting to substitute with XRRIG_XROS");
            _xrRigGO = GameObject.Find("XRRig_XROS");
            if (!_xrRigGO)
            {
                Dev.LogError("Cannot find XRRIG_XROS", Dev.LogCategory.XR);
            }
            else
            {
                ControllerManager_XROS controllerManager_XROS = _xrRigGO.GetComponent<ControllerManager_XROS>();
                if (controllerManager_XROS)
                {
                    _leftRayController = controllerManager_XROS.leftBaseController;
                    _leftDirectController = controllerManager_XROS.leftDirectController;
                    _leftTeleportController = controllerManager_XROS.leftTeleportController;
                    _rightRayControllerGO = controllerManager_XROS.rightRayController;
                    _rightDirectController = controllerManager_XROS.rightBaseController;
                    _rightTeleportController = controllerManager_XROS.rightTeleportController;
                }
                else
                {
                    Dev.LogWarning("ControllerManager does not exist", Dev.LogCategory.XR);
                }

                _xrRig = _xrRigGO.GetComponent<XRRig_XROS>();
                _spawnedObjects = _xrRig.GetSpawnedObjectsGO();
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

    private void LateUpdate()
    {
        var data = new PositionSample
        {
            timestamp = Time.time,
            headPos = _xrCamera.gameObject.transform.localPosition,
            headRot = _xrCamera.gameObject.transform.eulerAngles,
            headRotQ = _xrCamera.gameObject.transform.rotation,

            handRPos = _rightDirectController.transform.localPosition,
            handRRot = _rightDirectController.transform.eulerAngles,
            handRRotQ = _rightDirectController.transform.rotation,

            handLPos = _leftDirectController.transform.localPosition,
            handLRot = _leftDirectController.transform.eulerAngles,
            handLRotQ = _leftDirectController.transform.rotation
        };
        if (_tracker)
        {
            data.tracker1Pos = _tracker.transform.localPosition;
            data.tracker1Rot = _tracker.transform.eulerAngles;
            data.tracker1RotQ = _tracker.transform.rotation;
        }

        _lastPositions.Enqueue(data);
        if (_lastPositions.Count > _lastPositionsLimit)
        {
            _lastPositions.Dequeue();
        }

        EVENT_NewPosition?.Invoke(data);
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

    /// <summary>
    /// For organizing spawned GOs from code. When a newly created GO should fall under XrRig/YOffset
    /// </summary>
    /// <param name="go"></param>
    public void PlaceInXrRigSpawnedObjects(GameObject go)
    {
        if (go)
        {
            Dev.Log(go.name + " GO container exists in XrRig");
        }
        else
        {
            Dev.Log("GO container does not exist");
        }

        if (_spawnedObjects)
        {
            Dev.Log(_spawnedObjects.name + " GO exists");
        }
        else
        {
            Dev.Log("_spawnedObjects does not exist");
        }

        //SpawnedObject does not exist
        go.transform.SetParent(this._spawnedObjects.transform);
    }

    public GameObject GetXrRigGO()
    {
        return _xrRigGO;
    }

    public XRRig_XROS GetXrRig()
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

    public GameObject GetRightRayControllerGO()
    {
        return _rightRayControllerGO;
    }

    public XRRayInteractor GetRightRayController()
    {
        if (!_rightController)
        {
            _rightRayController = _rightRayControllerGO.GetComponent<XRRayInteractor>();
        }

        return _rightRayController;
    }

    public GameObject GetRightDirectControllerGO()
    {
        return _rightDirectController;
    }

    public GameObject GetRightTeleportController()
    {
        return _rightTeleportController;
    }

    public List<PositionSample> GetLastPositionSamples(int count)
    {
        return new List<PositionSample>(_lastPositions).GetRange(_lastPositions.Count - count, count);
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
        _xrRigGO = go;
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

    public XrControllerDirection CheckLeftOrRightController(XRBaseInteractor arg0)
    {
        var xrcontroller = arg0.GetComponent<XRController>();
        if (xrcontroller)
        {
            if (xrcontroller.controllerNode == XRNode.LeftHand)
            {
                return XrControllerDirection.Left;
            }

            if (xrcontroller.controllerNode == XRNode.RightHand)
            {
                return XrControllerDirection.Right;
            }
        }

        return XrControllerDirection.Neither;
    }

    public XrControllerType CheckInteractorType(XRBaseInteractor arg0)
    {
        print("Check Interactor Type: arg0");

        var id = arg0.gameObject.GetInstanceID();

        if (id == this._leftDirectController.GetInstanceID() || id == this._rightDirectController.GetInstanceID())
        {
            return XrControllerType.Direct;
        }

        if (id == this._leftRayController.GetInstanceID() || id == this._rightRayControllerGO.GetInstanceID())
        {
            return XrControllerType.Ray;
        }

        if (id == this._leftTeleportController.GetInstanceID() || id == this._rightTeleportController.GetInstanceID())
        {
            return XrControllerType.Teleport;
        }

        var controllerType = arg0.GetComponent<XRDirectInteractor>();
        if (controllerType)
        {
            return XrControllerType.Direct;
        }

        var controllerType2 = arg0.GetComponent<XRRayInteractor>();
        if (controllerType2)
        {
            if (controllerType2.lineType == XRRayInteractor.LineType.ProjectileCurve)
            {
                return XrControllerType.Teleport;
            }

            return XrControllerType.Ray;
        }

        return XrControllerType.None;
    }

    private AudioListener _XrCameraListener;

    public AudioListener GetXrCameraListener()
    {
        if (!_XrCameraListener)
        {
            _XrCameraListener = this.GetXrCamera().GetComponent<AudioListener>();
        }

        return _XrCameraListener;
    }
}

public struct PositionSample
{
    public float timestamp;
    public Vector3 headPos;
    public Vector3 headRot; //Euler Angles
    public Quaternion headRotQ; //Quaternion
    public Vector3 handRPos;
    public Vector3 handRRot;
    public Quaternion handRRotQ;
    public Vector3 handLPos;
    public Vector3 handLRot;
    public Quaternion handLRotQ;
    public Vector3 tracker1Pos;
    public Vector3 tracker1Rot;
    public Quaternion tracker1RotQ;

    public override string ToString()
    {
        var toReturn = "\n" + timestamp + ","
                       + headPos.x + "," + headPos.y + "," + headPos.z + ","
                       + headRot.x + "," + headRot.y + "," + headRot.z + ","
                       + headRotQ.x + "," + headRotQ.y + "," + headRotQ.z + "," + headRotQ.w + ","
                       + handRPos.x + "," + handRPos.y + "," + handRPos.z + ","
                       + handRRot.x + "," + handRRot.y + "," + handRRot.z + ","
                       + handRRotQ.x + "," + handRRotQ.y + "," + handRRotQ.z + "," + handRRotQ.w + ","
                       + handLPos.x + "," + handLPos.y + "," + handLPos.z + ","
                       + handLRot.x + "," + handLRot.y + "," + handLRot.z + ","
                       + handLRotQ.x + "," + handLRotQ.y + "," + handLRotQ.z + "," + handLRotQ.w + ",";
        return toReturn;
    }
}