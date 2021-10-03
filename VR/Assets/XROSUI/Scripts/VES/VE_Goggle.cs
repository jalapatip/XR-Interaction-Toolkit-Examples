using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(XRGrabInteractable))]
public class VE_Goggle : VE_EquipmentBase
{
    public float brightnessChangeRate = 0.01f;

    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
        Core.Ins.ScreenshotManager.TakeAShot();
    }

    protected override void Update()
    {
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     HandleGesture(ENUM_XROS_EquipmentGesture.Left, 0.5f);
        // }
    }
    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        base.HandleGesture(equipmentGesture, distance);
        //_actionTooltip = "None";
        _debugTooltip = ""+distance;
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
                _actionTooltip = "Decrease Brightness";
                Core.Ins.VisualManager.AdjustBrightness(-brightnessChangeRate);
                break;
            case ENUM_XROS_EquipmentGesture.Right:
                _actionTooltip = "Increase Brightness";
                Core.Ins.VisualManager.AdjustBrightness(brightnessChangeRate);
                break;
            case ENUM_XROS_EquipmentGesture.RotateBackward:
                break;
            case ENUM_XROS_EquipmentGesture.RotateForward:
                break;
            default:
                _actionTooltip = "None";
                break;
        }
        Core.Ins.VES.UpdateGestureFeedback(equipmentGesture, this);
    }
}
