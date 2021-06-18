using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportNaiveSocket : MonoBehaviour
{
    public UI_ShowNaiveSlotLocation display;
    public ENUM_XROS_PeripersonalEquipmentLocations location;

    private void OnTriggerEnter(Collider other)
    {
        if (display)
        {
            display.ChangeNaiveSlotLocation(location.ToString());    
        }
    }
}
