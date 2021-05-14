using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectPoolerImplement : MonoBehaviour
{

    public ObjectPool objectPool;
    private GameObject testCube;

    private float lastAskTime;
    GameObject pooledObject;

    //public List<GameObject> pooledObjects = new List<GameObject>();

    
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
        testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        objectPool.SetAmount(5);
        objectPool.Init(testCube);

        lastAskTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastAskTime > 3)
        {
            int objectID = 0;
            if (!objectPool.IsEmpty()) {
                pooledObject = objectPool.GetPooledObject(ref objectID);
                //pooledObject.GetComponent<XRGrabInteractable>();
                Debug.Log("Get " + objectID + "th pooled object");
                lastAskTime = Time.time;
            }
        }

        objectPool.MoveForward(0.02f, 0.02f, 0.02f);

        if (!objectPool.IsFull())
        {
            foreach (GameObject obj in objectPool.pooledObjects)
            {
                if (obj.activeInHierarchy && Vector3.Distance(obj.transform.position, new Vector3(0, 2, 0)) > 50)
                {
                    objectPool.ReturnPooledObject(obj);
                }
            }
        }
        //Debug.Log("SINGLETON UPDATE");
    }

    public void RegisterObjectPool(ObjectPool op)
    {
        Debug.Log("Register Object Pool");
        objectPool = op;
    }
}