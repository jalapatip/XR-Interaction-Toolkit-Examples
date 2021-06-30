using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Serialization;

/*
Until we update to XRITK 1.0, to use the attachTransform properly in XRGrabInteractable, we'll want to offset the center by the height of the collider divided by 2.

In XRITK 0.9, it assumes the collider center is where the XRGrabInteractable should be. 
In XRITK 1.0, it assumes the GameObject Location is where the XRGrabInteractable should be. 

For some reason, the attachpoint in XRITK 0.9 does not become the new location forXRGrabInteractable and you need to manually account for it.
 */
[RequireComponent(typeof(XRGrabInteractable))]
public class VE_Weapon : VE_EquipmentBase
{
    public Renderer assignedPredeployedForm;
    [FormerlySerializedAs("_assignedRenderer")]
    public Renderer assignedRenderer;
    [FormerlySerializedAs("_assignedWeaponCollider")]
    public Collider assignedWeaponCollider;

    public ProjectileElement elementType;

    public AudioClip selectAudio;

    //Variable preventing sound activating after picking up sword
    private bool soundOn = true;

    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
    }

    void Start()
    {
        this.ShowingWeapon(false);
        assignedWeaponCollider.enabled = false;
    }
    
    new void Update()
    {
        base.Update();
        if (IsWithinGestureSphere())
        {

        }
    }

    protected override void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        base.OnFirstHoverEnter(obj);
        ShowingWeapon(true);
        
    }

    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        base.OnLastHoverExit(obj);
        ShowingWeapon(false);
    }

    private void ShowingWeapon(bool b)
    {
        assignedRenderer.enabled = b;
        //assignedWeaponCollider.enabled = b;
    }

    private void SelectWeapon(bool b, XRBaseInteractor obj)
    {
        ShowingWeapon(b);
        assignedWeaponCollider.enabled = b;
        assignedPredeployedForm.enabled = !b;
         
        if (obj && obj.TryGetComponent(out LineRenderer lr))
        {
            lr.enabled = !b;
        }

        //this.gameObject.layer = b ? 0 : 0;
        //this.gameObject.layer = b ? 0 : 9;
    }
    

    protected override void OnSelectEnter(XRBaseInteractor obj)
    {
        base.OnSelectEnter(obj);

        //Only play the audio upon first picking up the sword each time
        if (soundOn)
        {
            Core.Ins.AudioManager.PlayAudio(selectAudio, ENUM_Audio_Type.Sfx);
        }
        soundOn = false;

        SelectWeapon(true, obj);
        //Core.Ins.XRManager.HideRayController(true);
    }
    
    protected override void OnSelectExit(XRBaseInteractor obj)
    {
        base.OnSelectExit(obj);
        
        //ShowingWeapon(false);
        //ActivateWeapon(false, obj);
        
        //Core.Ins.XRManager.HideRayController(false);
    }


    protected override void VE_Update()
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

                soundOn = true;

                _isInSocket = true;
                this.SelectWeapon(false, null);
            }
        }
    }
    

    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        base.HandleGesture(equipmentGesture, distance);
        //_actionTooltip = "None";
        _debugTooltip = "" + distance;
        switch (equipmentGesture)
        {
            case ENUM_XROS_EquipmentGesture.Up:
                break;
            case ENUM_XROS_EquipmentGesture.Down:
                break;
            case ENUM_XROS_EquipmentGesture.Forward:
                break;
            case ENUM_XROS_EquipmentGesture.Backward:
                break;
            case ENUM_XROS_EquipmentGesture.Left:
                break;
            case ENUM_XROS_EquipmentGesture.Right:
                break;
            case ENUM_XROS_EquipmentGesture.RotateClockwise:
                break;
            case ENUM_XROS_EquipmentGesture.RotateCounterclockwise:
                break;
            default:
                _actionTooltip = "None";
                break;
        }

        Core.Ins.VES.UpdateGestureFeedback(equipmentGesture, this);
    }
}