using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class UI_SaveData : MonoBehaviour
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
            button.onClick.AddListener(SaveData);
            button.onClick.AddListener(OnClickFeedback);
        }
    }

    public void OnDisable()
    {
        if (button)
        {
            button.onClick.RemoveListener(SaveData);
            button.onClick.RemoveListener(OnClickFeedback);
        }
    }

    public void SaveData()
    {
        Core.Ins.DataCollection.SaveExperimentData();
    }

    public void OnClickFeedback()
    {
    
    }
    
}
