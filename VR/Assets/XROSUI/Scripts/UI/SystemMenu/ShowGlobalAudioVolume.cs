using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowGlobalAudioVolume : ShowValue
{
    public ENUM_Audio_Type audioType;
    private string _audioTypeLabel = "Volume: ";
    // Start is called before the first frame update
    private void Start()
    {
        switch (audioType)
        {
            case ENUM_Audio_Type.Master:
                Controller_Audio.EVENT_NewVolumeMaster += HandleValueChange;
                _audioTypeLabel = "Master Volume: ";
                break;
            case ENUM_Audio_Type.Voice:
                Controller_Audio.EVENT_NewVolumeVoice += HandleValueChange;
                _audioTypeLabel = "Voice Volume: ";
                break;
            case ENUM_Audio_Type.Music:
                Controller_Audio.EVENT_NewVolumeMusic += HandleValueChange;
                _audioTypeLabel = "Music Volume: ";
                break;
            case ENUM_Audio_Type.Sfx:
                Controller_Audio.EVENT_NewVolumeSfx += HandleValueChange;
                _audioTypeLabel = "Sfx Volume: ";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override string FormatValue(float f)
    {
        return _audioTypeLabel + ((int) (f * 100f)).ToString() + "%";
    }
}