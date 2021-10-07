using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;
using TMPro;
public class CountdownController : MonoBehaviour
{
    public int startTimer;
    public TMP_Text startDisplay, timerDisplay;
    
    public GameObject dis,dis2, Instructions;
    public int countdownTimer;
    public GameObject tank;
    public GameObject explosion;
    private Vector3 initialPos;
    BombController bombController;

   // private BombController bombController;
    
    void Start()
    {
        dis.SetActive(false);
        dis2.SetActive(false);
        Instructions.SetActive(false);
      
        timerDisplay.gameObject.SetActive(false);
        startDisplay.gameObject.SetActive(false);
        
       
    }

    void Update()
    {
        //bool trig = GetComponent<BombController>().Restarts().isTriggered;


        if (Input.GetKeyDown("space"))
        {
            
            GameStart();

        
        }
        /* if (trig == true)
        {
            Debug.Log("trig");
        }
        */

    }
    public void GameStart()
    {
        
        StartCoroutine(StartCountdown());
        
    }
    IEnumerator StartCountdown()
    {
        Instructions.SetActive(true);
        yield return new WaitForSeconds(3f);
        Instructions.SetActive(false);
        dis.SetActive(true);
        startDisplay.gameObject.SetActive(true);
        while (startTimer > 0)
        {
            startDisplay.text = startTimer.ToString();

            yield return new WaitForSeconds(1f);

            startTimer--;

        }

        startDisplay.text = "GO!";
        yield return new WaitForSeconds(1f);
        startDisplay.gameObject.SetActive(false);
        timerDisplay.gameObject.SetActive(true);
        dis2.SetActive(true);
       
        while (countdownTimer > 0)
        {
            
              timerDisplay.text = countdownTimer.ToString();
              yield return new WaitForSeconds(1f);
              countdownTimer--;
            

        }



        Explode(tank.transform.position);
        yield return new WaitForSeconds(1f);
        timerDisplay.gameObject.SetActive(false);
        StopCoroutine(StartCountdown());
       








    }

   
    void Explode(Vector3 pos)
    {
        Instantiate(explosion, pos, Quaternion.identity);
        Destruction(tank);

    }
    void Destruction(GameObject destroyed)
    {


        Destroy(destroyed);
    }


}