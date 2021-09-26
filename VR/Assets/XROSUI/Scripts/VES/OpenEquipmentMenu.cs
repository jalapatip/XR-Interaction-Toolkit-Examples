using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used in the GameObject "Alt Node".
/// When Any Virtual Equipment (VE) GameObject come in contact with Alt Node, it will open the menu associated with that VE
/// </summary>
public class OpenEquipmentMenu : MonoBehaviour
{
    
    public TMP_Text text;
    public Renderer myRenderer;

    public Material inactiveMaterial;
    public Material activeMaterial;
    
    private List<VE_EquipmentBase> EquipList = new List<VE_EquipmentBase>();
    
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
            //Core.Ins.SystemMenu.OpenMenu(vre1.menuTypes);

            if (vre1.IsSelected())
            {
                EquipList.Add(vre1);    
            }
            else
            {
                print(vre1.IsSelected());
            }
        }
        else
        {
            if (other.transform.parent && other.transform.parent.TryGetComponent(out VE_EquipmentBase vre2))
            {
                // Debug.Log(this.name);
                // Debug.Log(vre2.name);
                //Debug.Log(vre2.menuTypes);
                //Core.Ins.SystemMenu.OpenMenu(vre2.menuTypes);

                if (vre2.IsSelected())
                {
                    EquipList.Add(vre2);

                    if (EquipList.Count > 0)
                    {
                        //renderer.material.color = Color.red;
                        myRenderer.material = activeMaterial;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out VE_EquipmentBase vre1))
        {
            // Debug.Log(this.name);
            // Debug.Log(vre1.name);
            //Core.Ins.SystemMenu.OpenMenu(vre1.menuTypes);

            EquipList.Remove(vre1);
        }
        else
        {
            if (other.transform.parent && other.transform.parent.TryGetComponent(out VE_EquipmentBase vre2))
            {
                // Debug.Log(this.name);
                // Debug.Log(vre2.name);
                //Debug.Log(vre2.menuTypes);
                //Core.Ins.SystemMenu.OpenMenu(vre2.menuTypes);

                EquipList.Remove(vre2);

                if (EquipList.Count == 0)
                {
                    //renderer.material.color = new Color(43 / 255f, 34 / 255f, 154 / 255f);
                    myRenderer.material = inactiveMaterial;
                }
            }
        }
    }

    void Update()
    {
        if (EquipList.Count > 0)
        {
            foreach (var e in EquipList)
            {
//                Dev.Log("Is Selected: " + e.IsSelected());
                if (!e.IsSelected())
                {
                    Core.Ins.SystemMenu.OpenMenu(e.menuTypes);
                }
            }
        }
    }
}