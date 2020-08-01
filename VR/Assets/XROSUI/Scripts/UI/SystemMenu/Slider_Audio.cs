using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Slider_Audio : MonoBehaviour
{
    public AudioMixer mixer;
    public ENUM_Audio_Type type;

    public void SetVolume(int level)
    {
        Core.Ins.AudioManager.SetVolume(level, type);
    }
}
