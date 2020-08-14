using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class LaserEmitterPositionRetainer : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private XRDirectInteractor _xrDirectInteractor;

    [TooltipAttribute("Assign using inspector")]
    public XrosRayControllerManager xrosRayControllerManager;

    public GameObject selfController;
    public GameObject secondController;
    public LaserTracking laserTracker;
    public LaserLengthAdjuster target;

    private Vector3 _previousDirection;

    private Vector3 _normalVector;
    private Quaternion _localRotation;

    public float emitterForwardOffsetValue = 0.06f;
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

        //_previousDirection = (secondController.transform.position - transform.position).normalized;
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

    //This is called by 
    public void onGrabingObject()
    {
        //change the direction of laser 
        if (laserTracker && !laserTracker.IsSelected())
        {
            var transform1 = transform;
            _localRotation = transform1.localRotation;
            transform1.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    public void onReleasingObject() //go back to the direction of laser before grabbing stuff.
    {
        if (laserTracker && !laserTracker.IsSelected())
        {
            transform.localRotation = _localRotation;
        }
    }

    private void PositionRetainer()
    {
        if (_grabInteractable.isSelected)
        {
            // print("grabbing " + Time.time);
            //var position = transform.position;
            var position = xrosRayControllerManager.transform.position +
                       xrosRayControllerManager.transform.forward * emitterForwardOffsetValue;
            // position =
            //     selfController.transform.position + selfController.transform.forward * emitterForwardOffsetValue;
            transform.position = position;
            //Vector3 newDirection = (secondController.transform.position - position).normalized;
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