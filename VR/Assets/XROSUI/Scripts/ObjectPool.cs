using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IPooledObject
{
    public List<T> pooledObjects;
    int _amount;
    int _activeNum = 0;

    void Awake()
    {
        pooledObjects = new List<T>();
    }


    public void Init(T objectToPool, int amount)
    {
        _amount = amount;
        for (int i = 0; i < _amount; i++)
        {
            T po = (T)Instantiate(objectToPool);
            po.name = "test" + i.ToString();
            po.InActivate();
            pooledObjects.Add(po);
        }
    }

    public T GetPooledObject()
    {
        if (!IsEmpty())
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].IsActive())
                {
                    pooledObjects[i].Activate();
                    _activeNum++;
                    return pooledObjects[i];
                }
            }
        }
        return default(T);
    }    


    public void ReturnPooledObject(T po)
    {
        if (_activeNum != 0)
        {
            po.InActivate();
            _activeNum--;
        }
        else
        {
            Debug.Log("Pool is full. Cannot return object");
        }
    }

    public bool IsEmpty()
    {
        if (_activeNum == _amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsFull()
    {
        if (_activeNum == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
