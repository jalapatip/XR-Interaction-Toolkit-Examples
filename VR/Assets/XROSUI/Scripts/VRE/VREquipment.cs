using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class VrEquipment : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;
    private MaterialPropertyBlock _propBlock;
    private bool _isInSocket = false;
    private bool _isEquipped = false;
    private float _lastHeldTime;

    public float timeBeforeReturn = 0.5f;
    public GameObject socket;
    public XROSMenuTypes menuTypes = XROSMenuTypes.Menu_General;
    
    void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
        
        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        _grabInteractable.onActivate.AddListener(OnActivate);
        _grabInteractable.onDeactivate.AddListener(OnDeactivate);
    }

    private void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        _grabInteractable.onActivate.RemoveListener(OnActivate);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivate);
    }

    protected virtual void OnActivate(XRBaseInteractor obj)
    {
        //print("Activated " + this.name);
    }
    protected virtual void OnDeactivate(XRBaseInteractor obj)
    {
        //print("Deactivated " + this.name);
    }

    protected virtual void OnSelectEnter(XRBaseInteractor obj)
    {
        _isInSocket = false;
        this.transform.SetParent(null);
    }

    protected virtual void OnSelectExit(XRBaseInteractor obj)
    {
        StopPhysics();
    }

    protected virtual void OnLastHoverExit(XRBaseInteractor obj)
    {

    }

    protected virtual void OnFirstHoverEnter(XRBaseInteractor obj)
    {

    }

    protected virtual void OnHoverEnter(XRBaseInteractor obj)
    {

    }

    public virtual void AlternateFunction()
    {
        Dev.Log("Alternate Function: " + this.name);
    }

    public virtual void HandleGesture(ENUM_XROS_Gesture gesture, float distance)
    {

    }

    protected void Update()
    {
        VREUpdate();
    }

    private void VREUpdate()
    {
        if (_grabInteractable.isSelected)
        {
            _lastHeldTime = Time.time;
        }
        else if (!_grabInteractable.isSelected && Time.time > _lastHeldTime + timeBeforeReturn)
        {
            
            if (!_isInSocket)
            //if (_isEquipped && !_isInSocket)
            {
                this.transform.localRotation = Quaternion.identity;
                this.transform.position = socket.transform.position;

                this.transform.SetParent(socket.transform);
                
                StopPhysics();
                this.transform.localRotation = Quaternion.identity;
                this.transform.position = socket.transform.position;

                _isInSocket = true;
            }
        }
    }

    private void StopPhysics()
    {
        _rigidbody.ResetCenterOfMass();
        _rigidbody.ResetInertiaTensor();
        _rigidbody.angularDrag = 0;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }

    public bool IsSelcted()
    {
        return _grabInteractable.isSelected;
    }
}
