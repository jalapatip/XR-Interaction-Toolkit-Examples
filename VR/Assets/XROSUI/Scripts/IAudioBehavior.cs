using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioBehavior
{
    void AssignAudio(GameObject go, string audioName);
}

public class NormalAudioClass: IAudioBehavior
{
    public void AssignAudio(GameObject go, string audioName)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(audioName);
    }
}