using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This script is intended for VE equipped on the user's head. That way, the user can see a ghostly copy of it in front of the user.
/// With the ghostly copy, the user can better see what's happening.
///  
/// </summary>
public class VES_EquipmentMirrorHelper : MonoBehaviour
{
    [Tooltip("Use this to set the Prefab for the mirrored Equipment. This prefab should have the same model and MirrorVirtualEquipment.cs script with the name prefix VESMirror_")]
    public GameObject PF_MirrorObject;
    [Tooltip("Leave this empty to spawn a model automatically. Otherwise use this to override the Prefab with a GameObject in the scene")]
    public GameObject GO_MirrorObject;
    private XROS_ENUM_InteractableConditions _mirrorConditions = XROS_ENUM_InteractableConditions.IsHovered;


    protected XRGrabInteractable _grabInteractable;
    private VE_EquipmentBase _equipmentBase;
    private MirrorVirtualEquipment _mirrorEquipmentScript;
    private float _distanceToHide = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        SetupMirrorEquipment();
    }

    protected void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _equipmentBase = GetComponent<VE_EquipmentBase>();

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
    }


    private void SetupMirrorEquipment()
    {
        //If there isn't one assigned from a scene, try to instantiate one from assigned Prefab. If no assigned Prefab, instantiate from default.
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
  
        //Core.Ins.XRManager.PlaceInXrRigYOffset(GO_MirrorObject);
        Core.Ins.VES.PlaceMirrorObject(GO_MirrorObject);
        //GO_MirrorObject.transform.SetParent();
//        Dev.Log(this.gameObject.name + " is instantiating " + GO_MirrorObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        MirrorEquipment_Update();
    }

    private void MirrorEquipment_Update()
    {
        //Check to see if the other GO has the appropriate script
        if (!_mirrorEquipmentScript)
            return;

        //Check to see if this equipment is far enough from the socket so that we should be showing something
        if (_equipmentBase.GetDistanceFromSocket() > this._distanceToHide)
        {
            _mirrorEquipmentScript.StartMirroring(false);
            return;
        }

        //Handle mirroring based on the interaction condition
        var toMirror = false;
        switch (_mirrorConditions)
        {
            case XROS_ENUM_InteractableConditions.Always:
                toMirror = true;
                break;
            case XROS_ENUM_InteractableConditions.IsHovered:
                toMirror = _grabInteractable.isHovered;
                break;
            case XROS_ENUM_InteractableConditions.IsSelected:
                toMirror = _grabInteractable.isSelected;
                break;
            case XROS_ENUM_InteractableConditions.Never:
                toMirror = false;
                break;
            default:
                break;
        }
        _mirrorEquipmentScript.StartMirroring(toMirror);
    }
}
