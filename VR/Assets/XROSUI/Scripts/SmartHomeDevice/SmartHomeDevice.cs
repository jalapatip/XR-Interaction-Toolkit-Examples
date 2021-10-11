using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class SmartHomeDevice : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    protected SmartHomeManager _shm;
    public Animator openAnimator;

    public int UniqueId;
    
    void OnEnable()
    {
        //print(this.name + "OnEnable");
        XrInteractableOnEnable();

        if (!_shm)
        {
            _shm  = (SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment();
        }
        
        openAnimator = GetComponent<Animator>();
        
        SmartHomeManager.EVENT_NewExperimentReady += RegisterDevice;
    }

    private void OnDisable()
    {
        XrInteractableOnDisable();
        SmartHomeManager.EVENT_NewExperimentReady -= RegisterDevice;
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
    
    private void RegisterDevice()
    {
        //print(this.name + " register device ");
        if (!_shm)
        {
            _shm  = (SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment();
        }
        
        if (_shm)
        {
//            print("register success");
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
        if (!openAnimator)
        {
            Debug.Log("OpenAnimator has not been assigned in " + this._applianceType);
        }
        Debug.Log("OpenDevice " + this._applianceType + " " + b);
        
        if (b)
        {
            openAnimator.ResetTrigger("Close");
            openAnimator.SetTrigger("Open");    
        }
        else
        {
            openAnimator.ResetTrigger("Open");
            openAnimator.SetTrigger("Close");
        }
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
        string json_str = "{" +
                          "\"instance_id\":" + "\"" + this.GetInstanceID().ToString() + "\"" + "," +
                          "\"appliance_type\":" + "\"" + this.GetApplianceType().ToString() + "\"" + "," +
                          "\"position\":" + "\"" + GetPosition().ToString() + "\"" +
                          "}";

        //print(json_str);
        return json_str;
    }

    public override string ToString()
    {
        return this.GetInstanceID() + "," + this.GetApplianceType() + "," + this.GetPosition() + "," +
               this.GetRotation();
    }
    
    public static string HeaderToString()
    {
        return "instance_id, appliance_type, position, rotation";
    }
}