using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class AudioPO : PooledObject
{
    public AudioPO()
    {
        AudioBehavior = new NormalAudioClass();
        _initPosition = new Vector3(-3, 2, 0);
    }

    public override void Init()
    {
        //throw new System.NotImplementedException();
        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        SetPosition(_initPosition);
        go.transform.SetParent(this.transform);
        go.name = "AudioPO";
        go.AddComponent<XRGrabInteractable>();

        //AssignAudio("Memories");

        go.SetActive(false);
    }
}