using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public GameObject go;
    public Vector3 _initPosition;

    public abstract void Init();


    public bool IsActive()
    {
        return go.activeInHierarchy;
    }
    public void Activate()
    {
        SetPosition(_initPosition);
        go.SetActive(true);
    }

    public void InActivate()
    {
        go.SetActive(false);
    }

    public void SetPosition(Vector3 initPosition)
    {
        go.transform.position = initPosition;
    }
}
