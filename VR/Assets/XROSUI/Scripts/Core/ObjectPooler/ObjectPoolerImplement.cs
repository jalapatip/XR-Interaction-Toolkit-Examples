using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

//ObjectPoolerImplement is the class to showcase how ObjectPooler OP and PooledObject PO works
public class ObjectPoolerImplement : MonoBehaviour
{
    //SoundBulletPO_OP _soundBulletOp;
    private XrosObjectPool _soundBulletPool;

    private const int SoundBulletOp_InitAmount = 4;
    // private const int SoundBulletOp_HighWaterMark = 20;

    XrosObjectPool _muteBulletPool;

    private const int MuteBulletOp_InitAmount = 2;
    private const int MuteBulletOp_HighWaterMark = 20;

    XrosObjectPool _audioPool;

    private const int AudioOp_InitAmount = 1;
    // private const int AudioOp_HighWaterMark = 20;


    private float _lastAskTimeSoundBullet;
    private float _lastAskTimeMuteBullet;
    private float _lastAskTimeAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Create GameObjects for testing MasterObjectPool
        //We can use prefabs instead
        GameObject soundBulletPoGo = new GameObject(name = "soundBulletPO");
        SoundBulletPO soundBulletPo = soundBulletPoGo.AddComponent<SoundBulletPO>();
        soundBulletPo.Init(PrimitiveType.Capsule);
        GameObject muteBulletPoGo = new GameObject(name = "muteBulletPO");
        muteBulletPoGo.AddComponent<MuteBulletPO>().Init(PrimitiveType.Capsule);
        GameObject audioPoGo = new GameObject(name = "audioPO");
        audioPoGo.AddComponent<AudioPO>().Init(PrimitiveType.Cube);

        //Actually using MOO
        MasterObjectPooler.Ins.SetupNewPool(soundBulletPoGo, "SoundBullet", SoundBulletOp_InitAmount);
        MasterObjectPooler.Ins.SetupNewPool(muteBulletPoGo, "MuteBullet", MuteBulletOp_InitAmount,
            MuteBulletOp_HighWaterMark);
        MasterObjectPooler.Ins.SetupNewPool(audioPoGo, "AudioPO", AudioOp_InitAmount);

        //Getting access to individual pools for ease of testing.
        //Note the identifier may be better as an enum
        _soundBulletPool = MasterObjectPooler.Ins.GetObjectPool("SoundBullet");
        _muteBulletPool = MasterObjectPooler.Ins.GetObjectPool("MuteBullet");
        _audioPool = MasterObjectPooler.Ins.GetObjectPool("AudioPO");

        //GameObject go = MasterObjectPool.Ins.GetPooledObject("SoundBullet");

        _lastAskTimeSoundBullet = Time.time;
        _lastAskTimeMuteBullet = Time.time;
        _lastAskTimeAudio = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Test_SoundBullet();
        Test_MuteBullet(); 
        Test_AudioPO();
    }

    private void Test_SoundBullet()
    {
        //Test Creation of PooledObject
        //Get a new object from pool every 0.1 seconds
        if (Time.time - _lastAskTimeSoundBullet > 0.2f)
        {
            _soundBulletPool.GetPooledObject();
            _lastAskTimeSoundBullet = Time.time;
        }

        //Test Modifying and Returning PooledObject
        for (int i = 0; i < _soundBulletPool.PooledObjects.Count; i++)
        {
            SoundBulletPO po = _soundBulletPool.PooledObjects[i].GetGameObject().GetComponent<SoundBulletPO>();

            //Move active objects
            if (po.IsActive())
            {
                po.MoveForward(new Vector3(0, 0.3f, 0));
            }

            //Return Object to Pool if beyond range
            if (po.IsActive() && po.OutOfRange(50.0f))
            {
                _soundBulletPool.ReturnPooledObject(po.GetGameObject());
            }
        }
    }

    private void Test_MuteBullet()
    {
        //Test Creation of PooledObject
        //Get a new object from pool every 0.1 seconds
        if (Time.time - _lastAskTimeMuteBullet > 0.2f)
        {
//            print("Add Mute Bullet");
            _muteBulletPool.GetPooledObject();
            _lastAskTimeMuteBullet = Time.time;
        }


        //Test Modifying and Returning PooledObject
        for (int i = 0; i < _muteBulletPool.PooledObjects.Count; i++)
        {
            MuteBulletPO po = _muteBulletPool.PooledObjects[i].GetGameObject().GetComponent<MuteBulletPO>();
            //Move active objects
            if (po.IsActive())
            {
                po.MoveForward(new Vector3(0, 0.4f, 0));
            }

            //Return Object to Pool if beyond range
            if (po.OutOfRange(50.0f))
            {
                _muteBulletPool.ReturnPooledObject(po.GetGameObject());
            }
        }
    }

    private void Test_AudioPO()
    {
        //Test Creation of PooledObject
        //Get a new object from pool every 0.1 seconds
        if (Time.time - _lastAskTimeAudio > 0.1f)
        {
            if (!_audioPool.IsEmpty())
            {
                _audioPool.GetPooledObject();
                _lastAskTimeAudio = Time.time;
            }
        }

        //Test Modifying and Returning PooledObject
        for (int i = 0; i < _audioPool.PooledObjects.Count; i++)
        {
            _audioPool.PooledObjects[i].GetGameObject().GetComponent<AudioPO>().gestureArea.MeasureDirect();
        }
    }
}