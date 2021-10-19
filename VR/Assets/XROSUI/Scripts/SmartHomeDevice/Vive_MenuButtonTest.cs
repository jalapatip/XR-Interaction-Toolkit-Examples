using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Vive_MenuButtonTest : XrButtonUtilizer
{
    void Start()
    {
        //Primary Button is the typical 'in-game menu button' for HTC Vive
        //It is the small circle button above the large circular touchpad.
        TargetXrButton = CommonUsages.primaryButton;
    }

    //In case you want to use the Update function to drive any behaviors.
    //Anything that's related to the button should be done in OnPushed, OnPushing, and OnReleased
    protected override void Update()
    {
        base.Update();
    }
    
    protected override void OnPushed()
    {
        Core.Ins.Microphone.DictationStart();
        ((SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment()).StartGesture();
        Core.Ins.AudioManager.PlaySfx("Beep_SFX");
        
        Dev.Log(Time.time +  " - Button Pushed");
    }

    protected override void OnPushing()
    {
    }
    
    protected override void OnReleased()
    {
        ((SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment()).EndGesture();
        
        Dev.Log(Time.time +  " - Button Released");
    }
}