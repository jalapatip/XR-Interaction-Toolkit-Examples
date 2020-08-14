using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FunctionToggler : MonoBehaviour
{//This class is used to make enter/exit behave like toggle.

    public bool InitalState;
    public UnityEvent TurnedOn;
    public UnityEvent TurnedOff;
    private bool _toggleState;

    // Start is called before the first frame update
    private void Start()
    {
        _toggleState = InitalState;
    }

    // Update is called once per frame
    private void Update()
    {

    }

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
