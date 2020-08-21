using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class VrUserCredential : VrEquipment
{
    [TooltipAttribute("Assign using inspector")]
    public TMP_Text Text_UserName;

    //credential associated with this GameObject
    public string Credential { get; private set; }

    private void Start()
    {
        Manager_Account.EVENT_NewUser += UpdateUser;
        Text_UserName.text = Core.Ins.Account.GetUserName();
    }

    private void UpdateUser(string newUserName)
    {
        Text_UserName.text = newUserName;
        Credential = newUserName;
    }
    
    protected override void OnActivate(XRBaseInteractor obj)
    {
        Core.Ins.SystemMenu.OpenMenu(XROSMenuTypes.Menu_User);
    }

    protected override void OnDeactivate(XRBaseInteractor obj)
    {
        Core.Ins.SystemMenu.OpenMenu(XROSMenuTypes.Menu_None);
    }
}
