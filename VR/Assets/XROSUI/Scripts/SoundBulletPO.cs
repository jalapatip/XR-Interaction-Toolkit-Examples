using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBulletPO : PooledObject
{
    //public GameObject go;
    public SoundBulletPO()
    {
        AudioBehavior = new NormalAudioClass();
        MoveBehavior = new MovewithTranslate();
        _initPosition = new Vector3(-3, 2, 0);
    } 

    public override void Init()
    {
        go = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        go.transform.Rotate(90, 0, 0);
        SetPosition(_initPosition);
        go.transform.SetParent(this.transform);
        go.name = "bullet";

        //AssignAudio("Gun");

        go.SetActive(false);
    }
}