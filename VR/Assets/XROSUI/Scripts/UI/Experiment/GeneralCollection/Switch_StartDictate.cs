using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_StartDictate : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        Core.Ins.Microphone.DictationStart();
        ((SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment()).StartGesture();
        
        Dev.Log(Time.time +  " - Switch Activated");
    }
    protected override void OnDeactivated(XRBaseInteractor obj)
    {
        //Core.Ins.Microphone.DictationStop();
        ((SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment()).EndGesture();
        Dev.Log(Time.time +  " - Switch Deactivated");
    }
}
