using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    void Init();

    bool IsActive();

    void Activate();

    void InActivate();
}