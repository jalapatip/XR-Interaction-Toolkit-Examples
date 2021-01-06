using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class VES_OutlineHelper : MonoBehaviour
{
    public Outline[] myOutlines;
    private XRGrabInteractable _grabInteractable;

    void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        //_grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        //_grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        //_grabInteractable.onSelectExit.AddListener(OnSelectExit);
        //_grabInteractable.onActivate.AddListener(OnActivate);
        //_grabInteractable.onDeactivate.AddListener(OnDeactivate);
    }

    private void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        //_grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        //_grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        //_grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        //_grabInteractable.onActivate.RemoveListener(OnActivate);
        //_grabInteractable.onDeactivate.RemoveListener(OnDeactivate);
    }

    void Start()
    {
        //var test = this.GetComponents(_myOutlines);
        myOutlines = this.GetComponentsInChildren<Outline>(true);
        foreach (var o in myOutlines)
        {
            o.OutlineWidth = 3f;
            o.OutlineColor = Color.black;
        }
    }

    private void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        SetOutlines(true);
    }

    private void OnLastHoverExit(XRBaseInteractor obj)
    {
        SetOutlines(false);
    }
    
    private void SetOutlines(bool b)
    {
        if (myOutlines.Length > 0)
        {
            foreach (var o in myOutlines)
            {
                o.enabled = b;
            }
        }
        else
        {
            Dev.LogError("Where is " + this.name + "'s outline");
        }
    }
}