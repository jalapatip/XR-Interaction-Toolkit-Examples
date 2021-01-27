using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VA_AvatarDoll : MonoBehaviour
{
    private VA_AvatarBase _associatedAvatar;
    protected XRGrabInteractable _grabInteractable;
    public TeleportationAnchor assignedTeleportationAnchor;
    public GameObject assignedMiniavatarRepresentation;

    void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
//        _rigidbody = GetComponent<Rigidbody>();

        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
//        _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        // _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        // _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        // _grabInteractable.onActivate.AddListener(OnActivate);
        // _grabInteractable.onDeactivate.AddListener(OnDeactivate);
    }

    private void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        //_grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        // _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        // _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        // _grabInteractable.onActivate.RemoveListener(OnActivate);
        // _grabInteractable.onDeactivate.RemoveListener(OnDeactivate);
    }
    
    public void Setup(VA_AvatarBase ava)
    {
        _associatedAvatar = ava;
        assignedTeleportationAnchor.teleportAnchorTransform =
            _associatedAvatar.assignedTeleportationAnchor.teleportAnchorTransform;
        _associatedAvatar.LinkMiniAvatar(this);
    }
    protected virtual void OnFirstHoverEnter(XRBaseInteractor obj)
    {
//        print("hover mini avatar");
        _associatedAvatar.SetAvatarActive(true);
    }
    
    protected virtual void OnLastHoverExit(XRBaseInteractor obj)
    {
        //print("leave mini avatar");
        _associatedAvatar.SetAvatarActive(false);
    }

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMiniavatar(bool b)
    {
        assignedTeleportationAnchor.gameObject.SetActive(b);
        assignedMiniavatarRepresentation.SetActive(b);
    }
}
