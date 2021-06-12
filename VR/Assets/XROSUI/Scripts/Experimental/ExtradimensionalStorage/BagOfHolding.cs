using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagOfHolding : MonoBehaviour
{
    #region Properties
    [SerializeField] private GameObject remoteGate;
    public GameObject RemoteGateGO
    {
        get
        {
            if(remoteGate == null)
                Debug.LogError("Interacting with Null RemoteGate");
            
            return remoteGate;
        }
        set
        {
            remoteGate = value;
        }
    }

    public CounterHelper detachCountdown = new CounterHelper("DetachCountdown", 30);
    #endregion

    private void OnDestroy()
    {
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if (detachCountdown.IsReachLimit(false))
        {
            //Do Detach
        }
        else
        {
            detachCountdown.Increment();
        }
    }

    // or interface
    private RemoteGate rGate;
    public void Setup(GameObject remoteGateGO =null)
    {
        if (remoteGateGO != null)
            this.RemoteGateGO = remoteGateGO;
            
        rGate =  RemoteGateGO?.GetComponent<RemoteGate>();
    }

    public bool Activate()
    {
        //Begin AutoDetach
        detachCountdown.Reset();
        detachCountdown.Run(true);

        if (rGate == null)
            return false;

        var initData = new RemoteGateInitData();
        var result = rGate.Activate(initData);
        
        // show content 
        if (result)
            result = false;
        
        return result;
    }

    public void Deactivate()
    {
        detachCountdown.Run(false);
        
        //hide content
        
    }
}
