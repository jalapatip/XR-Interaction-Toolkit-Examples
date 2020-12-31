using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;


[RequireComponent(typeof(XRGrabInteractable))]
public class VE_AnatomyPartForPrivacy: VrEquipment
{
    public ENUM_XROS_AnatomyParts AnatomyParts;

    private Outline myOutline;
    private bool isTracking = true;
    
    void Start()
    {
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;
        myOutline = this.GetComponent<Outline>();
    }

    protected override void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        base.OnFirstHoverEnter(obj);
        //myOutline.OutlineColor = Color.white;
        myOutline.enabled = true;
    }

    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        base.OnLastHoverExit(obj);
        myOutline.enabled = false;
    }
    
    protected override void OnActivate(XRBaseInteractor obj)
    {
        Core.Ins.Privacy.ToggleAnatomyPart(AnatomyParts);
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
                break;
            case ENUM_XROS_Gesture.Right:
                break;
            case ENUM_XROS_Gesture.RotateClockwise:
                break;
            case ENUM_XROS_Gesture.RotateCounterclockwise:
                break;
            default:
                break;
        }
    }

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts anatomy, bool changedBool)
    {
        if (anatomy == AnatomyParts)
        {
            HandleVisualChange(changedBool);            
        }
    }

    private void HandleVisualChange(bool ChangedBool)
    {
        
    }
}
