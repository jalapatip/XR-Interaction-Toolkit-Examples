using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class AudioPO : PooledObject
{
    public AudioPO()
    {
        _initPosition = new Vector3(-3, 2, 0);
    }

    public override void Init()
    {
        //throw new System.NotImplementedException();
        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        SetPosition(_initPosition);
        go.transform.SetParent(this.transform);
        go.name = "audioPO";
        go.AddComponent<XRGrabInteractable>();

        go.SetActive(false);

        AssignAudio("Gun");
    }

    private void AssignAudio(string audioName)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(audioName);
    }

    public bool OutOfRange(float d)
    {
        return Vector3.Distance(go.transform.position, _initPosition) > d;
    }
}