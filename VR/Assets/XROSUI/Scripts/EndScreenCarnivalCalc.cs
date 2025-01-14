﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
public class EndScreenCarnivalCalc : MonoBehaviour
{
    private XRDirectInteractor interactor;
    public TextMeshProUGUI prizeAmount;
    public TextMeshProUGUI scoreAmount;
    public TextMeshProUGUI mostPlayedGame;

    public Camera startCamera;

    public Camera endCamera;

    public GameObject exitGame;

    public GameObject restartGame;

    public GameObject trophy;
    
    // Start is called before the first frame update
    void Start()
    {
       
        scoreAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Highest Score: " + PeripersonalSword_GameLogic.highScore;
        prizeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Prizes won: " + PeripersonalSword_GameLogic.prizes;
        if (PeripersonalSword_GameLogic.prizes > 0)
        {
            for (int i = 0; i < PeripersonalSword_GameLogic.prizes; i++)
            {
                Instantiate(trophy);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
   
        if (SceneSwapper_Carnival.timePassed < 1700)
        {
            startCamera.enabled = true;
            endCamera.enabled = false;
        }
        else if (SceneSwapper_Carnival.timePassed == 1700)
        {
            endCamera.enabled = true;
            startCamera.enabled = false;

        }

       if (Input.GetKey(KeyCode.T))
       {
           SceneSwapper_Carnival.timePassed = 1700;
       }

       if (Input.GetKey(KeyCode.Q))
       {
           SceneSwapper_Carnival.timePassed = 0;
           PeripersonalSword_GameLogic.highScore = 0;
           PeripersonalSword_GameLogic.prizes = 0;
           GameStart_PeripersonalSwords.ps_played = 0;
           ReloadLevel();
       }
    }

    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
