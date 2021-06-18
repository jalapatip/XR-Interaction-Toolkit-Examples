using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This class is used to make enter/exit behave like toggle.
//This is used in the PFB_Light/SwitchForLight in the TestScene_Room prefab

public class FunctionToggler : MonoBehaviour
{

    public bool InitalState;
    public UnityEvent TurnedOn;
    public UnityEvent TurnedOff;
    private bool _toggleState;

    public void Trigger()
    {
        _toggleState = !_toggleState;
        if (_toggleState)
        {
            TurnedOn.Invoke();
        }
        else
        {
            TurnedOff.Invoke();
        }
    }
}
