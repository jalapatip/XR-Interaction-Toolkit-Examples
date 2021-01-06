using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VE_Avatar : VE_EquipmentBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }

    protected override void OnHoverEnter(XRBaseInteractor obj)
    {
        base.OnHoverEnter(obj);
        Core.Ins.Avatar.ShowAvatarManagementMode(true);
    }

    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        base.OnLastHoverExit(obj);
        Core.Ins.Avatar.ShowAvatarManagementMode(false);
    }
    
    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
        //Toggle On/Off Avatar Management Mode
        Core.Ins.Avatar.ToggleAvatarManagementModeLock();
    }

    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        base.HandleGesture(equipmentGesture, distance);
        
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
                Core.Ins.Avatar.PreviousAlternateAvatar();
                break;
            case ENUM_XROS_EquipmentGesture.Right:
                Core.Ins.Avatar.NextAlternateAvatar();
                break;
            case ENUM_XROS_EquipmentGesture.RotateClockwise:
                break;
            case ENUM_XROS_EquipmentGesture.RotateCounterclockwise:
                break;
            default:
                break;
        }
    }
}
