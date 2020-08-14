using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_UserMenu : MonoBehaviour
{
    public TMP_Text Text_UserName;

    // Start is called before the first frame update
    void Awake()
    {
        Manager_Account.EVENT_NewUser += UpdateUser;
    }
    private void Start()
    {
        Text_UserName.text = Core.Ins.Account.GetUserName();
    }

    public void UpdateUser(string name)
    {
        Text_UserName.text = name;
    }
    // Update is called once per frame
    void Update()
    {
    }
}
