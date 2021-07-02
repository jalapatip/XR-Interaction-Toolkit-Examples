using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;
using UnityEngine.SceneManagement;


public class Switch_AddVoiceCommand
    : Switch_Base
{
    public GameObject PF_Object;
    public string VoiceCommand;
    void Start()
    {

    }

    
    protected override void OnActivated(XRBaseInteractor obj)
    {
        //Core.Ins.Microphone.StartListeningForKeywords();
        Core.Ins.Microphone.RegisterVoiceCommand(VoiceCommand, CreateGameObject);
    }

    public void CreateGameObject()
    {
        GameObject.Instantiate(PF_Object, Core.Ins.XRManager.GetLeftDirectController().transform.position,
            Quaternion.identity);
    }
}
