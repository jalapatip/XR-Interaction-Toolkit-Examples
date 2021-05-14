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
        
        //Core.Ins.Microphone.
        //Core.Ins.AudioRecorderManager.Debug_StartRecording();
    }

    protected override void OnDeactivate(XRBaseInteractor obj)
    {
        base.OnDeactivate(obj);
        
        //Core.Ins.AudioRecorderManager.DebugStopRecording();
    }
}
