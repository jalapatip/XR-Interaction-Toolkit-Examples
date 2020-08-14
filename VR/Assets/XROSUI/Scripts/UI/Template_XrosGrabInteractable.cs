using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

/// <summary>
/// Basic Template for a simple Interactable for VR
/// References: XRITK's ComplexCube, BubbleGun
///References: VRBeginner
///References: XROSUI's VREquipment, KeyboardSetter
/// </summary>
public class Template_XrosGrabInteractable : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private MeshRenderer _meshRenderer;

    private void OnEnable()
    {
        OnEnableBase();
    }

    private void OnEnableBase()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onHoverExit.AddListener(OnHoverExit);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        _grabInteractable.onActivate.AddListener(OnActivate);
        _grabInteractable.onDeactivate.AddListener(OnDeactivate);
    }

    private void OnDisable()
    {
        OnDisableBase();
    }

    private void OnDisableBase()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onHoverExit.RemoveListener(OnHoverExit);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        _grabInteractable.onActivate.RemoveListener(OnActivate);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivate);
        
    }

    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        
    }

    #region XRBaseInteractor
    private void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        if (!_grabInteractable.isSelected)
        {
    
        }
    }
    
    private void OnHoverEnter(XRBaseInteractor obj)
    {
        if (!_grabInteractable.isSelected)
        {
    
        }
    }
    
    private void OnHoverExit(XRBaseInteractor obj)
    {
        if (!_grabInteractable.isSelected)
        {
    
        }
    }
    
    private void OnLastHoverExit(XRBaseInteractor obj)
    {
        if (!_grabInteractable.isSelected)
        {
            
        }
    }
    
    private void OnSelectEnter(XRBaseInteractor obj)
    {
        
    }

    private void OnSelectExit(XRBaseInteractor obj)
    {
        
    }

    private void OnActivate(XRBaseInteractor obj)
    {
        
    }
    
    private void OnDeactivate(XRBaseInteractor obj)
    {
        
    }
    #endregion XRBaseInteractor
}