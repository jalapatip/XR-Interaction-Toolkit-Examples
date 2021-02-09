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

    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
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
        //ShowingWeapon(true);
        
    }

    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        base.OnLastHoverExit(obj);
        //ShowingWeapon(false);
    }

    private void ShowingWeapon(bool b)
    {
        assignedRenderer.enabled = b;
    }

    private void ActivateWeapon(bool b, XRBaseInteractor obj)
    {
        assignedWeaponCollider.enabled = b;
        assignedPredeployedForm.enabled = !b;
         
        if (obj.TryGetComponent(out LineRenderer lr))
        {
            lr.enabled = !b;
        }

        this.gameObject.layer = b ? 0 : 0;
        //this.gameObject.layer = b ? 0 : 9;
    }
    

    protected override void OnSelectEnter(XRBaseInteractor obj)
    {
        base.OnSelectEnter(obj);

        //ShowingWeapon(true);
        //ActivateWeapon(true, obj);
        //Core.Ins.XRManager.HideRayController(true);
    }
    
    protected override void OnSelectExit(XRBaseInteractor obj)
    {
        base.OnSelectExit(obj);
        
        //ShowingWeapon(false);
        //ActivateWeapon(false, obj);
        
        //Core.Ins.XRManager.HideRayController(false);
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