using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void Activate();
    void InActivate();
    bool IsActive();
}
