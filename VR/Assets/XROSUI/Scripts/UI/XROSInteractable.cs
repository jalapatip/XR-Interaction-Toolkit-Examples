using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

/// <summary>
/// Basic Template for a simple Interactable for VR
/// </summary>
public class XROSInteractable : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private MeshRenderer _meshRenderer;

    private bool _held = false;

    private void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        _grabInteractable.onActivate.AddListener(OnActivate);
    }

    private void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        _grabInteractable.onActivate.RemoveListener(OnActivate);
    }

    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
    }

    #region XRBaseInteractor

    private void OnSelectEnter(XRBaseInteractor obj)
    {
        _meshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.OnGrab);
        _held = true;
    }

    private void OnSelectExit(XRBaseInteractor obj)
    {
        _meshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.Default);
        _held = false;
    }

    private void OnLastHoverExit(XRBaseInteractor obj)
    {
        if (!_held)
        {
            _meshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.Default);
        }
    }

    private void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        if (!_held)
        {
            _meshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.OnHover);
        }
    }

    private void OnActivate(XRBaseInteractor obj)
    {

    }
    #endregion XRBaseInteractor
}