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
        // var vre = other.GetComponent<VE_EquipmentBase>();
        // if (vre)
        // {
        //     Core.Ins.SystemMenu.OpenMenu(vre.menuTypes);
        // }
        
        //New way of getting a component
        //https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html
        // if (other.TryGetComponent(out VE_EquipmentBase vre1))
        // {
        //     Core.Ins.SystemMenu.OpenMenu(vre1.menuTypes);
        // }
        
        
        //Temporary HACK:
        //Given restructure of Gameobjects to:
        //1: GameObject for Game Logic
        //2: GameObject for Model/Collider
        //When OnTrigger hits, it will hit the GameObject storing Collider information and cannot get the script for Game Logic.
        
        if (other.TryGetComponent(out VE_EquipmentBase vre1))
        {
            // Debug.Log(this.name);
            // Debug.Log(vre1.name);
            Core.Ins.SystemMenu.OpenMenu(vre1.menuTypes);
        }
        else
        {
            
            if (other.transform.parent.TryGetComponent(out VE_EquipmentBase vre2))
            {
                // Debug.Log(this.name);
                // Debug.Log(vre2.name);
                //Debug.Log(vre2.menuTypes);
                Core.Ins.SystemMenu.OpenMenu(vre2.menuTypes);
            }
        }
    }
}
