using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class UI_RemoveLastEntry : MonoBehaviour
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
            button.onClick.AddListener(RemoveLastEntry);
            button.onClick.AddListener(OnClickFeedback);
        }
    }

    public void OnDisable()
    {
        if (button)
        {
            button.onClick.RemoveListener(RemoveLastEntry);
            button.onClick.RemoveListener(OnClickFeedback);
        }
    }

    public void RemoveLastEntry()
    {
        Core.Ins.DataCollection.GetCurrentExperiment().RemoveLastEntry();
    }

    public void OnClickFeedback()
    {
    
    }
    
}
