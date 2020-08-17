using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class LaserEmitterPositionRetainer : MonoBehaviour
{
    [TooltipAttribute("Assign using inspector")]
    public XrosRayControllerManager xrosRayControllerManager;
    [TooltipAttribute("Assign using inspector")]
    public float emitterForwardOffsetValue = 0.06f;
    
    private XRGrabInteractable _grabInteractable;
    private XRDirectInteractor _xrDirectInteractor;
    private Vector3 _previousDirection;
    private Vector3 _normalVector;
    private Quaternion _localRotation;

    private void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();

        _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        //Make sure the emitter starts at the position based on emitterForwardOffsetValue
        transform.position = xrosRayControllerManager.transform.position +
                        xrosRayControllerManager.transform.forward * emitterForwardOffsetValue;
    }

    private void OnDisable()
    {
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
    }

    private void OnSelectEnter(XRBaseInteractor obj)
    {
        Core.Ins.ScenarioManager.SetFlag("EmitterGrabbed", true);
        //NEW
        _xrDirectInteractor = obj.GetComponent<XRDirectInteractor>();
        if (_xrDirectInteractor)
        {
            _previousDirection = (_xrDirectInteractor.transform.position - transform.position).normalized;
        }
    }

    private void OnSelectExit(XRBaseInteractor obj)
    {
        if (_xrDirectInteractor)
        {
            _xrDirectInteractor = null;
        }
    }
    
    private void OnLastHoverExit(XRBaseInteractor obj)
    {
    }

    private void PositionRetainer()
    {
        if (_grabInteractable.isSelected)
        {
            //Determine the new position after being grabbed and translated by predefined offset
            var position = xrosRayControllerManager.transform.position +
                       xrosRayControllerManager.transform.forward * emitterForwardOffsetValue;
            
            //This make sure the emitter moves with the controller 
            transform.position = position;
            
            var newDirection = (_xrDirectInteractor.transform.position - position).normalized;
            
            var normalVector = Vector3.Cross(newDirection, _previousDirection);
            transform.RotateAround(position, normalVector,
                -Vector3.Angle(_previousDirection, newDirection));
            
            _previousDirection = newDirection;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        PositionRetainer();
    }
}