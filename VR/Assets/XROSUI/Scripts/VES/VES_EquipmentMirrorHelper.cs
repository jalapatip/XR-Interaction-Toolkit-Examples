using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This script is intended for VE equipped on the user's head. That way, the user can see a ghostly copy of it in front of the user.
/// With the ghostly copy, the user can better tell what's happening.
///  
/// </summary>
public class VES_EquipmentMirrorHelper : MonoBehaviour
{
    [Tooltip("Use this to set the Prefab for the mirrored Equipment")]
    public GameObject PF_MirrorObject;
    [Tooltip("Use this to override the Prefab with a GameObject in the scene")]
    public GameObject GO_MirrorObject;
    private XROS_ENUM_InteractableConditions _mirrorConditions = XROS_ENUM_InteractableConditions.IsHovered;

    
    protected XRGrabInteractable _grabInteractable;
    private VE_EquipmentBase _equipmentBase;
    private MirrorVirtualEquipment _mirrorEquipmentScript;
    private float _distanceToHide = 0.3f; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _equipmentBase = GetComponent <VE_EquipmentBase>();
        
        //_rigidbody = GetComponent<Rigidbody>();
/*
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
*/        
        SetupMirrorEquipment();
    }
    
    
    private void SetupMirrorEquipment()
    {
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
            if (_mirrorEquipmentScript)
            {
                _mirrorEquipmentScript.SetGameObjectToMirror(this.gameObject);    
            }
            else
            {
                Debug.LogWarning("Mirror Equipment " + GO_MirrorObject.name + " is missing MirrorVirtualEquipment script");
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        MirrorEquipment_Update();
    }
    
    //public bool ShowMirrorEquipmentAtHover;
    private void MirrorEquipment_Update()
    {
        if (!_mirrorEquipmentScript)
            return;

        if (_equipmentBase.GetDistanceFromSocket() > this._distanceToHide)
        {
            _mirrorEquipmentScript.StartMirroring(false);
            return;
        }
            
        
        bool toMirror = false;
        
        switch (_mirrorConditions)
        {
            case XROS_ENUM_InteractableConditions.Always:
                toMirror = true;
                break;
            case XROS_ENUM_InteractableConditions.IsHovered:
                //_mirrorEquipmentScript.StartMirroring(_grabInteractable.isHovered);
                toMirror = _grabInteractable.isHovered;
                break;
            case XROS_ENUM_InteractableConditions.IsSelected:
                //_mirrorEquipmentScript.StartMirroring(_grabInteractable.isSelected);
                toMirror = _grabInteractable.isSelected;
                break;
            default:
                break;
        }
        _mirrorEquipmentScript.StartMirroring(toMirror);
    }
}
