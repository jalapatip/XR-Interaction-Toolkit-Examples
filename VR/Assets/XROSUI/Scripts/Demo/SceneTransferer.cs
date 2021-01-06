using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneTransferer : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    public int SceneIdToLoad;
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
    }
}
