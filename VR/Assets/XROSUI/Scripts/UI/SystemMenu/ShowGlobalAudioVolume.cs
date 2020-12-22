using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowGlobalAudioVolume : ShowValue
{
    public ENUM_Audio_Type audioType;
    private string audioTypeLabel = "Volume: ";
    // Start is called before the first frame update
    private void Start()
    {
        //As this is 
        switch (audioType)
        {
            case ENUM_Audio_Type.Master:
                Controller_Audio.EVENT_NewVolumeMaster += HandleValueChange;
                audioTypeLabel = "Master Volume: ";
                break;
            case ENUM_Audio_Type.Voice:
                Controller_Audio.EVENT_NewVolumeVoice += HandleValueChange;
                audioTypeLabel = "Voice Volume: ";
                break;
            case ENUM_Audio_Type.Music:
                Controller_Audio.EVENT_NewVolumeMusic += HandleValueChange;
                audioTypeLabel = "Music Volume: ";
                break;
            case ENUM_Audio_Type.Sfx:
                Controller_Audio.EVENT_NewVolumeSfx += HandleValueChange;
                audioTypeLabel = "Sfx Volume: ";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
    }

    protected override string FormatValue(float f)
    {
        return audioTypeLabel + ((int) (f * 100f)).ToString() + "%";
    }
}