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
    private const int SoundBulletOp_InitAmount = 4;
    private const int SoundBulletOp_HighWaterMark = 20;

    private ObjectPool<MuteBulletPO> muteBulletOP;
    MuteBulletPO muteBulletPO;
    private const int MuteBulletOp_InitAmount = 0;
    private const int MuteBulletOp_HighWaterMark = 20;

    private ObjectPool<AudioPO> audioOP;
    AudioPO audioPO;
    private const int AudioOp_InitAmount = 1;
    private const int AudioOp_HighWaterMark = 20;

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

        muteBulletPO = new GameObject(name = "muteBulletPO").AddComponent<MuteBulletPO>();
        muteBulletOP = new GameObject(name = "muteBulletPO_OP").AddComponent<MuteBulletPO_OP>();
        
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
        _soundBulletPo.Init(PrimitiveType.Capsule);
        _soundBulletOp.Init(_soundBulletPo, SoundBulletOp_InitAmount, SoundBulletOp_HighWaterMark);

        muteBulletPO.Init(PrimitiveType.Capsule);
        muteBulletOP.Init(muteBulletPO, MuteBulletOp_InitAmount, MuteBulletOp_HighWaterMark);

        audioPO.Init(PrimitiveType.Cube);
        audioOP.Init(audioPO, AudioOp_InitAmount, AudioOp_HighWaterMark);
        
        lastAskTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Get a new object from pool every 3 seconds
        if (Time.time - lastAskTime > 0.1f)
        {
            if (_soundBulletOp._activeNum < 6) {
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
            for (int i = 0; i < _soundBulletOp._activeNum; i++)
            {
                //SoundBulletPO po = _soundBulletOp.GetPooledObject();
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
            for (int i = 0; i < MuteBulletOp_InitAmount; i++)
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
            for (int i = 0; i < AudioOp_InitAmount; i++) 
            {
                audioOP.pooledObjects[i].gestureArea.MeasureDirect();
            }
        }
        
    }
}