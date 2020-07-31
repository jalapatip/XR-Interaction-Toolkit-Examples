using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void Activate();
    void Deactivate();
    bool IsActive();
    GameObject GetGameObject();
    //string GetIdentifier();
}
