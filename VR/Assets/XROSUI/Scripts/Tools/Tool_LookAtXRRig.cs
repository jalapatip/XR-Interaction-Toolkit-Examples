using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_LookAtXRRig : MonoBehaviour
{
    private Transform _xrCameraTransform;
    //Only use additional rotation if you can't easily set the right orientation 
    public Vector3 additionalRotation;
    // Start is called before the first frame update
    private void Start()
    {
        _xrCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(_xrCameraTransform);
        transform.Rotate(this.additionalRotation, Space.Self);
    }
}
