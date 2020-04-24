﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note: In Project Setting, Script Execution Order, Core should be very early
/// Otherwise Public variables may not be assigned in inspectors when others try to interact with it in Awake
/// </summary>
public class Core : MonoBehaviour
{
    public GameObject PF_Core;
    private static Core ins = null;

    // Game Instance Singleton
    public static Core Ins
    {
        get
        {
            return ins;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (ins != null && ins != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            ins = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public Controller_Audio AudioManager;
    public Controller_Visual VisualManager;
    public Controller_Scene SceneManager;
    public Controller_UIEffects UIEffectsManager;
    public Controller_HumanScale HumanScaleManager;
    public Controller_Scenario ScenarioManager;
    public Controller_XR XRManager;
    public Controller_AudioRecorder AudioRecorderManager;
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
