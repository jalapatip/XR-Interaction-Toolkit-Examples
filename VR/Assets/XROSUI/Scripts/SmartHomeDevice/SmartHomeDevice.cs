using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SmartHomeDevice : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    protected SmartHomeManager _shm;

    void OnEnable()
    {
        XrInteractableOnEnable();

        if (!_shm)
        {
            _shm  = (SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment();
        }
        
        SmartHomeManager.EVENT_NewExperimentReady += RegisterDevice;
    }

    private void OnDisable()
    {
        XrInteractableOnDisable();
    }
    
    #region XrInteractable
    void XrInteractableOnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
//        _rigidbody = GetComponent<Rigidbody>();

        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
//        _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        // _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        // _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        // _grabInteractable.onActivate.AddListener(OnActivate);
        // _grabInteractable.onDeactivate.AddListener(OnDeactivate);
    }

    private void XrInteractableOnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        //_grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        // _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        // _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        // _grabInteractable.onActivate.RemoveListener(OnActivate);
        // _grabInteractable.onDeactivate.RemoveListener(OnDeactivate);
    }
    
    protected virtual void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        //print(this._applianceType);
    }
    
    protected virtual void OnLastHoverExit(XRBaseInteractor obj)
    {
    }
    #endregion XrInteractable
    
    private void Start()
    {
        print("start");
        if (!_shm)
        {
            _shm  = (SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment();
        }
        
        RegisterDevice();
    }
    
    private void RegisterDevice()
    {
        if (!_shm)
        {
            _shm  = (SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment();
        }
        
        if (_shm)
        {
            _shm.RegisterStationaryDevice(this);    
        }
        else
        {
            Debug.LogError("Cannot find SmartHomeManager (DataCollection.Experiment");
        }
    }
    
    /*
    UniqueId
    ApplicationType
    Position
    Rotation
    lastEventTime
    IsOpen
    IsStarted

     */

    public ApplianceType _applianceType;
    protected float _lastInteractedTime = 0;
    protected bool _isOpened = false;
    protected bool _isStarted = false;
    public int GetUniqueId()
    {
        return this.GetInstanceID();
    }

    public ApplianceType GetApplianceType()
    {
        return _applianceType;
    }

    public void SetLastInteractedTime(float f)
    {
        _lastInteractedTime = f;
    }
    public float GetLastInteractedTime()
    {
        return _lastInteractedTime;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public Quaternion GetRotation()
    {
        return this.transform.rotation;
    }
    public Vector3 GetEulerRotation()
    {
        return this.transform.eulerAngles;
    }

    public bool IsOpened()
    {
        return _isOpened;
    }

    public bool IsStarted()
    {
        return _isStarted;
    }

    public virtual void OpenDevice(bool b)
    {
        
    }

    public virtual void StartDevice(bool b)
    {
        
    }
    
    // void OnCollisionEnter(Collision collision)
    // {
    //     print(this.name + "collide " + collision.gameObject.name);
    // }

    
    public string GetJsonString()
    {
        //TODO change this to JSON format string
        return this.GetInstanceID() + " " + this.GetApplianceType() + " " + GetPosition();
    }
}