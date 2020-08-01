using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Delegate_NewUser(string name);

public class Manager_Account : MonoBehaviour
{
    public static event Delegate_NewUser EVENT_NewUser;
    private string m_UserName = "powenyao";

    public string UserName()
    {
        return m_UserName;
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeUserName("johnsmith");   
    }

    public bool CheckAuthentication(string userName)
    {
        return userName.Equals("powenyao");
    }
    public void ChangeUserName(string s)
    {
        EVENT_NewUser?.Invoke(s);
        m_UserName = s;
        //Dev.Log("User changed to " + s);
        Core.Ins.Messages.Log("User changed to " + s);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeUserName("TillyChan");
        }
    }
}
