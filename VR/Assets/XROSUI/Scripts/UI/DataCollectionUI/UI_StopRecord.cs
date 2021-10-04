using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_StopRecord : MonoBehaviour
{
    public Button button;
    private void Start()
    {
    }
    public void OnEnable()
    {
        //Fail Safe to remind Dev to assign the button
        if (button == null)
        {
            button = this.GetComponent<Button>();
            Dev.LogWarning("Button is not assigned in " + this.name);
        }

        if (button)
        {
            button.onClick.AddListener(StopRecording);
            button.onClick.AddListener(OnClickFeedback);
        }
    }

    public void OnDisable()
    {
        if (button)
        {
            button.onClick.RemoveListener(StopRecording);
            button.onClick.RemoveListener(OnClickFeedback);
        }
    }

    public void StopRecording()
    {
        Core.Ins.DataCollection.StopRecording();
        transform.parent.Find("Start_Record").Find("Text").GetComponent<Text>().text = "Start Recording"; 
    }

    public void OnClickFeedback()
    {
    
    }
}
