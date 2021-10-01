using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(XRGrabInteractable))]
public class AvatarBaseTest : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;

    public Camera assignedCamera;

    public GameObject assignedAvatarRepresentation;

    //public GameObject  
    //Powen: It seems XRITK did not intend IsActivated to be a variable. We can add one ourselves but it could cause more confusion
    //It may need to be handled case by case
    //private bool _isActivated = false;
    public ENUM_XROS_AvatarTypes avatarTypes = ENUM_XROS_AvatarTypes.Eyes;

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

       // assignedTeleportationAnchor.onSelectExit.AddListener(TeleportToAvatar);
        _colliderList = this.GetComponentsInChildren<Collider>();
    }

    void Start()
    {
        
    }

  

    private Collider[] _colliderList;

    private void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        _grabInteractable.onActivate.RemoveListener(OnActivate);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivate);

       // assignedTeleportationAnchor.onSelectExit.AddListener(TeleportToAvatar);
    }

    /*public void EnableColliders(bool b)
    {
        foreach (var c in _colliderList)
        {
            c.enabled = b;
        }
    }

   /* private void TeleportToAvatar(XRBaseInteractor arg0)
    {
        ShowAvatar(true);
    }

    private bool _IsShown = false;

    private void ShowAvatar(bool b)
    {
        _IsShown = b;
        this.assignedAvatarRepresentation.SetActive(b);
    }
   */

    //This only triggers while the object is grabbed (grip button) and the trigger button is initially pushed
    protected virtual void OnActivate(XRBaseInteractor obj)
    {
 
       
    }


    //This only triggers while the object is grabbed (grip button) and the trigger button is initially released
    protected virtual void OnDeactivate(XRBaseInteractor obj)
    {

 
    }

    protected virtual void OnSelectEnter(XRBaseInteractor obj)
    {

    }

    protected virtual void OnSelectExit(XRBaseInteractor obj)
    {
    }

    protected virtual void OnLastHoverExit(XRBaseInteractor obj)
    {
        SetAvatarActive(false);
    }

    public void SetAvatarActive(bool b)
    {
        //print(this.name + " is set active " + b);
        if (assignedCamera)
        {
            assignedCamera.enabled = b;
        }

    
    }

    protected virtual void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        SetAvatarActive(true);
    ;
    }

    protected virtual void OnHoverEnter(XRBaseInteractor obj)
    {
        SetAvatarActive(true);
    }

    public virtual void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
    }

    protected void Update()
    {
        VA_Update();
    }


    public float distanceToHideAvatar = 2f;

    private void VA_Update()
    {
        //if (_IsHidden)
        {
            // var distance = Vector3.Distance(Core.Ins.XRManager.GetXrCamera().gameObject.transform.position,
            //     this.gameObject.transform.position);

            var position1 = new Vector2(Core.Ins.XRManager.GetXrCamera().transform.position.x,
                Core.Ins.XRManager.GetXrCamera().transform.position.z);
            var position2 = new Vector2(this.transform.position.x, this.transform.position.z);
            var distance = Vector2.Distance(position1, position2);
            //            print(distance);
          /*  if (distance > 1)
            {
                ShowAvatar(true);
            }
            else
            {
                ShowAvatar(false);
            }*/
        }
    }
}