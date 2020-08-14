using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
//Ref: https://docs.unity3d.com/ScriptReference/Vector3.Dot.html
/// <summary>
/// Example of UnityEvent
///
/// Usage 
/// </summary>
public class FlipDetection : MonoBehaviour
{
    public UnityEvent EnterFlipPosition;
    public UnityEvent ExitFlipPosition;
    [Range(0, 1)]
    public float Tolerance = 0.25f;
    
    private bool _isFlipped = false;

    public Transform CameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        CameraTransform = Camera.main.transform;
    }

    
    // Update is called once per frame
    void Update()
    {
        var myTransform = transform;
        Vector3 toOther = CameraTransform.position - myTransform.position;        
        //print(toOther + " " + Viewer.forward);
        //print(Vector3.Dot(-this.transform.up, toOther));
        
        if (Vector3.Dot(-myTransform.up, toOther) > (Tolerance))
        {
            if (!_isFlipped)
            {
                this.EnterFlipPosition.Invoke();
                this._isFlipped = true;
                //Debug.Log("flipped");
            }
        }
        else
        {
            if (_isFlipped)
            {
                this.ExitFlipPosition.Invoke();
                this._isFlipped = false;
                //Debug.Log("NNflipped");
            }
        }
    }
}
