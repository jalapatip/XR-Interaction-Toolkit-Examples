using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_Translate : Switch_Base
{
    private bool _translateMode;
    public GameObject GO_parent;

    private XRBaseInteractor _InteractorObj;
    private Vector3 _interactorPosition;
    private Quaternion _interactorRotation;

    private Vector3 startingLocalPosition;


    void Start()
    {
        startingLocalPosition = this.transform.localPosition;
    }
    protected override void OnActivated(XRBaseInteractor obj)
    {
        _translateMode = true;
        _InteractorObj = obj;
        
        PositionHelper();

    }

    private void PositionHelper()
    {
        _interactorPosition = _InteractorObj.transform.position;
        _interactorRotation = _InteractorObj.transform.rotation;
    }
    
    protected override void OnDeactivated(XRBaseInteractor obj)
    {
        _translateMode = false;
        _InteractorObj = null;
        //
    }

    protected override void OnSelectedEnter(XRBaseInteractor obj)
    {
        startingLocalPosition = this.transform.localPosition;
        //Dev.Log("staring:" + startingLocalPosition);
    }
    protected override void OnSelectedExit(XRBaseInteractor obj)
    {
        _translateMode = false;
        _InteractorObj = null;
        //Dev.Log("before: " + this.transform.localPosition);
        //this.transform.localPosition = startingLocalPosition;
        //Dev.Log("after: " + this.transform.localPosition);
    }

    void Update()
    {
        if (_translateMode && _InteractorObj)
        {
            var currentPosition = _InteractorObj.transform.position;

            GO_parent.transform.localPosition += currentPosition - _interactorPosition;
            PositionHelper();
        }
        else
        {
            this.transform.localPosition = startingLocalPosition;    
        }
    }
}
