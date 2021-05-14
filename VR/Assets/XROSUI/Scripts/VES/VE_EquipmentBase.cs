using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(XRGrabInteractable))]
public class VE_EquipmentBase : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;
    private MaterialPropertyBlock _propBlock;

    protected bool _isInSocket = false;
    protected bool _isEquipped = true;

    protected float _lastHeldTime;

    //Powen: It seems XRITK did not intend IsActivated to be a variable. We can add one ourselves but it could cause more confusion
    //It may need to be handled case by case
    //private bool _isActivated = false;
    public float timeBeforeReturn = 0.5f;
    [FormerlySerializedAs("socket")]
    public GameObject assignedSocket;
    public XROSMenuTypes menuTypes = XROSMenuTypes.Menu_General;
    protected string _actionTooltip = "";
    protected string _debugTooltip = "";

    
    
    protected void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();

        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        _grabInteractable.onActivate.AddListener(OnActivate);
        _grabInteractable.onDeactivate.AddListener(OnDeactivate);

        if (!assignedSocket)
        {
            _isEquipped = false;
        }
    }


    protected  void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        _grabInteractable.onActivate.RemoveListener(OnActivate);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivate);
    }

    //This only triggers while the object is grabbed (grip button) and the trigger button is initially pushed
    protected virtual void OnActivate(XRBaseInteractor obj)
    {
        //Dev.Log("Activated " + this.name);
        //_isActivated = true;
    }

    //This only triggers while the object is grabbed (grip button) and the trigger button is initially released
    protected virtual void OnDeactivate(XRBaseInteractor obj)
    {
        //Dev.Log("Deactivated " + this.name);
        //_isActivated = false;
    }

    protected virtual void OnSelectEnter(XRBaseInteractor obj)
    {
        _isInSocket = false;
        this.transform.SetParent(null);
    }

    protected virtual void OnSelectExit(XRBaseInteractor obj)
    {
        StopPhysics();
    }

    protected virtual void OnLastHoverExit(XRBaseInteractor obj)
    {
        // if (_mirrorEquipmentScript)
        // {
        //     _mirrorEquipmentScript.StopMirroring();    
        // }
    }

    protected virtual void OnFirstHoverEnter(XRBaseInteractor obj)
    {
    }

    protected virtual void OnHoverEnter(XRBaseInteractor obj)
    {
    }

    public virtual void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        _actionTooltip = "None";
    }

    protected void Update()
    {
        VE_Update();
    }

    protected virtual void VE_Update()
    {
        if (_grabInteractable.isSelected)
        {
            _lastHeldTime = Time.time;
        }
        else if (!_grabInteractable.isSelected && Time.time > _lastHeldTime + timeBeforeReturn)
        {
            //if (!_isInSocket)
            if (_isEquipped && !_isInSocket)
            {
//                print(this.name + " is equipped and in socket");
                var transform1 = this.transform;
                transform1.localRotation = Quaternion.identity;
                transform1.position = assignedSocket.transform.position;

                transform1.SetParent(assignedSocket.transform);

                StopPhysics();
                transform1.localRotation = Quaternion.identity;
                transform1.position = assignedSocket.transform.position;

                _isInSocket = true;
            }
        }
    }


    protected void StopPhysics()
    {
        _rigidbody.ResetCenterOfMass();
        _rigidbody.ResetInertiaTensor();
        _rigidbody.angularDrag = 0;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }

    public bool IsSelected()
    {
        return _grabInteractable.isSelected;
    }

    // public bool IsActivated()
    // {
    //     return _isActivated;
    // }

    public string GetActionTooltip()
    {
        return _actionTooltip;
    }

    public string GetDebugTooltip()
    {
        return _debugTooltip;
    }

    public double GetDistanceFromSocket()
    {
        float socketDistance = 0;
        if (_isEquipped && !_isInSocket)
        {
            socketDistance = Vector3.Distance(this.transform.position, assignedSocket.transform.position);
        }

//        print(socketDistance);
        return socketDistance;
    }


    private float GestureSphereDistance = 0.3f;
    public bool IsWithinGestureSphere()
    {
        if (_isEquipped )
        {
            if (!_isInSocket)
            {
                //not in socket, check for distance
                if (Vector3.Distance(this.transform.position, assignedSocket.transform.position) < GestureSphereDistance)
                {
                    return true;
                }    
            }
            else
            {
                //it's equipped and in socket
                return true;
            }       
        }

        //not equipped, can't be within a gesture sphere
        return false;
    }
}