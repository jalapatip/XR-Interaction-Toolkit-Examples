using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public GameObject go;
    public Vector3 _initPosition; 

    public IActiveBehavior ActiveBehavior { private get; set; }
    public IAudioBehavior AudioBehavior { private get; set; }
    public IMoveBehavior MoveBehavior { private get; set; }
    //public IPoolBehavior PoolBehavior { private get; set; }

    #region Constructor
    public PooledObject(IActiveBehavior activeBehavior = null, IAudioBehavior audioBehavior = null, IMoveBehavior moveBehavior = null)
    {
        ActiveBehavior = activeBehavior ?? new NormalActiveClass();
        AudioBehavior = audioBehavior ?? new NormalAudioClass();
        MoveBehavior = moveBehavior ?? new MovewithTranslate();
    }
    #endregion

    #region Abstract Base Class Features
    public abstract void Init();
    public void SetPosition(Vector3 initPosition)
    {
        go.transform.position = initPosition;
    }

    public bool OutOfRange(float dist)
    {
        return Vector3.Distance(go.transform.position, _initPosition) > dist;
    }
    #endregion

    #region IActiveBehavior
    public bool IsActive(GameObject go)
    {
        return ActiveBehavior.IsActive(go);
    }

    public void Activate(GameObject go, Vector3 position)
    {
        ActiveBehavior.Activate(go, position);
    }

    public void InActivate(GameObject go, Vector3 position)
    {
        ActiveBehavior.InActivate(go, position);
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