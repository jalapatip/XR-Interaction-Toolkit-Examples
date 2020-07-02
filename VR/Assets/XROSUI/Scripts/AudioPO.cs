using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPO : MonoBehaviour
{
    public GameObject go;
    Vector3 _initPosition = new Vector3(-3, 2, 0);

    // Start is called before the first frame update

    void Awake()
    {
        go = new GameObject();
        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.SetParent(this.transform);
        ObjectPoolerImplement.Ins.RegisterAudioPO(this);
    }


    public void Init()
    {
        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = _initPosition;
        go.SetActive(false);
        AssignAudio("Gun");
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