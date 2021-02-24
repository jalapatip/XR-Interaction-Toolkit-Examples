using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RegisterXRCamera : MonoBehaviour
{
    void Awake()
    {
        Core.Ins.XRManager.RegisterCamera(this.GetComponent<Camera>());
    }
}
