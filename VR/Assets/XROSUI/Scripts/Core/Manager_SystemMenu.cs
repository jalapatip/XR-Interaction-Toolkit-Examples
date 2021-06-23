using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum XROSMenuTypes { Menu_None, Menu_General, Menu_Screenshot, Menu_Setting, Menu_Audio, Menu_Visual, Menu_User, Menu_Credit, Menu_Privacy }

public class Manager_SystemMenu : MonoBehaviour
{
    //We instantiate our menu gameobjects from the prefab in 
    [TooltipAttribute("Assign using inspector from Project Prefabs")]
    public GameObject PF_SystemMenu;
    
    private GameObject GO_SystemMenu;
    private Controller_SystemMenu SystemMenuAccessor;
    
    private void Awake()
    {
        
    }

    public void LoadModule()
    {        
        if (!SystemMenuAccessor)
        {
            GO_SystemMenu = GameObject.Find("PF_SystemMenu");
            if(!GO_SystemMenu)
            { 
                GO_SystemMenu = GameObject.Instantiate(PF_SystemMenu);
            }
            GO_SystemMenu.gameObject.transform.SetParent(this.transform);
            
            SystemMenuAccessor = GO_SystemMenu.GetComponent<Controller_SystemMenu>();
        }
    }

    public void OpenMenu(XROSMenuTypes menu)
    {
        if (SystemMenuAccessor)
        {
            SystemMenuAccessor.OpenMenu(menu);
        }
        else
        {
            LoadModule();
        }
    }

    public void OpenMenu(string val)
    {
        if (Enum.TryParse(val, true, out XROSMenuTypes currentMenu))
        {
            if (Enum.IsDefined(typeof(XROSMenuTypes), currentMenu) | currentMenu.ToString().Contains(","))
            {
                Console.WriteLine("Converted '{0}' to {1}", val, currentMenu.ToString());

                OpenMenu(currentMenu);
            }
            else
            {
                Dev.LogWarning(val + " is not a value of the enum");
            }
        }
        else
        {
            Dev.LogWarning(val + "is not a member of the enum");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DebugInput();
    }

    private void DebugInput()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            OpenMenu(XROSMenuTypes.Menu_None);
        }
        if (Input.GetKey(KeyCode.F2))
        {
            OpenMenu(XROSMenuTypes.Menu_General);
        }
        if (Input.GetKey(KeyCode.F3))
        {
            OpenMenu(XROSMenuTypes.Menu_Setting);
        }
        if (Input.GetKey(KeyCode.F4))
        {
            OpenMenu(XROSMenuTypes.Menu_Audio);
        }
        if (Input.GetKey(KeyCode.F5))
        {
            OpenMenu(XROSMenuTypes.Menu_Visual);
        }
        if (Input.GetKey(KeyCode.F6))
        {
            OpenMenu(XROSMenuTypes.Menu_User);
        }
        if (Input.GetKey(KeyCode.F7))
        {
            OpenMenu(XROSMenuTypes.Menu_Screenshot);
        }
        if (Input.GetKey(KeyCode.F8))
        {
            OpenMenu(XROSMenuTypes.Menu_Credit);
        }
    }
}
