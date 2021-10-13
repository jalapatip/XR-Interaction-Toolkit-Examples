using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class ExitOrRestart_CarnivalDemo : MonoBehaviour
{
    
    public GameObject exitGame;

    public GameObject restartGame;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter(Collider other)
    {

            SceneSwapper_Carnival.timePassed = 1400;
            PeripersonalSword_GameLogic.highScore = 0;
            PeripersonalSword_GameLogic.prizes = 0;
            GameStart_PeripersonalSwords.ps_played = 0;
            ReloadLevel();

    }
    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
