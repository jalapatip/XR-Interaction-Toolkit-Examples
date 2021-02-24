using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager current;
    private void Awake()
    {
        current = this;
    }

    public event Action onTriggerPress;
    public void TriggerPress()
    {
        if (onTriggerPress != null)
        {
            onTriggerPress();
        }
    }
}
