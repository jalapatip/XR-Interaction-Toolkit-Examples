using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPO : MonoBehaviour, IPooledObject
{
    public GameObject go;
    Vector3 _initPosition = new Vector3(-3, 2, 0);

    public void Init()
    {
        go = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        go.transform.Rotate(90, 0, 0);
        go.transform.position = _initPosition;
        go.transform.SetParent(this.transform);
        go.name = "bullet";

        AssignAudio("Gun");

        go.SetActive(false);
    }

    public bool IsActive()
    {
        return go.activeInHierarchy;
    }

    public void InActivate()
    {
        go.SetActive(false);
    }

    public void Activate()
    {
        go.SetActive(true);
        go.transform.position = _initPosition;
    }

    private void AssignAudio(string audioName)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(audioName);
    }

    public void MoveForward(float v1, float v2, float v3)
    {
        go.transform.Translate(v1, v2, v3);
    }

    public bool OutOfRange(float d)
    {
        return Vector3.Distance(go.transform.position, _initPosition) > d;
    }
}