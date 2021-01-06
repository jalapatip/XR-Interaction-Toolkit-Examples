using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;
using NUnit.Framework.Constraints;
using UnityEngine.Serialization;


[RequireComponent(typeof(XRGrabInteractable))]
public class VE_VoodooSceneFloor : VE_EquipmentBase
{
//    public ENUM_XROS_AnatomyParts AnatomyParts;

    public Outline Outline;
    private Outline _myOutline;

    void Start()
    {
//        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;
        if (Outline)
        {
            _myOutline = Outline;
        }

        if (!_myOutline)
        {
            _myOutline = this.GetComponent<Outline>();    
        }
        
    }

    protected override void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        base.OnFirstHoverEnter(obj);
        //myOutline.OutlineColor = Color.white;
        if (_myOutline)
        {
            _myOutline.enabled = true;
        }
        else
        {
            Dev.LogError("Where is " + this.name + "'s outline");
        }
    }

    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        base.OnLastHoverExit(obj);
        if (_myOutline)
        {
            _myOutline.enabled = false;
        }
        else
        {
            Dev.LogError("Where is " + this.name + "'s outline");
        }
    }

    protected override void OnActivate(XRBaseInteractor obj)
    {
        Dev.Log(this.name + " OnActivate");
        Core.Ins.Privacy.DeployVoodooDoll(false);
        this.gameObject.SetActive(false);
    }
}