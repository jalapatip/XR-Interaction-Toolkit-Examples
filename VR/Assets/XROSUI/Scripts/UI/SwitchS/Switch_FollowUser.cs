using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_FollowUser : Switch_Base
{
    public Tool_FollowXRRigV2 followScript;
    protected override void OnActivated(XRBaseInteractor obj)
    {
        followScript.ToggleFollowState();
    }
}
