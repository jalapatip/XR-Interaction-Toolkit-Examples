using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class Switch_Base : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;    
    private void OnEnable()
    {
        if (!_grabInteractable)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }
        
        _grabInteractable.onActivate.AddListener(OnActivated);
        _grabInteractable.onDeactivate.AddListener(OnDeactivated);
        
    }

    private void OnDisable()
    {
        _grabInteractable.onActivate.RemoveListener(OnActivated);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivated);
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
        
    }
}
