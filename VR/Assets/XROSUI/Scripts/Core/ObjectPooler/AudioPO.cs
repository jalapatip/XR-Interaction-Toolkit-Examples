using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class AudioPO : MonoBehaviour, IPoolable
{
    public GameObject go;
    public GestureArea gestureArea;
    public VRAudioPO VE;
    AudioSource _audioSource;

    private Vector3 _initPosition = new Vector3(-4, 1, 0);

    public void Init(PrimitiveType primType)
    {
        go = GameObject.CreatePrimitive(primType);
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        go.name = "AudioPO";
        go.transform.SetParent(this.transform);

        go.AddComponent<XRGrabInteractable>();
        go.GetComponent<Rigidbody>().useGravity = false;
        go.GetComponent<Rigidbody>().isKinematic = true;

        Deactivate();

        //init VREquipment
        VE = go.AddComponent<VRAudioPO>();

        VE.socket = new GameObject();
        VE.socket.transform.position = _initPosition;

        //init gestureArea
        gestureArea = go.AddComponent<GestureArea>();

        gestureArea.GestureCore = new GameObject("AudioPO_GestureCore");
        gestureArea.GestureCore.transform.position = _initPosition;

        gestureArea.Area = new GameObject("AudioPO_Area");
        gestureArea.Area.transform.localScale = new Vector3(10, 10, 10);

        gestureArea.GO_VE = go;

        // assign audio
        _audioSource = go.GetComponent<AudioSource>();
        AssignAudio("Journey");
    }

    public void AssignAudio(string audioName)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(audioName);
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
    
    
    #region IPoolable
    public void Activate()
    {
        go.SetActive(true);
        go.transform.position = _initPosition;
    }

    public void Deactivate()
    {
        go.SetActive(false);
        go.transform.position = _initPosition;
        //MasterObjectPooler.Ins.ReturnPooledObject("AudioPO", this.GetGameObject());
    }

    public bool IsActive()
    {
        return go.activeInHierarchy;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    #endregion
}