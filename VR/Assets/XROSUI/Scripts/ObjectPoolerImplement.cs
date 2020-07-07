using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectPoolerImplement : MonoBehaviour
{
    SoundBulletPO_OP soundBulletOP;
    SoundBulletPO soundBulletPO;

    AudioPO_OP audioOP;
    AudioPO audioPO;
    
    MuteBulletPO_OP muteBulletOP;
    MuteBulletPO muteBulletPO;

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
        soundBulletPO = new GameObject(name = "soundBulletPO").AddComponent<SoundBulletPO>();
        soundBulletOP = new GameObject(name = "soundBulletPO_OP").AddComponent<SoundBulletPO_OP>();

        audioPO = new GameObject(name = "audioPO").AddComponent<AudioPO>();
        audioOP = new GameObject(name = "audioPO_OP").AddComponent<AudioPO_OP>();

        muteBulletPO = new GameObject(name = "muteBulletPO").AddComponent<MuteBulletPO>();
        muteBulletOP = new GameObject(name = "muteBulletPO_OP").AddComponent<MuteBulletPO_OP>();

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
        soundBulletPO.Init();
        soundBulletOP.Init(soundBulletPO, amount);

        muteBulletPO.Init();
        muteBulletOP.Init(muteBulletPO, amount);

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
            if (!soundBulletOP.IsEmpty()) {
                soundBulletOP.GetPooledObject();

                lastAskTime = Time.time;
            }

            if (!audioOP.IsEmpty())
            {
                audioOP.GetPooledObject();
                lastAskTime = Time.time;
            }
        }

        if (Time.time - lastAskTime > 0.3f)
        {
            if (!muteBulletOP.IsEmpty())
            {
                muteBulletOP.GetPooledObject();

                lastAskTime = Time.time;
            }
        }

        if (!soundBulletOP.IsFull())
        {
            for (int i = 0; i < amount; i++)
            {
                //Move active objects
                if (soundBulletOP.pooledObjects[i].IsActive())
                {
                    soundBulletOP.pooledObjects[i].MoveForward(new Vector3(0, 0.2f, 0));
                }

                //Return Object to Pool if beyond range
                if (soundBulletOP.pooledObjects[i].IsActive() && soundBulletOP.pooledObjects[i].OutOfRange(100.0f))
                {
                    soundBulletOP.ReturnPooledObject(soundBulletOP.pooledObjects[i]);
                }
            }
        }

        if (!muteBulletOP.IsFull())
        {
            for (int i = 0; i < amount; i++)
            {
                //Move active objects
                if (muteBulletOP.pooledObjects[i].IsActive())
                {
                    muteBulletOP.pooledObjects[i].MoveForward(new Vector3(0, 0.2f, 0));
                }

                //Return Object to Pool if beyond range
                if (muteBulletOP.pooledObjects[i].IsActive() && muteBulletOP.pooledObjects[i].OutOfRange(100.0f))
                {
                    muteBulletOP.ReturnPooledObject(muteBulletOP.pooledObjects[i]);
                }
            }
        }
        //Debug.Log("SINGLETON UPDATE");
    }
}