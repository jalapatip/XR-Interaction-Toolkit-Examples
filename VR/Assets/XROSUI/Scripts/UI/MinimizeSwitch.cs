using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class MinimizeSwitch : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;

    private bool _isMinimized = false;
    [FormerlySerializedAs("IsMinimized")] public bool MinimizeAtStart;
    public List<GameObject> MinimizeList;

    private void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();

        _grabInteractable.onActivate.AddListener(OnActivated);

        if (MinimizeAtStart)
        {
            Minimize(MinimizeAtStart);
        }
    }

    private void OnDisable()
    {
        _grabInteractable.onActivate.RemoveListener(OnActivated);
    }

    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Minimize(!_isMinimized);
        }
    }

    private void OnActivated(XRBaseInteractor obj)
    {
        Minimize(!_isMinimized);
    }

    private void Minimize(bool b)
    {
        _isMinimized = b;
        foreach (var go in this.MinimizeList)
        {
            go.SetActive(_isMinimized);
        }
    }
}