using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Canvas))]
public class Tool_GetWorldSpaceCamera : MonoBehaviour
{
    //Can be Assigned in Inspector or this script will find a failsafe
    public Canvas Canvas;

    // Start is called before the first frame update
    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        //Canvas.worldCamera = GameObject.Find("XRRig_XROS").GetComponent<UnityEngine.XR.Interaction.Toolkit.XRRig>().cameraGameObject.GetComponent<Camera>();
        if (!Canvas)
        {
            Canvas = GetComponent<Canvas>();
        }

        if (!Canvas.worldCamera)
        {
            Canvas.worldCamera = Camera.main;
        }
    }
}
