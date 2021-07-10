using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 2D UI
/// This is used in the system menu to make it easy to add buttons that take you to different sub menus
/// </summary>
public class UI_OpenMenu : MonoBehaviour
{
    public XROSMenuTypes AssociatedMenuType;
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
            button.onClick.AddListener(OpenAssociatedMenu);
            button.onClick.AddListener(OnClickFeedback);
        }
    }

    public void OnDisable()
    {
        if (button)
        {
            button.onClick.RemoveListener(OpenAssociatedMenu);
            button.onClick.RemoveListener(OnClickFeedback);
        }
    }

    public void OpenAssociatedMenu()
    {
        Core.Ins.SystemMenu.OpenMenu(AssociatedMenuType);
    }
    
    public void OnClickFeedback()
    {
        Core.Ins.AudioManager.PlaySfx("320181__dland__hint");
    }
}
