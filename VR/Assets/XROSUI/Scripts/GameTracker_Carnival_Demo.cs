using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GameTracker_Carnival_Demo : MonoBehaviour
{
    public XRNode inputSource;

    private InputDevice _device;

    private bool bPrimaryButton = false;

    public Canvas menu;

    public int currentTime;

    public int currentPrizes;

    public string mostPlayedGame;
    
    public TextMeshProUGUI prizeAmount;
    
    public TextMeshProUGUI timeAmount;
    
    public TextMeshProUGUI mpg;

    public int tank_Played = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 1400;
        currentPrizes = 0;
        mostPlayedGame = "";
        timeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Current Time: " + currentTime;
        prizeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Prizes Won: " + currentPrizes;
        mpg.GetComponent<TMPro.TextMeshProUGUI>().text = "Most Played Game: " + mostPlayedGame;
        menu.enabled = false;
        getDevice();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = SceneSwapper_Carnival.timePassed;
        currentPrizes = PeripersonalSword_GameLogic.prizes;
        timeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Current Time: " + currentTime;
        prizeAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "Prizes Won: " + currentPrizes;
      
        if (GameStart_PeripersonalSwords.ps_played > tank_Played)
        {
            mostPlayedGame = "Peripersonal Swords";
            mpg.GetComponent<TMPro.TextMeshProUGUI>().text = "Most Played Game: " + mostPlayedGame;
        }
        _device = InputDevices.GetDeviceAtXRNode(inputSource);
        _device.TryGetFeatureValue(CommonUsages.primaryButton, out bPrimaryButton);
        if (bPrimaryButton)
        {
            menu.enabled = true;
        }
        else if (!bPrimaryButton)
        {
            menu.enabled = false;
        }
    }

    void getDevice()
    {
        _device = InputDevices.GetDeviceAtXRNode(inputSource);
    }
}
