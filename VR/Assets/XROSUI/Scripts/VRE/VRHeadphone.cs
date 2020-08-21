using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class VrHeadphone : VrEquipment
{
    public GameObject GestureCore;
    public GameObject PF_MirrorObject;
    public GameObject GO_MirrorObject;
    private MirrorGameObject mirrorGO;
    
    public void Start()
    {
        //GO_MirrorObject = GameObject.Instantiate(PF_MirrorObject, this.transform.position, Quaternion.identity);
        if (GO_MirrorObject)
        {
            mirrorGO = GO_MirrorObject.GetComponent<MirrorGameObject>();
            mirrorGO.SetGameObjectToMirror(this.gameObject);
        }
    }

    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        if (mirrorGO)
        {
            mirrorGO.StopMirroring();    
        }
    }

    protected override void OnActivate(XRBaseInteractor obj)
    {
        Core.Ins.AudioManager.PlayPauseMusic();
    }

    //public override void OnDeactivated(XRBaseInteractor obj)
    //{
    //}

    protected new void Update()
    {
        if (mirrorGO)
        {
            if (_grabInteractable.isSelected)
            {
                mirrorGO.StartMirroring();
            }
            else
            {
                mirrorGO.StopMirroring();
            }
        }
        
        base.Update();
        //    if (Input.GetKeyDown(KeyCode.I))
        //    {
        //        Core.Ins.AudioManager.AdjustVolume(1, Audio_Type.master);
        //    }
        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        Core.Ins.AudioManager.AdjustVolume(-1, Audio_Type.master);
        //    }
    }

    public override void HandleGesture(ENUM_XROS_Gesture gesture, float distance)
    {
        switch (gesture)
        {
            case ENUM_XROS_Gesture.Up:
                Core.Ins.AudioManager.AdjustVolume(1, ENUM_Audio_Type.Master);
                break;
            case ENUM_XROS_Gesture.Down:
                Core.Ins.AudioManager.AdjustVolume(-1, ENUM_Audio_Type.Master);
                break;
            case ENUM_XROS_Gesture.Forward:
                break;
            case ENUM_XROS_Gesture.Backward:
                break;
            case ENUM_XROS_Gesture.RotateClockwise:
                break;
            case ENUM_XROS_Gesture.RotateCounterclockwise:
                break;
            case ENUM_XROS_Gesture.Left:
                break;
            case ENUM_XROS_Gesture.Right:
                break;
            default:
                break;
        }
    }
}
