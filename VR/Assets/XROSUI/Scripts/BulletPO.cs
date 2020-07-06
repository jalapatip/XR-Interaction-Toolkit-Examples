using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPO : PooledObject
{
    //public GameObject go;
    public BulletPO()
    {
        _initPosition = new Vector3(-3, 2, 0);
    } 

    public override void Init()
    {
        go = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        go.transform.Rotate(90, 0, 0);
        SetPosition(_initPosition);
        go.transform.SetParent(this.transform);
        go.name = "bullet";

        AssignAudio("Gun");

        go.SetActive(false);
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