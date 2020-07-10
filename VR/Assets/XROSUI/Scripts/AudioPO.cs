using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class AudioPO : PooledObject
{
    public GestureArea gestureArea;
    public VRAudioPO VE;

    AudioSource _audioSource;
    Vector3 initPosition = new Vector3(-4, 1, 0);

    public AudioPO()
    {
        AudioBehavior = new NormalAudioClass();
        _initPosition = initPosition;
    }

    public override void Init()
    {
        //throw new System.NotImplementedException();
        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        SetPosition(_initPosition);
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        go.name = "AudioPO";

        go.transform.SetParent(this.transform);
        
        go.AddComponent<XRGrabInteractable>();
        go.GetComponent<Rigidbody>().useGravity = false;
        go.GetComponent<Rigidbody>().isKinematic = true;

        //init VREquipment
        VE = go.AddComponent<VRAudioPO>();

        VE.socket = new GameObject();
        VE.socket.transform.position = initPosition;

        //init gestureArea
        gestureArea = go.AddComponent<GestureArea>();
        
        gestureArea.GestureCore = new GameObject("AudioPO_GestureCore");
        gestureArea.GestureCore.transform.position = initPosition;

        gestureArea.Area = new GameObject("AudioPO_Area");
        gestureArea.Area.transform.localScale = new Vector3(10, 10, 10);

        gestureArea.GO_VE = go;

        // assign audio
        _audioSource = go.GetComponent<AudioSource>();
        AssignAudio("Journey");

        go.SetActive(false);
    }

    public void IncreaseVolume()
    {
        Debug.Log("Call Increase Volume");
        if (!_audioSource) 
            _audioSource = go.GetComponent<AudioSource>();
        _audioSource.volume += 0.1f;
    }

    public void DecreaseVolume()
    {
        Debug.Log("Call Decrease Volume");
        if (!_audioSource)
            _audioSource = go.GetComponent<AudioSource>();
        _audioSource.volume -= 0.1f;
    }
}