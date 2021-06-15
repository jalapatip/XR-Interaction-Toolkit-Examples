using System;
using UnityEngine;

[Serializable]
public class RemoteGateInitData
{
    public IReferenceObject refObject;
    public Vector3 camPos = Vector3.zero;
    public Quaternion camRot = Quaternion.identity;

    public bool ShouldUpdate()
    {
        if (refObject == null)
            return false;

        return true;
    }

    public bool GetRefPosition(out Vector3 newPos)
    {
        newPos = Vector3.zero;
        
        if (refObject == null)
            return false;
        newPos = refObject.GetDeltaPosition();
        
        //TODO: if

        return true;
    }

    public bool GetRefRotation(out Quaternion newRot)
    {
        newRot = Quaternion.identity;
        
        if (refObject == null)
            return false;
        newRot = refObject.GetDeltaRotation();
        
        //TODO: if

        return true;
    }
}