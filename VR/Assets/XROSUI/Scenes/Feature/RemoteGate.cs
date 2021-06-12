using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RemoteGate : MonoBehaviour
{
    public Camera camera;

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Activate(RemoteGateInitData initData)
    {

        // Check/Enable camera
        SetupCamera(initData.camPos, initData.camRot);
        //
        
        return true;
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

public class RemoteGateInitData
{
    public Vector3 camPos = Vector3.zero;
    public Quaternion camRot = Quaternion.identity;
}
