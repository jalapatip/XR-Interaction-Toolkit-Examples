using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VE_Microphone : VE_EquipmentBase
{
    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
        
        Core.Ins.AudioRecorderManager.StartRecording();
    }

    protected override void OnDeactivate(XRBaseInteractor obj)
    {
        base.OnDeactivate(obj);
        
        Core.Ins.AudioRecorderManager.StopRecording();
    }
}
