using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

//Test Script
//Changes a grabbed object's color upon activation
//Only used in 3DFolder
public class ColorChange : MonoBehaviour
{
    public Material greenMaterial = null;
    public Material pinkMaterial = null;
    private MeshRenderer _mRenderer = null;
    private XRGrabInteractable grabInteractable = null;

    // Start is called before the first frame update
    private void Awake()
    {
        _mRenderer = GetComponent<MeshRenderer>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.onActivate.AddListener(SetPink);
        grabInteractable.onDeactivate.AddListener(SetGreen);
    }
    private void OnDestroy()
    {
        grabInteractable.onActivate.RemoveListener(SetPink);
        grabInteractable.onDeactivate.RemoveListener(SetGreen);
    }
    private void SetGreen(XRBaseInteractor interactor)
    {
        _mRenderer.material = greenMaterial;
    }
    private void SetPink(XRBaseInteractor interactor)
    {
        _mRenderer.material = pinkMaterial;
    }
}
