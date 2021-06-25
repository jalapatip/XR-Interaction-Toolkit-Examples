using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// This script is used by Switch GO to show or hide objects.
///
/// Usage:
/// Parent GO - handles any logic dealing with the children
/// This GO - a XR Grab Interactable with Rigidbody so the player can interact with it
/// Other GO - put anything visual or should not be function into the HideList
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class UI_HideObject : MonoBehaviour
{
    XRGrabInteractable m_GrabInteractable;
    public List<GameObject> HideList;
    private bool bShow = false;

    void OnEnable()
    {
        m_GrabInteractable = GetComponent<XRGrabInteractable>();

        m_GrabInteractable.onActivate.AddListener(OnActivate);

    }

    private void OnDisable()
    {
        m_GrabInteractable.onActivate.RemoveListener(OnActivate);
    }
    private void OnActivate(XRBaseInteractor obj)
    {
        foreach(GameObject go in HideList)
        {
            go.SetActive(bShow);
        }
        bShow = !bShow;
    }
    //private void OnDeactivate(XRBaseInteractor obj)
    //{
    //}
    //private void OnFirstHoverEnter(XRBaseInteractor obj)
    //{
    //}
    //private void OnHoverEnter(XRBaseInteractor obj)
    //{
    //}
    //private void OnHoverExit(XRBaseInteractor obj)
    //{
    //}    
    //private void OnLastHoverExit(XRBaseInteractor obj)
    //{
    //}
    //private void OnSelectEnter(XRBaseInteractor obj)
    //{
    //}
    //private void OnSelectExit(XRBaseInteractor obj)
    //{
    //}
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
