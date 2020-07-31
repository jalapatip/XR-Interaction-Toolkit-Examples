using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;
using UnityEngine.Serialization;

public class XrosObjectPool : MonoBehaviour
{
    private string _identifier;
    private GameObject _gameobjectToPool;
    public List<IPoolable> PooledObjects;
    [SerializeField]
    private int _amount;
    public int _activeNum = 0;
    private int _highWaterMark; 
   
    public void Init(string identifier, GameObject gameobjectToPool, int initSize, int highWaterMark)
    {
        _identifier = identifier;
        _gameobjectToPool = gameobjectToPool;
        PooledObjects = new List<IPoolable>();
        _amount = initSize;
        _highWaterMark = highWaterMark;
        // print(gameobjectToPool.name);
        // print(_amount);
        // print(_highWaterMark);
        for (int i = 0; i < _amount; i++)
        {
            InstantiateNewPoolObject();
        }
    }

    private IPoolable  InstantiateNewPoolObject()
    {
        GameObject go = Instantiate(_gameobjectToPool);
        IPoolable po = go.GetComponent<IPoolable>();
        if (po == null)
        {
            Dev.LogError("GameObject" + go.name + " does not implement IPoolable");
        }
        else
        {
            //go.name = typeof(IPoolable).ToString() + "_" + _amount;
            
            po.Deactivate();
            go.transform.SetParent(this.transform);
            PooledObjects.Add(po);
        }

        return po;
    }
    public IPoolable GetPooledObject()
    {
        if (HasNext())
        {
            foreach (var po in PooledObjects)
            {
                if (!po.IsActive())
                {
                    po.Activate();
                    _activeNum++;
                    return po;
                }
            }
        }
        else 
        {
            if (_amount < _highWaterMark)
            {    
                var po = InstantiateNewPoolObject();
                po.Activate();
                _amount++;
                _activeNum++;

                return po;
            }
            else
            {
                Debug.LogWarning(_identifier + " reach the maximum number of objects ever used. " + _activeNum + " / " + _amount + " / " + _highWaterMark + " " + HasNext());
            }
        }
        return default(IPoolable);
    }    

    public void ReturnPooledObject(GameObject go)
    {
        IPoolable po = go.GetComponent<IPoolable>();
        if (po == null)
        {
            Dev.LogError("GameObject" + go.name + " does not implement IPoolable");
            return;
        }

        if (_activeNum != 0)
        {
            po.Deactivate();
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
        return _activeNum == 0;
    }

    public bool HasNext()
    {
        return _activeNum < _amount;
    }
}
