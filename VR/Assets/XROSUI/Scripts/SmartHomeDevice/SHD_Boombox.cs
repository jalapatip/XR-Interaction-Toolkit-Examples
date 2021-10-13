using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://taimiso0319.booth.pm/items/2070078?registration=1
/// </summary>
public class SHD_Boombox : SmartHomeDevice
{
    public AudioSource myAudioSource;
    public ParticleSystem myParticleSystem;
    
    private bool IsPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            this.StartDevice(true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            this.StartDevice(false);
        }
    }

    public override void StartDevice(bool b)
    {
        if (b && !IsPlaying)
        {
            myAudioSource.Play();
            myParticleSystem.Play();
            IsPlaying = true;
        }
        else if(!b && IsPlaying)
        {
            myAudioSource.Stop();
            myParticleSystem.Stop();
            IsPlaying = false;
        }
    }
}
