using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Delegate_NewUser(string name);

public class Manager_Account : MonoBehaviour
{
    public static event Delegate_NewUser EVENT_NewUser;
    private string m_UserName = "powenyao";

    public string GetUserName()
    {
        return m_UserName;
    }

    // Start is called before the first frame update
    private void Start()
    {
        ChangeUserName("johnsmith");
    }

    public bool CheckAuthentication(string userName)
    {
        return userName.Equals("powenyao");
    }

    public void ChangeUserName(string newName)
    {
        EVENT_NewUser?.Invoke(newName);
        m_UserName = newName;
        //Dev.Log("User changed to " + s);
        Core.Ins.Messages.Log("User changed to " + newName);
    }

    // Update is called once per frame
    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeUserName("TillyChan");
        }
    }
}