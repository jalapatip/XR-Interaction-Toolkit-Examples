using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportNaiveSocket : MonoBehaviour
{
    public UI_ShowNaiveSlotLocation display;
    public ENUM_XROS_PeripersonalEquipmentLocations location;

    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (display)
        {
            display.ChangeNaiveSlotLocation(location.ToString());    
        }
    }
}
