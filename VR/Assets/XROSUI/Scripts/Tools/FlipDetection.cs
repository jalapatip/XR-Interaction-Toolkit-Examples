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
/// When the GameObject with the script is flipped over, use Unity Events to handle flipped logic.
/// To change the Unity Events, go to the script in Inspector. 
/// </summary>
public class FlipDetection : MonoBehaviour
{
    public UnityEvent EnterFlipPosition;
    public UnityEvent ExitFlipPosition;
    
    [Range(0, 1)]
    public float Tolerance = 0.25f;
    
    private bool _isFlipped = false;

    private Transform _cameraTransform;

    // Start is called before the first frame update
    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        UpdateFlipDetection();
    }

    
    // Update is called once per frame
    private void Update()
    {
        UpdateFlipDetection();
    }

    private void UpdateFlipDetection()
    {
        //check to see if the GameObject is considered flipped
        var myTransform = transform;
        var toOther = _cameraTransform.position - myTransform.position;        
        
        if (Vector3.Dot(-myTransform.up, toOther) > (Tolerance))
        {
            if (!_isFlipped)
            {
                EnterFlipPosition.Invoke();
                _isFlipped = true;
            }
        }
        else
        {
            if (_isFlipped)
            {
                ExitFlipPosition.Invoke();
                _isFlipped = false;
            }
        }   
    }
}
