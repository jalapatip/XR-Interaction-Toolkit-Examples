using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This switch minimize script work by setting gameobjects inactive.
/// To use this switch, drag and drop things that should be set inactive in the list when setting up the prefab.
/// To avoid minimizing itself, the prefab should look like:
/// GO_XXX <- top level gameobject that's there to store any script that should always be running
/// GO_XXX > GO_ThingsThatShouldBeMinimized
/// GO_XXX > Switch_Minimize Prefab
/// 
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class Switch_Minimize : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;

    private bool _isMinimized = false;
    [FormerlySerializedAs("IsMinimized")] 
    public bool MinimizeAtStart;
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
        //DebugUpdate();
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