using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public delegate void Delegate_NewUser(string name);
public class Manager_Template : MonoBehaviour
{
    //public static event Delegate_NewUser EVENT_NewUser;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //EVENT_NewUser?.Invoke(s);
        }
    }
}