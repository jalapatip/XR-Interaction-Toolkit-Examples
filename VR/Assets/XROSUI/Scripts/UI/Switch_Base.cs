using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class Switch_Base : MonoBehaviour
{
    //We use XR Interaction Toolkit (XRITK) to handle XR devices and then interaction through their Interactor and Interactable System.
    //Switch is a 3D object that function similar to a 2D button in User Interface. The 3D object can be interacted with by Interactors through having XRGrabInteractable.
    protected XRGrabInteractable _grabInteractable;

    private Rigidbody _rigidbody;
    private Vector3 startingPosition;
    private Vector3 startingEulerRotation;
    
    //When this script is first enabled, we subscribe to relevant events
    private void OnEnable()
    {
        //If _grabInteractable isn't assigned, we'll attempt to assign one automatically.
        //Powen: Is this needed if we already have RequireComponent before the class declaration?
        if (!_grabInteractable)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }
        
        //if interactor is able to interact with the interactable, it's first in Hover State
        //if the user push the grip button, the user would grab the interactable and enter Select State
        //if the user push the trigger button with the index finger while in Select State, it will enter OnActivated
        //Note: it is possible to leave OnSelectedEnter before leaving OnDeactivated, which could potentially cause bugs.
        //This script does not currently handle Hover State. Simple highlighting is handled by another script and is subject to change.
        _grabInteractable.onSelectEnter.AddListener(OnSelectedEnter);
        _grabInteractable.onSelectEnter.AddListener(MarkAsGrabbed);
        _grabInteractable.onSelectExit.AddListener(OnSelectedExit);
        _grabInteractable.onActivate.AddListener(OnActivated);
        _grabInteractable.onDeactivate.AddListener(OnDeactivated);

        //startingPosition = this.transform.position;
        //startingEulerRotation = this.transform.localEulerAngles;
        //Powen: We track the initial location of the switch so that after interactions, we could return it to where it was. 
        //This may be subject to change as we learn how to utilize XRITK's sockets better.
        startingPosition = this.transform.localPosition;
        startingEulerRotation = this.transform.localEulerAngles;
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    //Opposite of OnEnable, This is when the little checkmark by the script gets checked or if this script is disabled by other scripts
    private void OnDisable()
    {
        _grabInteractable.onActivate.RemoveListener(OnActivated);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivated);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectedEnter);
        _grabInteractable.onSelectEnter.RemoveListener(MarkAsGrabbed);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectedExit);
    }

    protected virtual void OnSelectedEnter(XRBaseInteractor arg0)
    {
        
    }

    private bool _grabbed = false;
    protected void MarkAsGrabbed(XRBaseInteractor arg0)
    {
        _grabbed = true;
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
        if (!_grabInteractable.isSelected && _grabbed)
        {
            ReturnToStartingLocation();
            _grabbed = false;
        }
    }

    protected void ReturnToStartingLocation()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        //this.transform.position = startingPosition;
        this.transform.localPosition = startingPosition;
        this.transform.localEulerAngles = startingEulerRotation;
    }
}
