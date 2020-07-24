using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//ObjectPoolerImplement is the class to showcase how ObjectPooler OP and PooledObject PO works
public class ObjectPoolerImplement : MonoBehaviour
{
    //SoundBulletPO_OP _soundBulletOp;
    private ObjectPool<SoundBulletPO> _soundBulletOp;
    SoundBulletPO _soundBulletPo;
    private const int SoundBulletOpAmount = 5;

    private ObjectPool<MuteBulletPO> muteBulletOP;
    MuteBulletPO muteBulletPO;
    private const int MuteBulletOpAmount = 5;

    private ObjectPool<AudioPO> audioOP;
    AudioPO audioPO;
    private const int AudioOpAmount = 1;

    float lastAskTime;

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
        _soundBulletPo = new GameObject(name = "soundBulletPO").AddComponent<SoundBulletPO>();
        _soundBulletOp = new GameObject(name = "soundBulletPO_OP").AddComponent<SoundBulletPO_OP>();

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
        _soundBulletPo.Init();
        _soundBulletOp.Init(_soundBulletPo, SoundBulletOpAmount);

        muteBulletPO.Init();
        muteBulletOP.Init(muteBulletPO, MuteBulletOpAmount);

        audioPO.Init();
        audioOP.Init(audioPO, AudioOpAmount);
        
        lastAskTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Get a new object from pool every 3 seconds
        if (Time.time - lastAskTime > 0.2f)
        {
            if (!_soundBulletOp.IsEmpty()) {
                _soundBulletOp.GetPooledObject();
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

        if (!_soundBulletOp.IsFull())
        {
            for (int i = 0; i < SoundBulletOpAmount; i++)
            {
                //Move active objects
                if (_soundBulletOp.pooledObjects[i].IsActive())
                {
                    _soundBulletOp.pooledObjects[i].MoveForward(new Vector3(0, 0.2f, 0));
                }

                //Return Object to Pool if beyond range
                if (_soundBulletOp.pooledObjects[i].IsActive() && _soundBulletOp.pooledObjects[i].OutOfRange(100.0f))
                {
                    _soundBulletOp.ReturnPooledObject(_soundBulletOp.pooledObjects[i]);
                }
            }
        }

        if (!muteBulletOP.IsFull())
        {
            for (int i = 0; i < MuteBulletOpAmount; i++)
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

        if (!audioOP.IsFull())
        {
            for (int i = 0; i < AudioOpAmount; i++) 
            {
                audioOP.pooledObjects[i].gestureArea.MeasureDirect();
            }
        }
    }
}