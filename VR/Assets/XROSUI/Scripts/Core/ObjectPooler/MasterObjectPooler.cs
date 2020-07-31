using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterObjectPooler : MonoBehaviour
{
    //public GameObject PF_Pool;
    private Dictionary<string, XrosObjectPool> _poolDictionary;
    
    #region Singleton Setup
    private static MasterObjectPooler _ins = null;
    public static MasterObjectPooler Ins
    {
        get
        {
            return _ins;
        }
    }

    void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (_ins != null && _ins != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _ins = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion Singleton Setup
    
    // Start is called before the first frame update
    void Start()
    {
        _poolDictionary = new Dictionary<string, XrosObjectPool>();
    }

    public void SetupNewPool(GameObject pfgo, string identifier, int pooledAmount, int maxAmount = 0)
    {
        ///Creating new GameObject using code 
        GameObject pool = new GameObject();
        pool.AddComponent<XrosObjectPool>();
        
        ///Alternatively, use Prefab to setup pool
        //GameObject pool = GameObject.Instantiate((PF_Pool));
        
        pool.transform.SetParent(this.transform);
        pool.name = "Pool: " + identifier;
        
        XrosObjectPool xop = pool.GetComponent<XrosObjectPool>();
        xop.Init(identifier, pfgo, pooledAmount, maxAmount);
        _poolDictionary.Add(identifier, xop);
    }

    public GameObject GetPooledObject(string identifier)
    {
        return _poolDictionary[identifier].GetPooledObject().GetGameObject();
    }

    public void ReturnPooledObject(string identifier, GameObject go)
    {
        _poolDictionary[identifier].ReturnPooledObject(go);
    }

    public XrosObjectPool GetObjectPool(string identifier)
    {
        return _poolDictionary[identifier];
    }
}
