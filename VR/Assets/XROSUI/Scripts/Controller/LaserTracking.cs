using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// When the ray interactor selects this gameObject, the ray will now point toward this GameObject no matter the orientation of the controller.
/// This allows the user to adjust the orientation of the ray with the use of one hand.
/// </summary>
public class LaserTracking : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private MeshRenderer _meshRenderer;
    private GameObject _laserEmitterGameObject;

    void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();

        _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
    }

    private void OnDisable()
    {
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
    }
    
    // Start is called before the first frame update
    private void Start()
    {

    }
    
    private void OnSelectEnter(XRBaseInteractor obj)
    {
        //NOTE LaserEmitter must be set as attach Transform on XRRayInteractor for this to work
        //Might want to add a check for null
        if (obj.attachTransform.gameObject)
        {
            _laserEmitterGameObject = obj.attachTransform.gameObject;    
        }
        else
        {
            Dev.LogWarning("Missing attachTransform");
        }        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (_grabInteractable.isSelected)
        {
            LockOn();
        }
    }

    //This changes the ray's forward direction while it is selected by the ray.
    private void LockOn()
    {
        var direction = transform.position - _laserEmitterGameObject.transform.position;

        //Change the forward of our laserEmitter
        _laserEmitterGameObject.transform.forward = direction;
    }

    public bool IsSelected()
    {
        return _grabInteractable.isSelected;
    }
}
