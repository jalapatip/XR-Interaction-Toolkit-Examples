using System.Collections;
using UnityEngine;

public interface IReferenceObject
{
    Vector3 GetDeltaPosition();
    Quaternion GetDeltaRotation();
}
