using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour, IPoolable
{
    public GameObject go;
    public Vector3 _initPosition;
    public IAudioBehavior AudioBehavior { private get; set; }
    public IMoveBehavior MoveBehavior { private get; set; }


    #region Constructor
    public PooledObject(IAudioBehavior audioBehavior = null, IMoveBehavior moveBehavior = null)
    {
        AudioBehavior = audioBehavior ?? new NormalAudioClass();
        MoveBehavior = moveBehavior ?? new MovewithTranslate();
    }
    #endregion


    #region Abstract Base Class Features
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

    public bool OutOfRange(float dist)
    {
        return Vector3.Distance(go.transform.position, _initPosition) > dist;
    }
    #endregion


    #region IAudioBehavior Features
    public void AssignAudio(string audioName)
    {
        AudioBehavior.AssignAudio(go, audioName);
    }
    #endregion


    #region IMoveBehavior Features
    public void MoveForward(Vector3 v)
    {
        MoveBehavior.MoveForward(go, v);
    }
    #endregion
}
