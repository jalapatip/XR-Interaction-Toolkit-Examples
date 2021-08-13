using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_LookAtUser : Switch_Base
{
    public List<Tool_LookAtXRRig> lookAtScripts;

    protected override void OnActivated(XRBaseInteractor obj)
    {
        //LookAtScript.enabled = !LookAtScript.enabled;
        foreach (var script in lookAtScripts)
        {
            script.ToggleLookAtStatus();
        }
    }
}
