using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SmartDevice : XrInteractable
{
    public Camera assignedCamera;
    public AudioListener assignedAudioListener;
    public AudioSource assignedAudioSource;
    
    protected override void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        print("Hover");
        SetAvatarActive(true);
    }

    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        SetAvatarActive(false);
    }

    public void SetAvatarActive(bool b)
    { 
        //print(this.name + " is set active " + b);
        if (assignedCamera)
        {
            assignedCamera.enabled = b;
        }

        if (assignedAudioListener)
        {
            Core.Ins.Avatar.DisableMainListener(b);
            assignedAudioListener.enabled = b;
        }

        if (assignedAudioSource)
        {
            assignedAudioSource.enabled = b;
        }
    }
}