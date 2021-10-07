using System.Collections;
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
    // Start is called before the first frame update
    void Start()
    {
       
        scoreAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Highest Score: " + PeripersonalSword_GameLogic.highScore;
        prizeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Prizes won: " + PeripersonalSword_GameLogic.prizes;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneSwapper_Carnival.timePassed < 120)
        {
            startCamera.enabled = true;
            endCamera.enabled = false;
        }
        else if (SceneSwapper_Carnival.timePassed == 120)
        {
            endCamera.enabled = true;
            startCamera.enabled = false;

        }

       /* if (Input.GetKey(KeyCode.T))
        {
            endCamera.enabled = true;
            startCamera.enabled = false;
        }*/
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == exitGame)
        {
            Application.Quit();
        }
        else if (other.gameObject == restartGame)
        {
            SceneSwapper_Carnival.timePassed = 0;
            PeripersonalSword_GameLogic.highScore = 0;
            PeripersonalSword_GameLogic.prizes = 0;
            
        }

    }
    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
