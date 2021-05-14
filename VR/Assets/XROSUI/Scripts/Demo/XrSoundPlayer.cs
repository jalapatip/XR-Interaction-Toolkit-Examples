using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrSoundPlayer : VE_EquipmentBase
{
    public ENUM_Audio_Type typeOfSound;
    public AudioSource source;

    protected override void OnActivate(XRBaseInteractor obj)
    {
        if (source.isPlaying)
        {
            source.Pause();    
        }
        else
        {
            source.Play();
            source.UnPause();
        }
        
        // switch (typeOfSound)
        // {
        //     case ENUM_Audio_Type.Master:
        //         break;
        //     case ENUM_Audio_Type.Voice:
        //         Core.Ins.AudioManager.PlayPauseVoiceChannel();
        //         break;
        //     case ENUM_Audio_Type.Music:
        //         Core.Ins.AudioManager.PlayPauseMusic();
        //         break;
        //     case ENUM_Audio_Type.Sfx:
        //         Core.Ins.AudioManager.PlayPauseSfc();
        //         break;
        //     default:
        //         break;
        // }
    }
}