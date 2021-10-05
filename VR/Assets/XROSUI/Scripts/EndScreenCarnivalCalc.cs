using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndScreenCarnivalCalc : MonoBehaviour
{
    public TextMeshProUGUI prizeAmount;
    public TextMeshProUGUI scoreAmount;
    public TextMeshProUGUI mostPlayedGame;
    // Start is called before the first frame update
    void Start()
    {
        scoreAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Highest Score: " + PeripersonalSword_GameLogic.highScore;
        prizeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Prizes won: " + PeripersonalSword_GameLogic.prizes;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
