using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteBulletPO : PooledObject
{
    //public GameObject go;
    public MuteBulletPO()
    {
        MoveBehavior = new MovewithTranslate();
        _initPosition = new Vector3(0, 2, 0);
    }

    public override void Init()
    {
        go = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        go.transform.Rotate(90, 0, 0);
        SetPosition(_initPosition);
        go.transform.SetParent(this.transform);
        go.name = "muteBullet";

        go.SetActive(false);
    }
}