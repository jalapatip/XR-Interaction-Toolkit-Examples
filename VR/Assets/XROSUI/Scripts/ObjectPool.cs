using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<AudioPO> pooledObjects;
    int _amount;
    int _activeNum = 0;
    //Vector3 _initPosition = new Vector3(-3, 2, 0);

    void Awake()
    {
        pooledObjects = new List<AudioPO>();
        ObjectPoolerImplement.Ins.RegisterObjectPool(this);
    }

    void Start()
    {
        
    }

    public void SetAmount(int amount)
    {
        _amount = amount;
    }

    public void Init(AudioPO objectToPool)
    {
        for (int i = 0; i < _amount; i++)
        {
            AudioPO po = (AudioPO)Instantiate(objectToPool);
            po.InActivate();
            pooledObjects.Add(po);
        }
    }

    public AudioPO GetPooledObject()
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
        return null;
    }    


    public void ReturnPooledObject(AudioPO po)
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
