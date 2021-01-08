using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;


[RequireComponent(typeof(XRGrabInteractable))]
public class VE_AnatomyPartForPrivacy : VE_EquipmentBase
{
    public ENUM_XROS_AnatomyParts AnatomyParts;
    private bool _isTracking = true;
    private VES_MaterialPropertyBlockHelper _mbp; 

    void Start()
    {
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;

        //_mbp = this.gameObject.AddComponent<VES_MaterialPropertyBlockHelper>() as VES_MaterialPropertyBlockHelper;
    }

    protected override void OnActivate(XRBaseInteractor obj)
    {
//        Debug.Log("Activated: " + AnatomyParts.ToString());
        Core.Ins.Privacy.ToggleAnatomyPart(AnatomyParts);
    }

    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
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
                break;
        }
    }

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts anatomy, bool changedBool)
    {
        if (anatomy == AnatomyParts)
        {
            //_mbp.HandleVisualChange(changedBool);
        }
    }
}