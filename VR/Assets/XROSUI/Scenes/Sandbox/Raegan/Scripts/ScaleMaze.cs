using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMaze : MonoBehaviour
{
    public float X, Y, Z, Xpos, Ypos,Zpos;

    public void Start()
    {
        Xpos = -6.1f;
        Ypos = 2.4f;
        Zpos = -6.5f;
        X = 0.26f;
        Y = 0.4f;
        Z = 0.23f;
    }


    public void Update()
    {
        transform.localScale = new Vector3(X, Y, Z);
        transform.localPosition = new Vector3(Xpos,Ypos,Zpos);
    }

}