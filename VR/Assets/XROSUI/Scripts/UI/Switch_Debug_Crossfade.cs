using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Switch_Debug_Crossfade : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        Core.Ins.VisualManager.PlayCrossfadeEffect(0.5f);
    }
}