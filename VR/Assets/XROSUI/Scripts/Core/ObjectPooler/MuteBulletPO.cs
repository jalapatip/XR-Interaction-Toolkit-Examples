using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteBulletPO : MonoBehaviour, IPoolable
{
    //public GameObject go;
    Vector3 _initPosition = new Vector3(0, 4, 0);

    public void Init(PrimitiveType primType)
    {
        var go = GameObject.CreatePrimitive(primType);
        
        go.transform.Rotate(90, 0, 0);
        go.transform.position = _initPosition;
        go.transform.SetParent(this.transform);
        go.name = "mute_bullet";

        //this.gameObject.SetActive(false);
        this.Deactivate();
    }

    public void MoveForward(Vector3 v)
    {
        this.transform.Translate(v);
    }

    public bool OutOfRange(float dist)
    {
        return Vector3.Distance(this.transform.position, _initPosition) > dist;
    }
    
    #region IPoolable
    public void Activate()
    {
        this.gameObject.SetActive(true);
        this.transform.position = _initPosition;
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
        this.transform.position = _initPosition;
        //MasterObjectPooler.Ins.ReturnPooledObject("MuteBullet", this.GetGameObject());
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