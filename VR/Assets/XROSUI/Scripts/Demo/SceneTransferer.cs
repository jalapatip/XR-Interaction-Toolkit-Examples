using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This is used for SUI 2020 Demo scenes
///
///
/// TODO:
/// scene 0 - Virtual Equipment
/// scene 1 - Adjustable Pointer
/// Scene 2 - 3D Typing
///
/// We want to change it to be based on names so if someone messed with the scene order, it shouldn't affect it.
/// We should also make sure the scene name conform to naming standard.
/// DemoScene_SUI_x
/// DemoScene_SUI_y
/// DemoScene_SUI_z
///
/// The SceneTransferer Switch should be renamed.
/// It also needs to use sockets.
/// 
/// </summary>
public class SceneTransferer : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    
    public int SceneIdToLoad;
    //public string SceneNameToLoad;

    private void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.onActivate.AddListener(OnActivated);
    }

    private void OnDisable()
    {
        _grabInteractable.onActivate.RemoveListener(OnActivated);
    }

    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
    }

    private void OnActivated(XRBaseInteractor obj)
    {
        Core.Ins.SceneManager.LoadSceneById(SceneIdToLoad);
        //Core.Ins.SceneManager.LoadSceneByName();
    }
}
