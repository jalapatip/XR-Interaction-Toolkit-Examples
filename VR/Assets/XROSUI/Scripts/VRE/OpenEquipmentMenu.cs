using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used on the GameObject "Alt Node".
/// When Any VRE GameObject come in contact with Alt Node, it will open the menu associated with that VRE
/// </summary>
public class OpenEquipmentMenu : MonoBehaviour
{
    //OnTriggerEnter is called by Unity when a GameObject with a collider that is set to trigger has something enter the collider
    private void OnTriggerEnter(Collider other)
    {
        //Less efficient if VrEquipment is pre-allocated by compiler
        // var vre = other.GetComponent<VrEquipment>();
        // if (vre)
        // {
        //     Core.Ins.SystemMenu.OpenMenu(vre.menuTypes);
        // }
        
        
        if (other.TryGetComponent(out VrEquipment vre1))
        {
            Core.Ins.SystemMenu.OpenMenu(vre1.menuTypes);
        }
    }
}
