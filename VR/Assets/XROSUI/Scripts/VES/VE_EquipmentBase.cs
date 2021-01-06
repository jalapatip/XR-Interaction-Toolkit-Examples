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

    private float _lastHeldTime;

    //Powen: It seems XRITK did not intend IsActivated to be a variable. We can add one ourselves but it could cause more confusion
    //It may need to be handled case by case
    //private bool _isActivated = false;
    public float timeBeforeReturn = 0.5f;
    public GameObject socket;
    public XROSMenuTypes menuTypes = XROSMenuTypes.Menu_General;
    protected string _actionTooltip = "";
    protected string _debugTooltip = "";

    public bool useMirrorObject = true;

    [Tooltip("Use this to set the Prefab for the mirrored Equipment")]
    public GameObject PF_MirrorObject;

    [Tooltip("Use this to override the Prefab with a GameObject in the scene")]
    public GameObject GO_MirrorObject;

    private MirrorVirtualEquipment _mirrorEquipmentScript;

    void OnEnable()
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

        if (!socket)
        {
            _isEquipped = false;
        }


        SetupMirrorEquipment();
    }

    private void SetupMirrorEquipment()
    {
        if (!useMirrorObject)
            return;
        
        //If there isn't one assigned from a scene, try to instantiate one from assigned Prefab
        if (!GO_MirrorObject)
        {
            if (PF_MirrorObject)
            {
                GO_MirrorObject = GameObject.Instantiate(PF_MirrorObject, this.transform.position, Quaternion.identity);
            }
            else
            {
                GO_MirrorObject = GameObject.Instantiate(Core.Ins.VES.PF_DefaultMirrorObject, this.transform.position,
                    Quaternion.identity);
                GO_MirrorObject.name = "MIRROR: " + this.name;
            }
        }

        if (GO_MirrorObject)
        {
            _mirrorEquipmentScript = GO_MirrorObject.GetComponent<MirrorVirtualEquipment>();
            _mirrorEquipmentScript.SetGameObjectToMirror(this.gameObject);
        }
    }

    private void OnDisable()
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
        MirrorEquipment_Update();
    }

    private void VE_Update()
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
                var transform1 = this.transform;
                transform1.localRotation = Quaternion.identity;
                transform1.position = socket.transform.position;

                transform1.SetParent(socket.transform);

                StopPhysics();
                transform1.localRotation = Quaternion.identity;
                transform1.position = socket.transform.position;

                _isInSocket = true;
            }
        }
    }

    public enum XROS_ENUM_InteractableCondition
    {
        Always,
        IsHover,
        IsGrab,
        Never
    }

    private XROS_ENUM_InteractableCondition MirrorCondition = XROS_ENUM_InteractableCondition.IsHover;

    //public bool ShowMirrorEquipmentAtHover;
    private void MirrorEquipment_Update()
    {
        if (!useMirrorObject)
            return;
        if (!_mirrorEquipmentScript) return;

        switch (MirrorCondition)
        {
            case XROS_ENUM_InteractableCondition.Always:
                _mirrorEquipmentScript.StartMirroring(true);
                break;
            case XROS_ENUM_InteractableCondition.IsHover:
                _mirrorEquipmentScript.StartMirroring(_grabInteractable.isHovered);
                break;
            case XROS_ENUM_InteractableCondition.IsGrab:
                _mirrorEquipmentScript.StartMirroring(_grabInteractable.isSelected);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void StopPhysics()
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
}