using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(XRGrabInteractable))]
public class VrGoggle : VrEquipment
{
    public float brightnessChangeRate = 0.01f;

    protected override void OnActivate(XRBaseInteractor obj)
    {
        Core.Ins.ScreenshotManager.TakeAShot();
    }

    public override void HandleGesture(ENUM_XROS_Gesture gesture, float distance)
    {
        switch (gesture)
        {
            case ENUM_XROS_Gesture.Up:
                break;
            case ENUM_XROS_Gesture.Down:
                break;
            case ENUM_XROS_Gesture.Forward:
                break;
            case ENUM_XROS_Gesture.Backward:
                break;
            case ENUM_XROS_Gesture.Left:
                Core.Ins.VisualManager.AdjustBrightness(-brightnessChangeRate);
                break;
            case ENUM_XROS_Gesture.Right:
                Core.Ins.VisualManager.AdjustBrightness(brightnessChangeRate);
                break;
            case ENUM_XROS_Gesture.RotateClockwise:
                break;
            case ENUM_XROS_Gesture.RotateCounterclockwise:
                break;
            default:
                break;
        }
    }
}
