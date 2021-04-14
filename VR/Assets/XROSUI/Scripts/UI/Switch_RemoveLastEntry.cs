using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_RemoveLastEntry : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        Core.Ins.DataCollection.currentExperiment.RemoveLastEntry();
    }
}
