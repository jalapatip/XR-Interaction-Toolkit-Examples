using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Text_ShowDevLog : MonoBehaviour
{
    private string _compiledMessages = "";
    //List<string> logMessages = new List<string>();
    public TMP_Text text;

    public void Awake()
    {
        if (!text)
        {
            text = this.GetComponentInChildren<TMP_Text>();
        }
        Dev.EVENT_NewLog += CompileMessage;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Dev.EventHandler_NewLog += CompileMessage;
        //Dev.Log("hello");

    }

    // Update is called once per frame
    void Update()
    {
        DebugUpdate();
    }

    //Track Debug Inputs here
    //https://docs.google.com/spreadsheets/d/1NMH43LMlbs5lggdhq4Pa4qQ569U1lr_O7HSHESEantU/edit#gid=0
    private void DebugUpdate()
    {
        if (Input.GetKeyUp(KeyCode.F8))
        {
            Dev.Log("Testing Text_ShowDevLog");
        }
    }

    public void CompileMessage(string s)
    {
        //logMessages.Add(s);

        _compiledMessages = "\n" + s + _compiledMessages;
        text.text = _compiledMessages;
    }
}