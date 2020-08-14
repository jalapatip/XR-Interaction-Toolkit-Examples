using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VrMicrophone : VrEquipment
{
    protected override void OnActivate(XRBaseInteractor obj)
    {
        Core.Ins.AudioRecorderManager.StartRecording();
    }

    protected override void OnDeactivate(XRBaseInteractor obj)
    {
        Core.Ins.AudioRecorderManager.StopRecording();
    }
}
