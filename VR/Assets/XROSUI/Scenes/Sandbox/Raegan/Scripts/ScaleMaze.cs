using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMaze : MonoBehaviour
{
    public GameObject floor;

    public void Start()
    {

        transform.position = new Vector3(floor.transform.position.x-1.5f, floor.transform.position.y+0.1f, transform.position.z-1.5f);     

    }



        public void Update()
        {
            transform.localScale = new Vector3(0.17f, 0.1f,0.2f);
        }

    

}