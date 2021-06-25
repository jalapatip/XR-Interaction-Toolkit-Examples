using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_OpenMenu : Switch_Base
{
    public XROSMenuTypes AssociatedMenuType;

    protected override void OnActivated(XRBaseInteractor obj)
    {
        Core.Ins.SystemMenu.OpenMenu(AssociatedMenuType);
    }
}
