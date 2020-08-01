using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class EventInteractionForSwitch : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    // Start is called before the first frame update
    private void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.onSelectEnter.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        _grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(XRBaseInteractor obj)
    {
        Core.Ins.ScenarioManager.SetFlag("SwitchGrabbed",true);
    }
}
