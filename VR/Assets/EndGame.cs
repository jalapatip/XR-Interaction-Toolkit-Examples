using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



[RequireComponent(typeof(XRGrabInteractable))]
public class EndGame : MonoBehaviour
{

    protected XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;
    public GameObject startButton;




    public void Start()
    {

    }
    void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();


        _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);


        // assignedTeleportationAnchor.onSelectExit.AddListener(TeleportToAvatar);
        _colliderList = this.GetComponentsInChildren<Collider>();
    }

    private Collider[] _colliderList;

    private void OnDisable()
    {

        _grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);

    }



    protected virtual void OnHoverEnter(XRBaseInteractor obj)
    {
        startButton.GetComponent<StartandStop>().StopGame();
    }

}