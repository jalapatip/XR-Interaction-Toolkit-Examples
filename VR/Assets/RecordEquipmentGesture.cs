using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RecordEquipmentGesture : MonoBehaviour
{
    private DataCollection_Exp1Gestures _exp1;
    
    protected XRGrabInteractable _grabInteractable;

    private void OnEnable()
    {
        if (!_grabInteractable)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }
        
        //_grabInteractable.onActivate.AddListener(OnActivated);
        //_grabInteractable.onDeactivate.AddListener(OnDeactivated);
        _grabInteractable.onSelectEnter.AddListener(OnSelectedEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectedExit);
    }

    private void OnDisable()
    {
        //_grabInteractable.onActivate.RemoveListener(OnActivated);
        //_grabInteractable.onDeactivate.RemoveListener(OnDeactivated);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectedEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectedExit);
        
    }
    void Start()
    {
        _exp1 = (DataCollection_Exp1Gestures) Core.Ins.DataCollection.currentExperiment;
    }
    
    protected void OnSelectedEnter(XRBaseInteractor obj)
    {
        _exp1.StartGesture();
    }
    protected void OnSelectedExit(XRBaseInteractor obj)
    {
        _exp1.EndGesture();
    }
}
