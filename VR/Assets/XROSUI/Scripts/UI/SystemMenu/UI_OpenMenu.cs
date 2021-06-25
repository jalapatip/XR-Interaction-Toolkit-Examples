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
        //Fail Safe to remind Dev to assign the button
        if (button == null)
        {
            button = this.GetComponent<Button>();
            Dev.LogWarning("Button is not assigned in " + this.name);
        }

        if (button)
        {
            button.onClick.AddListener(OpenAssociatedMenu);
        }
    }
    public void OpenAssociatedMenu()
    {
        Core.Ins.SystemMenu.OpenMenu(AssociatedMenuType);
    }
}
