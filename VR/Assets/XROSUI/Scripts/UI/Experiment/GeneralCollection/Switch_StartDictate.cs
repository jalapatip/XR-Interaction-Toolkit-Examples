using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_StartDictate : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        Dev.Log("Start Dictate & Start Gesture " + Time.time);
        Core.Ins.Microphone.DictationStart();
        ((SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment()).StartGesture();
    }
    protected override void OnDeactivated(XRBaseInteractor obj)
    {
        Dev.Log("Stop Dictate");
        Core.Ins.Microphone.DictationStop();
        //((SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment()).EndGesture();
    }
}
