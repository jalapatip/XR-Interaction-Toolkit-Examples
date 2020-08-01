using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used on Alt Node so that any VRE in contact with Alt Node can open the menu associated with that VRE
/// </summary>
public class OpenEquipmentMenu : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var vre = other.GetComponent<VREquipment>();
        if (vre)
        {
            Core.Ins.SystemMenu.OpenMenu(vre.menuTypes);
        }
    }
}
