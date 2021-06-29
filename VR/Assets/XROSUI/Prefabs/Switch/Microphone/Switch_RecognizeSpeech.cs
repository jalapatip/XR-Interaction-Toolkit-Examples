using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;


public class Switch_RecognizeSpeech : Switch_Base
{

    void Start()
    {

    }

    
    protected override void OnActivated(XRBaseInteractor obj)
    {
        //Core.Ins.Microphone.StartListeningForKeywords();
        Core.Ins.Microphone.ToggleListeningForKeywords();
    }
}
