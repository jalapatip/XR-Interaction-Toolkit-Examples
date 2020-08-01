using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(XRGrabInteractable))]
public class VRGoggle : VREquipment
{
    public float lightIncreaseRate = 0.01f;
    public float lightDecreaseRate = -0.01f;
    string m_pathToSave;

    public override void OnActivated(XRBaseInteractor obj)
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
                Core.Ins.VisualManager.AdjustBrightness(lightDecreaseRate);
                //Debug.Log("decreaselight");
                break;
            case ENUM_XROS_Gesture.Right:
                Core.Ins.VisualManager.AdjustBrightness(lightIncreaseRate);
                //Debug.Log("increaselight");
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
