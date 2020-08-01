using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class MinimizeSwitch : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private MeshRenderer _meshRenderer;

    private bool _held = false;
    [FormerlySerializedAs("isMinimized")] [FormerlySerializedAs("bMinimize")] 
    public bool IsMinimized;
    public List<GameObject> MinimizeList;

    private void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _grabInteractable.onFirstHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnHoverExit);
        _grabInteractable.onSelectEnter.AddListener(OnGrabbed);
        _grabInteractable.onSelectExit.AddListener(OnReleased);
        _grabInteractable.onActivate.AddListener(OnActivated);

        Minimize();
    }

    private void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnHoverExit);
        _grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        _grabInteractable.onSelectExit.RemoveListener(OnReleased);
        _grabInteractable.onActivate.RemoveListener(OnActivated);
    }

    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        // if (m_Held)
        // {
        //     print("grabbing " + Time.time);
        // }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Minimize();
        }
    }

    #region XRBaseInteractor

    private void OnGrabbed(XRBaseInteractor obj)
    {
        _meshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.OnGrab);
        _held = true;
    }

    private void OnReleased(XRBaseInteractor obj)
    {
        _meshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.Default);
        _held = false;
    }

    private void OnHoverExit(XRBaseInteractor obj)
    {
        if (!_held)
        {
            _meshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.Default);
        }
    }

    private void OnHoverEnter(XRBaseInteractor obj)
    {
        if (!_held)
        {
            _meshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.OnHover);
        }
    }

    private void OnActivated(XRBaseInteractor obj)
    {
        Minimize();
    }

    #endregion XRBaseInteractor

    private void Minimize()
    {
        IsMinimized = !IsMinimized;
        foreach (var go in this.MinimizeList)
        {
            go.SetActive(IsMinimized);
        }
    }
}