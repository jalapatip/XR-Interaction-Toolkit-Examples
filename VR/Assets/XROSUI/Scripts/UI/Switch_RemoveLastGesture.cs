using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_RemoveLastGesture : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        DataCollection_ExpGestures exp = (DataCollection_ExpGestures) Core.Ins.DataCollection.currentExperiment;
        exp.RemoveLastGesture();
    }
}
