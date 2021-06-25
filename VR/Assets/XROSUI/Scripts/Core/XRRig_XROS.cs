using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// XRRig_Xros is intended for us to expand upon features that we'd normally expect in XRRig.
/// Given that XRRig is written by Unity and prone to change, we want to leave that alone.
/// </summary>
public class XRRig_XROS : MonoBehaviour
{
    //public Camera WorldCamera;

    public GameObject SpawnedObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetSpawnedObjectsGO()
    {
        return SpawnedObjects;
    }
}
