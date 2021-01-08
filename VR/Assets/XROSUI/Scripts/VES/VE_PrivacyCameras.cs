using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Serialization;


[RequireComponent(typeof(XRGrabInteractable))]
public class VE_PrivacyCameras : VE_EquipmentBase
{
    [FormerlySerializedAs("observer")]
    public ENUM_XROS_PrivacyObserver observerType;

    // void OnEnable()
    // {
    //     Manager_Privacy.EVENT_NewPrivacyObserver += HandleObserverChange;
    // }
    // void OnDisable()
    // {
    //     Manager_Privacy.EVENT_NewPrivacyObserver -= HandleObserverChange;
    // }
    void Start()
    {
        Manager_Privacy.EVENT_NewPrivacyObserver += HandleObserverChange;
        //Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;
    }

    protected override void OnActivate(XRBaseInteractor obj)
    {
//        Debug.Log("Activated: " + AnatomyParts.ToString());
        //Core.Ins.Privacy.ToggleAnatomyPart(AnatomyParts);
        Core.Ins.Privacy.SwitchPrivacyObserver(observerType);
    }

    // protected override void OnFirstHoverEnter(XRBaseInteractor obj)
    // {
    //     base.OnFirstHoverEnter(obj);
    //     
    // }
    //
    // protected override void OnLastHoverExit(XRBaseInteractor obj)
    // {
    //     base.OnLastHoverExit(obj);
    // }

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

    private void HandleObserverChange(ENUM_XROS_PrivacyObserver o, bool changedBool)
    {
//        print(this.gameObject + " handle observer change");
        if (o == observerType)
        {
            HandleVisualChange(changedBool);
        }
    }
    // private void HandleAnatomyChange(ENUM_XROS_AnatomyParts anatomy, bool changedBool)
    // {
    //         HandleVisualChange(changedBool);
    // }

    private void HandleVisualChange(bool ChangedBool)
    {
        if (ChangedBool)
        {
            this.GetComponentInChildren<Renderer>().material.color = Color.red;
        }
        else
        {
            this.GetComponentInChildren<Renderer>().material.color = Color.blue;
        }

    }
}