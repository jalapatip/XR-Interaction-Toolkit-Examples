using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectPoolerImplement : MonoBehaviour
{
    ObjectPool objectPool;
    AudioPO testCube;
    float lastAskTime;
    AudioPO pooledObject;
    int amount = 5;

    #region Singleton Setup
    private static ObjectPoolerImplement ins = null;
    public static ObjectPoolerImplement Ins
    {
        get
        {
            return ins;
        }
    }

    private void Awake()
    {
        //Debug.Log("Implement Awake");
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
        testCube.Init();
        objectPool.SetAmount(amount);
        objectPool.Init(testCube);
        lastAskTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Get a new object from pool every 3 seconds
        if (Time.time - lastAskTime > 3.0f)
        {
            if (!objectPool.IsEmpty()) {
                pooledObject = objectPool.GetPooledObject();
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
                    objectPool.pooledObjects[i].MoveForward(0.02f, 0.02f, 0.02f);
                }
            }

            //Return Object to Pool if beyond range
            foreach (AudioPO po in objectPool.pooledObjects)
            {
                if (po.IsActive() && po.OutOfRange(50.0f))
                {
                    objectPool.ReturnPooledObject(po);
                }
            }
        }
        //Debug.Log("SINGLETON UPDATE");
    }
    public void RegisterObjectPool(ObjectPool op)
    {
        objectPool = op;
    }

    public void RegisterAudioPO(AudioPO po)
    {
        testCube = po;
    }
}