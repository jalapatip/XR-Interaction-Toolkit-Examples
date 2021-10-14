using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(XRGrabInteractable))]
public class TankGameController : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;

    


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

        
    }

  

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
       
    }

   

    protected virtual void OnFirstHoverEnter(XRBaseInteractor obj)
    {
       
        
    }

    protected virtual void OnHoverEnter(XRBaseInteractor obj)
    {
      
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