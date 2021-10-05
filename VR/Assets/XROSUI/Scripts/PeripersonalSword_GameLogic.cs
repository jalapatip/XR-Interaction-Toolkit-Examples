using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PeripersonalSword_GameLogic : MonoBehaviour
{
    public static int prizes;
    public GameObject projSpawner;
    public static int highScore;
    public string endScene;
    public GameObject exitCube;
    public AudioClip selectAudio;
    public ParticleSystem confetti;
    public GameObject restart;
    public GameObject deathWall;
    public static int LifeTotal = 15;
    public static bool GameOver = false;
    private float timerCheck = 0.0f;
    public TextMeshProUGUI lifeAmount;
    public TextMeshProUGUI scoreAmount;
    // Start is called before the first frame update
    void Start()
    {
        //projSpawner.SetActive(false);
        confetti.Stop();
    
        restart.SetActive(false);
        exitCube.SetActive(false);
        lifeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = LifeTotal.ToString();
       // scoreAmount.GetComponent<TMPro.TextMeshProUGUI>().text += " " + score;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneSwapper_Carnival.reset)
        {
            LifeTotal = 15;
            Projectile.score = 0;
            exitCube.SetActive(false);
            restart.SetActive(false);
            SceneSwapper_Carnival.reset = false;
        }
        lifeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = LifeTotal.ToString();
        scoreAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + Projectile.score;
        timerCheck += Time.deltaTime;
        if (SceneSwapper_Carnival.timePassed == 120)
        {
            SceneManager.LoadScene(endScene);
        }

        if (Restart_PeripersonalSwords.restartGame)
        {
            exitCube.SetActive(false);
            Restart_PeripersonalSwords.restartGame = false;
        }

      
        
    }
    public void updateTimeAndPrize()
    {
        prizes += 1;
        SceneSwapper_Carnival.timePassed += 30;
        GameOver = false;
    }
  
    public void OnTriggerEnter(Collider other)
    {
        if (LifeTotal > 0)
        {
               
            if (other.gameObject.TryGetComponent(out Projectile p))
            {
                
                LifeTotal -= 1;  
                p.destroySelf();
            }
        }
      
        if (LifeTotal <= 0)
        {
            SceneSwapper_Carnival.timePassed += 30;
            if (highScore < Projectile.score)
            {
                highScore = Projectile.score;
            }

            if (restart == null)
            {
                Restart_PeripersonalSwords.newReStart.SetActive(true);     
            }

            if (restart != null)
            {
                restart.SetActive(true);
            }
            
            if (Projectile.score >= 15)
            {
                prizes += 1;
                Core.Ins.AudioManager.PlayAudio(selectAudio, ENUM_Audio_Type.Sfx);
                confetti.Play();
            }
            
            exitCube.SetActive(true);
            GameOver = true;
           // GameStart.life.SetActive(false);
           // GameStart.score.SetActive(false);
            
        }
        
    }
}
