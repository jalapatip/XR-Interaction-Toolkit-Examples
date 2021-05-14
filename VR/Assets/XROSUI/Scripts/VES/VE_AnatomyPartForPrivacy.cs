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
    private VES_MaterialPropertyBlockHelper _mbp;

    public Color color1 = new Color(0, 62 / 255f, 192 / 255f, 255 / 255f);
    public Color color2 = new Color(192 / 255f, 0, 15 / 255f, 255 / 255f);
    private Renderer _renderer;

    protected void Start()
    {
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;
        _renderer = this.GetComponent<Renderer>();
//        _mbp = this.gameObject.AddComponent<VES_MaterialPropertyBlockHelper>() as VES_MaterialPropertyBlockHelper;
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

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts anatomy, bool changedBool, ENUM_XROS_PrivacyObserver o)
    {
        if (anatomy == AnatomyParts)
        {
            HandleVisualChange(changedBool);
        }
    }

    private void HandleVisualChange(bool changedBool)
    {
        if (!_renderer)
            return;
        
        _renderer.material.SetColor("_BaseColor", changedBool ? color1 : color2);
    }
}