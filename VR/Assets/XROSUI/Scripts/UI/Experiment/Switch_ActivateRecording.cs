using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_ActivateRecording : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        Dev.Log("Activated " + this.name);
        
        if(!Core.Ins.DataCollection.IsRecording())
        {
            Core.Ins.DataCollection.StartRecording();    
        }
        else
        {
            Core.Ins.DataCollection.StopRecording();
        }
    }
}
