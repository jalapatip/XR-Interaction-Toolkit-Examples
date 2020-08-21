using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BTN_TakeScreenshot : MonoBehaviour
{
    private Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(TakeAShot);
    }

    public void TakeAShot()
    {
        Core.Ins.ScreenshotManager.TakeAShot();
    }
}