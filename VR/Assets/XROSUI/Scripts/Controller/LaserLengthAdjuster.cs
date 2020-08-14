using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Laser Length Adjuster refers to the selectable sphere that surrounds your controller.
/// When you point at it with the other ray interactor, it will reveal itself as interactable.
/// By selecting Laser Length Adjuster on Controller A with Controller B, Controller B will change its laser length
/// based on the distance between the two controllers. It also changes the orientation on Controller B
/// </summary>
public class LaserLengthAdjuster : MonoBehaviour
{
    [TooltipAttribute("Assign using inspector from Hierarchy")]
    public XrosRayControllerManager xrosRayControllerManager;

    private XRGrabInteractable _grabInteractable;

    //for hiding the selectable sphere when not in use
    private MeshRenderer _meshRenderer;

    //this is the xrRayInteractor that grabbed the sphere, the one that we want to change
    private XRRayInteractor _xrRayInteractor;

    void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);

        _meshRenderer.enabled = false;
    }

    private void OnDisable()
    {
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
    }

    private void OnSelectEnter(XRBaseInteractor obj)
    {
        _xrRayInteractor = obj.GetComponent<XRRayInteractor>();

        //if it's being hovered by a rayInteractor, show that you can manipulate it
        if (_xrRayInteractor)
        {
            Core.Ins.ScenarioManager.SetFlag("LaserLengthChanged", true);
        }
    }

    private void OnSelectExit(XRBaseInteractor obj)
    {
        if (_xrRayInteractor)
        {
            _xrRayInteractor = null;
        }
    }

    private void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        _meshRenderer.enabled = true;
    }

    private void OnLastHoverExit(XRBaseInteractor obj)
    {
        _meshRenderer.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLaserLengthAndOrientation();
    }

    private void UpdateLaserLengthAndOrientation()
    {
        if (!_grabInteractable.isSelected) return;

        //Without this line, the sphere will stay frozen in place while the controller it's on is free to move away.
        var receiverPosition = xrosRayControllerManager.transform.position;
        this.transform.position = receiverPosition;
        var fromPosition = _xrRayInteractor.transform.position;

        xrosRayControllerManager.AdjustRayLength(_xrRayInteractor,
            CalculateNewLaserLength(fromPosition, receiverPosition));

        var direction = receiverPosition - fromPosition;
        xrosRayControllerManager.AdjustEmitterForwardDirection(_xrRayInteractor, direction);
    }

    //This formula may be tweaked based on our need
    //Right now, it simply assigns it the distance between the two controllers
    private float CalculateNewLaserLength(Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }
}