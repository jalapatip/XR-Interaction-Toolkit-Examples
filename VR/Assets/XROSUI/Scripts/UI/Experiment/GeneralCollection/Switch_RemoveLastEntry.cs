using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_RemoveLastEntry : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        print("remove last entry");
        Core.Ins.DataCollection.RemoveLastEntry();
    }
}
