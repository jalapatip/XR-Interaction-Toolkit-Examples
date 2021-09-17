using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Experiment2_UI_VisualizeColor : MonoBehaviour
{
    public TMP_Text text;
        
        
    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        foreach (int i in Enum.GetValues(typeof(ENUM_XROS_PeripersonalEquipmentLocations)))
        {
            ENUM_XROS_PeripersonalEquipmentLocations a = (ENUM_XROS_PeripersonalEquipmentLocations)i;
            text.text += "\n<color=#"+ColorUtility.ToHtmlStringRGB(Experiment2_PeripersonalSlotHelper.GetSlotColor(a))+">" + Enum.GetName(typeof(ENUM_XROS_PeripersonalEquipmentLocations), i) + "</color>";      
        }  
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
