using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_LookAtXRRig : MonoBehaviour
{
    private Transform _xrCameraTransform;
    //Only use additional rotation if you can't easily set the right orientation 
    public Vector3 additionalRotation;

    private bool IsLookingAt = true;
    
    // Start is called before the first frame update
    private void Start()
    {
        _xrCameraTransform = Core.Ins.XRManager.GetXrCamera().transform;
        if (!_xrCameraTransform)
        {
            _xrCameraTransform = Camera.main.transform;    
        }
    }

    public void ToggleLookAtStatus()
    {
        IsLookingAt = !IsLookingAt;
    }
        
    // Update is called once per frame
    private void LateUpdate()
    {
        if (IsLookingAt)
        {
            transform.LookAt(_xrCameraTransform);
            transform.Rotate(this.additionalRotation, Space.Self);
        }
        else
        {
            transform.localRotation = Quaternion.identity;
            //transform.Rotate(this.additionalRotation, Space.Self);
        }
    }
}
