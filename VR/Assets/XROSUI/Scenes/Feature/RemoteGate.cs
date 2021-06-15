﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RemoteGate : MonoBehaviour
{
    public Camera camera;

    public RemoteGateInitData initData;
    // Update is called once per frame
    void Update()
    {
        if (initData!=null && initData.ShouldUpdate())
        {
            if(initData.GetRefPosition(out Vector3 newPos))
                transform.position = newPos;
            
            if(initData.GetRefRotation(out Quaternion newRot))
                transform.rotation = newRot;
        }
    }

    public bool Init(RemoteGateInitData initData)
    {

        // Check/Enable camera
        SetupCamera(initData.camPos, initData.camRot);
        //
        
        return true;
    }
    
    public void Activate()
    {
    }
    
    public void Deactivate()
    {
        // Check/Disable camera
        
        // Reset Camera
        SetupCamera(Vector3.zero, Quaternion.identity);
    }

    void SetupCamera(Vector3 pos, Quaternion rot)
    {
        camera.transform.position = pos;
        camera.transform.rotation = rot;
    }
}

