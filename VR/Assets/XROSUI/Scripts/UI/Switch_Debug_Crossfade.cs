using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This script provides a Switch GameObject with the ability to play the crossfade effect.
/// It's mainly used to validate the crossfade works without needing to use keyboard keys to trigger the effect.
/// 
/// </summary>
public class Switch_Debug_Crossfade : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        Core.Ins.VisualManager.PlayCrossfadeEffect(0.5f);
    }
}