using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum XROSMessageTypes { Message_Tutorial, Message_Status, Message_Lore}
//Message_Tutorial: Provide user with tutorial messages
//Message_Status: Provide user with component's status update
//Message_Lore: Provide user with information to immerse them in the world

public delegate void Delegate_NewInGameMessage(string name);

public class Manager_InGameMessages : MonoBehaviour
{
    public static event Delegate_NewInGameMessage EVENT_NewInGameMessage;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Log("This is an in game message");
        }
    }

    public void Log(string s)
    {
        EVENT_NewInGameMessage?.Invoke(s);
    }
}
