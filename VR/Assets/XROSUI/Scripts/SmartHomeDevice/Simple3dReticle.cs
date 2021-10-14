using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple3dReticle : MonoBehaviour
{
    public GameObject reticleGO;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetParent(Camera.main.gameObject.transform, false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 9;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10, layerMask))
        {
            reticleGO.transform.position = hit.point;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
//            Debug.Log("Did Hit");
        }
        else
        {
//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
//            Debug.Log("Did not Hit");
        }
    }
}
