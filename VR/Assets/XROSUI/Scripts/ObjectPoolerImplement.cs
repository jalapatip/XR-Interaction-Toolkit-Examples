using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectPoolerImplement : MonoBehaviour
{
    BulletPO_OP objectPool;
    BulletPO bullet;
    float lastAskTime;
    const int amount = 5;

    #region Singleton Setup
    private static ObjectPoolerImplement ins = null;
    public static ObjectPoolerImplement Ins
    {
        get
        {
            return ins;
        }
    }

    void Awake()
    {
        GameObject bulletPO = new GameObject();
        bulletPO.name = "bulletPO";
        bullet = bulletPO.AddComponent<BulletPO>();

        GameObject bulletPO_OP = new GameObject();
        bulletPO_OP.name = "bulletPO_OP";
        objectPool = bulletPO_OP.AddComponent<BulletPO_OP>();

        // if the singleton hasn't been initialized yet
        if (ins != null && ins != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            ins = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion Singleton Setup

    // Start is called before the first frame update
    void Start()
    {
        bullet.Init();
        objectPool.Init(bullet, amount);
        
        lastAskTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Get a new object from pool every 3 seconds
        if (Time.time - lastAskTime > 0.2f)
        {
            if (!objectPool.IsEmpty()) {
                objectPool.GetPooledObject();
                lastAskTime = Time.time;
            }
        }

        if (!objectPool.IsFull())
        {
            //Move all active objects every frame
            for (int i = 0; i < amount; i++)
            {
                if (objectPool.pooledObjects[i].IsActive())
                {
                    objectPool.pooledObjects[i].MoveForward(0, 0.2f, 0);
                }
            }

            //Return Object to Pool if beyond range
            for (int i = 0; i < amount; i++)
            {
                if (objectPool.pooledObjects[i].IsActive() && objectPool.pooledObjects[i].OutOfRange(100.0f))
                {
                    objectPool.ReturnPooledObject(objectPool.pooledObjects[i]);
                }
            }
        }
        //Debug.Log("SINGLETON UPDATE");
    }

}