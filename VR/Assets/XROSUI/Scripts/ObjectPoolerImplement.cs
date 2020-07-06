using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectPoolerImplement : MonoBehaviour
{
    BulletPO_OP bulletOP;
    BulletPO bulletPO;
    AudioPO_OP audioOP;
    AudioPO audioPO;

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
        bulletPO = new GameObject(name = "bulletPO").AddComponent<BulletPO>();
        bulletOP = new GameObject(name = "bulletPO_OP").AddComponent<BulletPO_OP>();

        audioPO = new GameObject(name = "audioPO").AddComponent<AudioPO>();
        audioOP = new GameObject(name = "audioPO_OP").AddComponent<AudioPO_OP>();

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
        bulletPO.Init();
        bulletOP.Init(bulletPO, amount);

        audioPO.Init();
        audioOP.Init(audioPO, amount);
        
        lastAskTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Get a new object from pool every 3 seconds
        if (Time.time - lastAskTime > 0.2f)
        {
            if (!bulletOP.IsEmpty()) {
                bulletOP.GetPooledObject();

                lastAskTime = Time.time;
            }

            if (!audioOP.IsEmpty())
            {
                audioOP.GetPooledObject();
                lastAskTime = Time.time;
            }
        }

        if (!bulletOP.IsFull())
        {
            for (int i = 0; i < amount; i++)
            {
                //Move active objects
                if (bulletOP.pooledObjects[i].IsActive())
                {
                    bulletOP.pooledObjects[i].MoveForward(0, 0.2f, 0);
                }

                //Return Object to Pool if beyond range
                if (bulletOP.pooledObjects[i].IsActive() && bulletOP.pooledObjects[i].OutOfRange(100.0f))
                {
                    bulletOP.ReturnPooledObject(bulletOP.pooledObjects[i]);
                }
            }
        }
        //Debug.Log("SINGLETON UPDATE");
    }

}