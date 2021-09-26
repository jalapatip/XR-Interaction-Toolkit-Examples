﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Core is a class using the Singleton Design Pattern. It is meant as the one stop for your common needs.
/// Under Core, there are a variety of Modules (may be called Manager or Controllers) that will perform commonly used
/// functionalities for their respective area. These modules should not contain scene-specific functionalities.  
/// For example, the Audio Module allows other scripts to adjust volume levels or request a new song.
/// A in-game VR music player with buttons would go through Core's Audio Module to play music and adjust volume, but
/// it will have code for responding to button pushes in a separate script.
/// 
/// Setup Note:
/// In Project Setting, Script Execution Order, Core should be very early to ensure it is available for other scripts.
/// Otherwise Public variables may not be assigned in inspectors when others try to interact with it in Awake
/// </summary>
public class Core : MonoBehaviour
{
    #region Singleton Setup

    public static Core Ins { get; private set; } = null;

    private void Awake()
    {
        // if the static reference to singleton has already been initialized somewhere AND it's not this one, then this
        // GameObject is a duplicate and should not exist
        if (Ins != null && Ins != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Ins = this;
            //So this singleton will stay when we change scenes.
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion Singleton Setup

    public void Start()
    {
        SystemMenu.LoadModule();
    }

    public Controller_Audio AudioManager;
    public Controller_Visual VisualManager;
    public Controller_Scene SceneManager;
    public Controller_UIEffects UIEffectsManager;
    public Controller_HumanScale HumanScaleManager;
    public Controller_Scenario ScenarioManager;
    public Controller_XR XRManager;
    public Manager_Microphone Microphone;
    public Controller_Screenshot ScreenshotManager;
    public Manager_Account Account;
    public Manager_SystemMenu SystemMenu;
    public Manager_InGameMessages Messages;
    public Manager_Avatar Avatar;
    public Manager_Privacy Privacy;
    public Manager_VES VES;
    public Manager_DataCollection DataCollection;
    public Manager_Debug Debug;
}