using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

/// <summary>
/// Basic script for handling highlight
/// </summary>
public class XrosGrabInteractableColorControl : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    public MeshRenderer assignedMeshRenderer;

    private void OnEnable()
    {
        OnEnableColor();
    }

    private void OnEnableColor()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        //assignedMeshRenderer = GetComponent<MeshRenderer>();

        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnterChangeColor);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExitChangeColor);
        _grabInteractable.onSelectEnter.AddListener(OnSelectEnterChangeColor);
        _grabInteractable.onSelectExit.AddListener(OnSelectExitChangeColor);
        _grabInteractable.onActivate.AddListener(OnActivateChangeColor);
        _grabInteractable.onDeactivate.AddListener(OnDeactivateChangeColor);
    }

    private void OnDisable()
    {
        OnDisableColor();
    }

    private void OnDisableColor()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnterChangeColor);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExitChangeColor);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnterChangeColor);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExitChangeColor);
        _grabInteractable.onActivate.RemoveListener(OnActivateChangeColor);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivateChangeColor);
    }

    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        
    }

    #region XRBaseInteractor
    private void OnSelectEnterChangeColor(XRBaseInteractor obj)
    {
        ChangeColor(Enum_XROSUI_Color.OnGrab);
    }

    private void OnSelectExitChangeColor(XRBaseInteractor obj)
    {
        ChangeColor(Enum_XROSUI_Color.Default);
    }

    private void OnLastHoverExitChangeColor(XRBaseInteractor obj)
    {
        if (!_grabInteractable.isSelected)
        {
            ChangeColor(Enum_XROSUI_Color.Default);
        }
    }

    private void OnFirstHoverEnterChangeColor(XRBaseInteractor obj)
    {
        if (!_grabInteractable.isSelected)
        {
            ChangeColor(Enum_XROSUI_Color.OnHover);
        }
    }

    private void OnActivateChangeColor(XRBaseInteractor obj)
    {
        ChangeColor(Enum_XROSUI_Color.OnActivate);
    }
    
    private void OnDeactivateChangeColor(XRBaseInteractor obj)
    {
        if (_grabInteractable.isSelected)
        {
            ChangeColor(Enum_XROSUI_Color.OnGrab);
        }
    }    
    #endregion XRBaseInteractor
    
    //Shortcut for selecting default colors that's specified in Core.UIEffects
    private void ChangeColor(Enum_XROSUI_Color e)
    {
         ChangeColor(Core.Ins.UIEffectsManager.GetColor(e));
    }
    
    //This method is the one that actually change the color of this GameObject.
    //Any child class could call this directly if they don't want to use the enum color
    //This may have to be changed based on the shader being used
    private void ChangeColor(Color c)
    {
        if (assignedMeshRenderer)
        {
            assignedMeshRenderer.material.color = c;    
        }
        else
        {
            Dev.LogError("Missing assigned MeshRenderer in GameObject" + this.name);
        }
        
    }
}