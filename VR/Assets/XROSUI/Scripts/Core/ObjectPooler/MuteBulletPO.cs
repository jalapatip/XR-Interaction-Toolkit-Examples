using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteBulletPO : MonoBehaviour, IPoolable
{
    public GameObject go;
    Vector3 _initPosition = new Vector3(0, 4, 0);

    public void Init(PrimitiveType primType)
    {
        go = GameObject.CreatePrimitive(primType);

        go.transform.Rotate(90, 0, 0);
        go.transform.position = _initPosition;
        go.transform.SetParent(this.transform);
        go.name = "mute_bullet";

        go.SetActive(false);
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

    public void MoveForward(Vector3 v)
    {
        go.transform.Translate(v);
    }

    public bool OutOfRange(float dist)
    {
        return Vector3.Distance(go.transform.position, _initPosition) > dist;
    }
}