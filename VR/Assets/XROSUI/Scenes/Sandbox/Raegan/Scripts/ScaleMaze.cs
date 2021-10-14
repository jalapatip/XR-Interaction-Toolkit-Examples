using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMaze : MonoBehaviour
{
    public float X, Y, Z, Xpos, Ypos,Zpos;

    public void Start()
    {
        Xpos = -0.5f;
        Ypos =  2.4f;
        Zpos = -2.15f;
        X = 0.1f;
        Y = 0.64f;
        Z = 0.22f;
        transform.localScale = new Vector3(X, Y, Z);
        transform.localPosition = new Vector3(Xpos, Ypos, Zpos);
      

    }
   
       

    public void Update()
    {
        transform.localScale = new Vector3(X, Y, Z);
        transform.localPosition = new Vector3(Xpos,Ypos,Zpos);
    }

}