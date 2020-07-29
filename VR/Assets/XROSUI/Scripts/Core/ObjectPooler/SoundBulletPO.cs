using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBulletPO : MonoBehaviour, IPoolable
{
    public GameObject go;
    Vector3 _initPosition = new Vector3(2, 2, 2);

    public void Init(PrimitiveType primType)
    {
        go = GameObject.CreatePrimitive(primType);

        go.transform.Rotate(90, 0, 0);
        go.transform.SetParent(this.transform);
        go.name = "sound_bullet";

        InActivate();
    }

    public void Activate()
    {
        go.SetActive(true);
        go.transform.position = _initPosition;
    }

    public void InActivate()
    {
        go.SetActive(false);
        go.transform.position = _initPosition;
    }

    public bool IsActive()
    {
        return go.activeInHierarchy;
    }

    public void AssignAudio(string audioName)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(audioName);
    }

    public void MoveForward(Vector3 v)
    {
        go.transform.Translate(v);
    }

    public bool OutOfRange(float dist)
    {
        return Vector3.Distance(go.transform.position, _initPosition) > dist;                   
    }
}