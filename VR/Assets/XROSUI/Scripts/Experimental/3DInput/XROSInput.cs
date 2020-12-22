using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reference
//https://en.wikibooks.org/wiki/C_Sharp_Programming/Delegates_and_Events

public delegate void XROSInputHandler_NewInput(string logMessage);
public delegate void XROSInputHandler_NewRemoveInput();
public delegate void XROSInputHandler_NewBackspace();

public static class XROSInput
{
    public static event XROSInputHandler_NewInput EVENT_NewInput;
    public static event XROSInputHandler_NewRemoveInput EVENT_NewRemoveInput;
    public static event XROSInputHandler_NewBackspace EVENT_NewBackspace;
    public static void AddInput(string s)
    {
        EVENT_NewInput?.Invoke(s);
    }

    public static void RemoveInput()
    {
        EVENT_NewRemoveInput?.Invoke();
    }

    public static void Backspace()
    {
        EVENT_NewBackspace?.Invoke();
    }
}
