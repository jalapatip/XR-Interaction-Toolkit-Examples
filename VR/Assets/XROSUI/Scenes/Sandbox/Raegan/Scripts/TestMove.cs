using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class TestMove : MonoBehaviour
{
    // Start is called before the first frame update
    private MazeGenerator mazeGen;
    private Rigidbody rig;
    public bool Triggered;
    public void Start()
    {
    
        CharacterController tank = GetComponent<CharacterController>();
        rig = GetComponent<Rigidbody>();

        
    }

    public float speed = 0.5f;
    public Vector3 pos;
    public void StartPosition(Vector3 initial)
    {
        Debug.Log(initial);


    }
    public bool isLeft;
    public bool isRight;
 
    // Update is called once per frame
    void Update()
    {
        

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
      
        Vector3 position =  -(new Vector3(horizontal, 0f, vertical)).normalized;

        
            Quaternion rotates = Quaternion.LookRotation(position);
            rotates = Quaternion.RotateTowards(transform.rotation, rotates, 360 * Time.deltaTime);
            rig.MoveRotation(rotates);
            this.transform.Translate(position * speed * Time.deltaTime, Space.World);
      
    }
   
}
