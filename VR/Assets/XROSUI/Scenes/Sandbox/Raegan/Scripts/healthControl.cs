﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthControl : MonoBehaviour
{
    //public GameObject RestartText;
    //public GameObject TimerText;
   // public GameObject explosion;
    public bool winner;
    void Start()
    {
        winner = false; 
        /*float X, Z;
        X = Random.Range(-3.56f, -5.87f);
        Z = Random.Range(-4.27f, -6.4f);
        transform.position = new Vector3(X, transform.position.y, Z);*/


    }
    public void ActivateHealth()
    {
        this.gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider collide)
    {
        //  GameObject win = Instantiate(explosion) as GameObject;
        // win.transform.position = transform.position;
       
        Destroy(collide.gameObject);
        this.gameObject.SetActive(false);
        winner = true;
       
        
    }
    
    public void Update()
    {
        if(winner == false)
        {
            ActivateHealth();
        }
    }
}


