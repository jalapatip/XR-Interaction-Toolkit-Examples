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
/// </summary>
public class FlipDetection : MonoBehaviour
{
    public UnityEvent EnterFlipPosition;
    public UnityEvent ExitFlipPosition;
    [Range(0, 1)]
    public float Tolerance = 0.25f;
    bool m_IsFlipped = false;

    public float Test;
    public Transform Viewer;

    // Start is called before the first frame update
    void Start()
    {
        Viewer = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toOther = Viewer.position - transform.position;        
        //print(toOther + " " + Viewer.forward);
        //print(Vector3.Dot(-this.transform.up, toOther));
        Test = Vector3.Dot(-this.transform.up, toOther);
        if (Vector3.Dot(-this.transform.up, toOther) > (Tolerance))
        //if (Vector3.Dot(-this.transform.up, toOther) < (Tolerance -1))
        //if (Vector3.Dot(transform.up, -Camera.main.transform.forward) < (Tolerance - 1))
        {
            if (!m_IsFlipped)
            {
                this.EnterFlipPosition.Invoke();
                this.m_IsFlipped = true;
                //Debug.Log("flipped");
            }
        }
        else
        {
            if (m_IsFlipped)
            {
                this.ExitFlipPosition.Invoke();
                this.m_IsFlipped = false;
                //Debug.Log("NNflipped");
            }
        }
    }
}
