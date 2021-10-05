using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(XRGrabInteractable))]
public class VE_Controller: VE_EquipmentBase
{
    public Rigidbody tank_rigidbody;
    private float speed = 1.0f;

    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
       
    }
    void Start()
    {
      
    }
    protected override void Update()
    {
       
    }
    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        base.HandleGesture(equipmentGesture, distance);
        //_actionTooltip = "None";
        _debugTooltip = "" + distance;
        switch (equipmentGesture)
        {
            case ENUM_XROS_EquipmentGesture.Up:
                tank_rigidbody.velocity = transform.forward * speed;
                break;
            case ENUM_XROS_EquipmentGesture.Down:
                tank_rigidbody.velocity = -transform.forward * speed;
                break;
            case ENUM_XROS_EquipmentGesture.Forward:
                break;
            case ENUM_XROS_EquipmentGesture.Backward:
                break;
            case ENUM_XROS_EquipmentGesture.Left:
                _actionTooltip = "Move Left";
               

                break;
            case ENUM_XROS_EquipmentGesture.Right:

                break;
            case ENUM_XROS_EquipmentGesture.UForward:
                break;
            case ENUM_XROS_EquipmentGesture.ArchBackward:
                break;
            default:
                _actionTooltip = "None";
                break;
        }
        Core.Ins.VES.UpdateGestureFeedback(equipmentGesture, this);
    }
}
