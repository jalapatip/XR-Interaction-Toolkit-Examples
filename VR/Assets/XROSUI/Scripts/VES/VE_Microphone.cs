using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VE_Microphone : VE_EquipmentBase
{
//    AudioSource
    private void Start()
    {
//        GetAudioClipFromSelectedMicrophone();
    }

    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
        Dev.Log("Microphone Activated");
        //Core.Ins.Microphone
        //Core.Ins.Microphone.Debug_StartRecording();
    }

    protected override void OnDeactivate(XRBaseInteractor obj)
    {
        base.OnDeactivate(obj);
        Dev.Log("Microphone Deactivated");
        //Core.Ins.Microphone.DebugStopRecording();
    }
}
