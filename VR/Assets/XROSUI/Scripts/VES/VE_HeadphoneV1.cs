using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class VE_HeadphoneV1 : VE_EquipmentBase
{
    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);        
        Core.Ins.AudioManager.PlayPauseMusic();
    }

    protected override void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        
    }
    
    protected new void Update()
    {
        base.Update();

        DebugUpdate();
    }

    private void DebugUpdate()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     HandleGesture(ENUM_XROS_EquipmentGesture.Up, 0.3f);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     Core.Ins.AudioManager.AdjustVolume(1, ENUM_Audio_Type.Master);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     Core.Ins.AudioManager.AdjustVolume(-1, ENUM_Audio_Type.Master);
        // }
    }

    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        base.HandleGesture(equipmentGesture, distance);
        _debugTooltip = ""+distance;
        
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
                _actionTooltip = "None";
                break;
        }
        
        Core.Ins.VES.UpdateGestureFeedback(equipmentGesture, this);
    }
}
