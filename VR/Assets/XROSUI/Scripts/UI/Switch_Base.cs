using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class Switch_Base : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;

    private Rigidbody _rigidbody;
    private Vector3 startingPosition;
    private Vector3 startingEulerRotation;
    private void OnEnable()
    {
        if (!_grabInteractable)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }
        
        _grabInteractable.onActivate.AddListener(OnActivated);
        _grabInteractable.onDeactivate.AddListener(OnDeactivated);
        _grabInteractable.onSelectExit.AddListener(OnSelectedExit);

        startingPosition = this.transform.position;
        startingEulerRotation = this.transform.localEulerAngles;
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        _grabInteractable.onActivate.RemoveListener(OnActivated);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivated);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectedExit);
    }

    protected virtual void OnSelectedExit(XRBaseInteractor arg0)
    {
        
    }

    protected virtual void OnActivated(XRBaseInteractor obj)
    {
        
    }
    
    protected virtual void OnDeactivated(XRBaseInteractor obj)
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_grabInteractable.isSelected)
        {
            ReturnToStartingLocation();    
        }
    }

    protected void ReturnToStartingLocation()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        this.transform.position = startingPosition;
        this.transform.localEulerAngles = startingEulerRotation;
    }
}
