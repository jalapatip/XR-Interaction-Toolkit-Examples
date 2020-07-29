using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    private T pooledObject;
    public List<T> pooledObjects;
    private int _amount;
    public int _activeNum = 0;
    private int _highWaterMark; 
   
    public void Init(T objectToPool, int initSize, int highWaterMark)
    {
        pooledObject = objectToPool;
        pooledObjects = new List<T>();
        _amount = initSize;
        _highWaterMark = highWaterMark;
        for (int i = 0; i < _amount; i++)
        {
            T po = (T)Instantiate(objectToPool);
            po.name = typeof(T).ToString() + "_" + i.ToString();
            po.InActivate();
            pooledObjects.Add(po);
        }
    }

    public T GetPooledObject()
    {
        //TODO add a check to see if this OP is allowed to grow and grow to accomodate.
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
        else 
        {
            if (_amount < _highWaterMark)
            {    
                T po = (T)Instantiate(pooledObject);
                po.name = typeof(T).ToString() + "_" + _amount.ToString();
                po.Activate();
                pooledObjects.Add(po);

                _amount++;
                _activeNum++;

                return po;
            }
            else
            {
                Debug.LogWarning("Reach the maximum number of objects ever used");
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
        return _activeNum == _amount;
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
