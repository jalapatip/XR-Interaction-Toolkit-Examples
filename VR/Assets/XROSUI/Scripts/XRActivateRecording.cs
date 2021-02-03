using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRActivateRecording : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;
    
    protected void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();

        // _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        // _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        // _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        // _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        // _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        _grabInteractable.onActivate.AddListener(OnActivate);
        _grabInteractable.onDeactivate.AddListener(OnDeactivate);
        
    }
    protected virtual void OnActivate(XRBaseInteractor obj)
    {
        Dev.Log("Activated " + this.name);
        //Core.Ins.DataCollection.StartRecording();
        if(!Core.Ins.DataCollection.IsRecording())
        {
            Core.Ins.DataCollection.StartRecording();    
        }
        else
        {
            Core.Ins.DataCollection.StopRecording();
        }
    }

    //This only triggers while the object is grabbed (grip button) and the trigger button is initially released
    protected virtual void OnDeactivate(XRBaseInteractor obj)
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
