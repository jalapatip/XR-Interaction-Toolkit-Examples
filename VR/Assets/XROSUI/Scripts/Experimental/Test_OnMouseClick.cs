using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// OnMouseDown works if Camera's TargetEye is set to None.
/// We can have another camera that follows the XR Camera with higher priority. Enable this camera if we want to debug by clicking.
/// </summary>
public class Test_OnMouseClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("OnMouseUpAsButton");
    }
}