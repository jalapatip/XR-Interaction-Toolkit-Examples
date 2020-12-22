using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This is the class that receives events and display the text on screen
public class Text_ShowXROSInput : MonoBehaviour
{
    string compiledMessages = "";
    string targetCompiledMessages = "barbara had been waiting";
    public TMP_Text text;

    public TimerClass timer;
    // Start is called before the first frame update
    void Start()
    {
        if (!text)
        {
            text = this.GetComponent<TMP_Text>();
        }
        XROSInput.EVENT_NewInput += CompileMessage;
        XROSInput.EVENT_NewRemoveInput += RemoveMessage;
        XROSInput.EVENT_NewBackspace += Backspace;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            text.text = "";
            compiledMessages = "";
        }

        if (compiledMessages == targetCompiledMessages)
        {
            timer.TestComplete();       
        }
    }
    public void CompileMessage(string s)
    {
        compiledMessages += s;
        text.text = compiledMessages;
    }

    public void RemoveMessage()
    {
        compiledMessages = "";
        text.text = compiledMessages;
    }

    public void Backspace()
    {
        if (compiledMessages.Length == 0)
        {
            return;
        }
        compiledMessages = compiledMessages.Substring(0, compiledMessages.Length - 1);
        text.text = compiledMessages;
    }
}