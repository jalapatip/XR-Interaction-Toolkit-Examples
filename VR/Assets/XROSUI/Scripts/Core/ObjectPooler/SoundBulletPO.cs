using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBulletPO : MonoBehaviour, IPoolable
{
    //public GameObject go;
    Vector3 _initPosition = new Vector3(2, 2, 2);

    public void Init(PrimitiveType primType)
    {
        var go = GameObject.CreatePrimitive(primType);

        go.transform.Rotate(90, 0, 0);
        go.transform.SetParent(this.transform);
        go.transform.SetParent(this.transform);
        go.name = "sound_bullet";

        Deactivate();
    }
    
    public void AssignAudio(string audioName)
    {
        var audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(audioName);
    }

    public void MoveForward(Vector3 v)
    {
        this.gameObject.transform.Translate(v);
    }

    public bool OutOfRange(float dist)
    {
        return Vector3.Distance(this.transform.position, _initPosition) > dist;                   
    }
    
    #region IPoolable
    public void Activate()
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = _initPosition;
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
        this.gameObject.transform.position = _initPosition;
        //MasterObjectPooler.Ins.ReturnPooledObject("SoundBullet", this.GetGameObject());
    }

    public bool IsActive()
    {
        return this.gameObject.activeInHierarchy;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    #endregion
}