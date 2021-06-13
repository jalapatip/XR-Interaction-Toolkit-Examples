using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagOfHolding : MonoBehaviour, IReferenceObject
{
    public GameObject peekWindow;
    
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

    #region IReferenceObject

    public Transform beginTran;
    public Vector3 GetDeltaPosition()
    {
        return transform.position;
    }

    public Quaternion GetDeltaRotation()
    {
        return transform.rotation;
    }
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
        if (rGate == null)
            return;

        var initData = new RemoteGateInitData();
        rGate.Init(initData);
    }
    
    public void Activate()
    {
        //Begin AutoDetach
        detachCountdown.Reset();
        detachCountdown.Run(true);
        
        if (rGate == null)
            return;
        
        rGate.Activate();
        
        peekWindow.SetActive(true);
    }

    public void Deactivate()
    {
        detachCountdown.Run(false);
        
        //hide content
        peekWindow.SetActive(false);
    }
}
