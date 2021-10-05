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
    public TMP_Text startDisplay;
    public TMP_Text timerDisplay;
    public int countdownTimer;
    public GameObject tank;
    public GameObject explosion;
    private Vector3 initialPos;
    public TMP_Text RestartText;
    public void Start()
    {

         StartCoroutine(StartCountdown());
     
    }
    IEnumerator StartCountdown()
    {
        
        
        while (startTimer > 0)
        {
            startDisplay.text = startTimer.ToString();

            yield return new WaitForSeconds(1f);

            startTimer--;

        }

        startDisplay.text = "GO!";
        yield return new WaitForSeconds(1f);
        startDisplay.gameObject.SetActive(false);

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
        //Restarts();








    }

    public void Restarts()
    {

        RestartText.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        { 
           
            tank.transform.position = initialPos;
            StartCoroutine(StartCountdown());
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
           
        }
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