using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveBehavior
{
    void MoveForward(GameObject go, Vector3 v);
}

public class MovewithTranslate : IMoveBehavior
{
    public void MoveForward(GameObject go, Vector3 v)
    {
        go.transform.Translate(v);
    }
}