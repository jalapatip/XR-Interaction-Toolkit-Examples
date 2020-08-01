using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class BTN_TestVolume : MonoBehaviour
{
    public string AudioClipName = "";
    [FormerlySerializedAs("m_audioType")] public ENUM_Audio_Type mEnumAudioType;

    public void TestVolume()
    {
        switch (mEnumAudioType)
        {
            case ENUM_Audio_Type.master:
                Core.Ins.AudioManager.PlayMaster(AudioClipName);
                break;
            case ENUM_Audio_Type.music:
                Core.Ins.AudioManager.PlayMusic("Beep_SFX");
                break;
            case ENUM_Audio_Type.sfx:
                Core.Ins.AudioManager.Play2DAudio(AudioClipName);
                break;
            case ENUM_Audio_Type.voice:
                Core.Ins.AudioManager.PlayVoice(AudioClipName);
                break;
        }
    }
}
