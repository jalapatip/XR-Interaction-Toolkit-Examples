using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class BTN_TestVolume : MonoBehaviour
{
    [Tooltip("Assign in Inspector the sound to test volume")]
    public string AudioClipName = "";
    
    [Tooltip("Assign in Inspector the type of Audio Type")]
    public ENUM_Audio_Type enumAudioType;

    public void TestVolume()
    {
        switch (enumAudioType)
        {
            case ENUM_Audio_Type.Master:
                Core.Ins.AudioManager.PlayMaster(AudioClipName);
                break;
            case ENUM_Audio_Type.Music:
                Core.Ins.AudioManager.PlayMusic(AudioClipName);
                break;
            case ENUM_Audio_Type.Sfx:
                Core.Ins.AudioManager.Play2DAudio(AudioClipName);
                break;
            case ENUM_Audio_Type.Voice:
                Core.Ins.AudioManager.PlayVoice(AudioClipName);
                break;
        }
    }
}
