using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Tool_RegisterXrCamera : MonoBehaviour
{
    void Awake()
    {
        Core.Ins.XRManager.RegisterCamera(this.GetComponent<Camera>());
    }
}
