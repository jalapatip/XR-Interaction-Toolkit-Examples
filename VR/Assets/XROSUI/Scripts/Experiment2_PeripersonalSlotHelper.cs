using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment2_PeripersonalSlotHelper : MonoBehaviour
{
    public static Color GetSlotColor(ENUM_XROS_PeripersonalEquipmentLocations slot)
    {
        Color c = Color.white; 
        switch (slot)
        {
            case ENUM_XROS_PeripersonalEquipmentLocations.None:
                c = Color.white;
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._0800:
                c = Color.red;
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._0900:
                //c = Color.orange;
                c = new Color(255f, 165f, 0f);
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._1000:
                c = Color.yellow;
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._1100:
                c = Color.green;
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._1200:
                c = Color.blue;
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._0100:
                c = Color.cyan;
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._0200:
                //c = Color.indigo;
                c = new Color(75f, 0f, 130f);
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._0300:
                //c = Color.purple;
                c = new Color(128f, 0f, 128f);
                break;
            case ENUM_XROS_PeripersonalEquipmentLocations._0400:
                c = Color.magenta;
                break;
            default:
                c = Color.black;
                break;
        }

        return c;
    }
}
