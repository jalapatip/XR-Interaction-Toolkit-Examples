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
    public ENUM_XROS_PrivacyObserver observerType;
    public Renderer myRenderer;
    protected new void OnEnable()
    {
        base.OnEnable();
        Manager_Privacy.EVENT_NewPrivacyObserver += HandleObserverChange;
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        Manager_Privacy.EVENT_NewPrivacyObserver -= HandleObserverChange;
    }
    // void Start()
    // {
    //     Manager_Privacy.EVENT_NewPrivacyObserver += HandleObserverChange;
    //     //Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;
    // }

    private bool _active = true;
    protected override void OnActivate(XRBaseInteractor obj)
    {
        _active = !_active;
//        Debug.Log("Activated: " + AnatomyParts.ToString());
        //Core.Ins.Privacy.ToggleAnatomyPart(AnatomyParts);
        Core.Ins.Privacy.ActivatePrivacyObserver(observerType, _active);
    }

    protected override void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        base.OnFirstHoverEnter(obj);
        Core.Ins.Privacy.SwitchPrivacyObserver(observerType);
    }
    
    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        base.OnLastHoverExit(obj);
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
            case ENUM_XROS_EquipmentGesture.RotateForward:
                break;
            case ENUM_XROS_EquipmentGesture.RotateBackward:
                break;
            default:
                break;
        }
    }

    public Color color1 = new Color(0, 62 / 255f, 192 / 255f, 255 / 255f);
    public Color color2 = new Color(192 / 255f, 0, 15 / 255f, 255 / 255f);
    public Color color3 = new Color(0 / 255f, 192/255f, 0 / 255f, 255 / 255f);
    
    private void HandleObserverChange(ENUM_XROS_PrivacyObserver o, bool changedBool)
    {

        HandleVisualChange(o==observerType);
        // if (o == observerType)
        // {
        //     HandleVisualChange(changedBool);
        // }
    }
    // private void HandleAnatomyChange(ENUM_XROS_AnatomyParts anatomy, bool changedBool)
    // {
    //         HandleVisualChange(changedBool);
    // }

    private void HandleVisualChange(bool HasChanged)
    {
        if (HasChanged)
        {
            myRenderer.material.SetColor("_BaseColor", color3); 
        }
        else
        {
            myRenderer.material.SetColor("_BaseColor", color1);
            //this.GetComponentInChildren<Renderer>().material.color = Color.blue;
        }
    }
    
    private void HandleObserverActivateChange(ENUM_XROS_PrivacyObserver o, bool IsActivated)
    {
        if (o != observerType)
            return;
        
        if (IsActivated)
        {
            HandleVisualChange(Core.Ins.Privacy.GetCurrentPrivacyObserver() == this.observerType);
            // if (Core.Ins.Privacy.GetCurrentPrivacyObserver() == this.observerType)
            // {
            //     myRenderer.material.SetColor("_BaseColor", color3);    
            // }
            // else
            // {
            //     
            // }
            //  
        }
        else
        {
            myRenderer.material.SetColor("_BaseColor", color2);
            //this.GetComponentInChildren<Renderer>().material.color = Color.blue;
        }
    }
}