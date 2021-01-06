using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used in the GameObject "Alt Node".
/// When Any Virtual Equipment (VE) GameObject come in contact with Alt Node, it will open the menu associated with that VE
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
        
        //New way of getting a component
        //https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html
        if (other.TryGetComponent(out VE_EquipmentBase vre1))
        {
            Debug.Log(this.name);
            Debug.Log(vre1.name);
            Core.Ins.SystemMenu.OpenMenu(vre1.menuTypes);
        }
    }
}
