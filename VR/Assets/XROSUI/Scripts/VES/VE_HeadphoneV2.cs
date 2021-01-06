using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class VE_HeadphoneV2 : VE_EquipmentBase
{
    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
        Core.Ins.AudioManager.PlayPauseMusic();
    }

    // protected new void Update()
    // {
    //     base.Update();
    // }

    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        base.HandleGesture(equipmentGesture, distance);
        
        switch (equipmentGesture)
        {
            case ENUM_XROS_EquipmentGesture.Up:
                _actionTooltip = "Increase Volume";
                Core.Ins.AudioManager.AdjustVolume(1, ENUM_Audio_Type.Master);
                break;
            case ENUM_XROS_EquipmentGesture.Down:
                _actionTooltip = "Decrease Volume";
                Core.Ins.AudioManager.AdjustVolume(-1, ENUM_Audio_Type.Master);
                break;
            case ENUM_XROS_EquipmentGesture.Forward:
                break;
            case ENUM_XROS_EquipmentGesture.Backward:
                break;
            case ENUM_XROS_EquipmentGesture.RotateClockwise:
                break;
            case ENUM_XROS_EquipmentGesture.RotateCounterclockwise:
                break;
            case ENUM_XROS_EquipmentGesture.Left:
                break;
            case ENUM_XROS_EquipmentGesture.Right:
                break;
            default:
                break;
        }
    }
}
