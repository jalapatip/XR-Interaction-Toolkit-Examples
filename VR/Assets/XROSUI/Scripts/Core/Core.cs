using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note: In Project Setting, Script Execution Order, Core should be very early
/// Otherwise Public variables may not be assigned in inspectors when others try to interact with it in Awake
/// </summary>
public class Core : MonoBehaviour
{
    public GameObject PF_Core;

    #region Singleton Setup
    private static Core _ins = null;
    public static Core Ins
    {
        get
        {
            return _ins;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (_ins != null && _ins != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _ins = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion Singleton Setup

    public Controller_Audio AudioManager;
    public Controller_Visual VisualManager;
    public Controller_Scene SceneManager;
    public Controller_UIEffects UIEffectsManager;
    public Controller_HumanScale HumanScaleManager;
    public Controller_Scenario ScenarioManager;
    public Controller_XR XRManager;
    public Controller_AudioRecorder AudioRecorderManager;
    public Controller_Screenshot ScreenshotManager;
    public Manager_Account Account;
    public Manager_SystemMenu SystemMenu;
    public Manager_InGameMessages Messages;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("SINGLETON UPDATE");
    }
}
