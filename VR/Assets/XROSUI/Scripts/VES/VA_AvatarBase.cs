using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(XRGrabInteractable))]
public class VA_AvatarBase : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;
    
    public Camera assignedCamera;
    public AudioListener assignedAudioListener;
    public AudioSource assignedAudioSource;

    //public GameObject  
    //Powen: It seems XRITK did not intend IsActivated to be a variable. We can add one ourselves but it could cause more confusion
    //It may need to be handled case by case
    //private bool _isActivated = false;
    public ENUM_XROS_AvatarTypes avatarTypes = ENUM_XROS_AvatarTypes.FullAvatar;

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

    //This only triggers while the object is grabbed (grip button) and the trigger button is initially pushed
    protected virtual void OnActivate(XRBaseInteractor obj)
    {
        //Dev.Log("Activated " + this.name);
        //_isActivated = true;
    }

    //This only triggers while the object is grabbed (grip button) and the trigger button is initially released
    protected virtual void OnDeactivate(XRBaseInteractor obj)
    {
        //Dev.Log("Deactivated " + this.name);
        //_isActivated = false;
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

    private void SetAvatarActive(bool b)
    {
        if (assignedCamera)
        {
            assignedCamera.enabled = b;
        }
        if (assignedAudioListener)
        {
            Core.Ins.Avatar.DisableMainListener(b);
            assignedAudioListener.enabled = b;
        }
        if (assignedAudioSource)
        {
            assignedAudioSource.enabled = b;
        }
    }

    protected virtual void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        SetAvatarActive(true);
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

    private void VA_Update()
    {
    }
}