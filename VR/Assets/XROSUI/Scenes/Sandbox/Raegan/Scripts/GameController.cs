using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;
using TMPro;
public class GameController : MonoBehaviour
{
    public TMP_Text RestartText;
    
    void Start()
    {
        RestartText.gameObject.SetActive(false);
    }


    public void Restarts()
    {
        RestartText.gameObject.SetActive(true);
    }
}
