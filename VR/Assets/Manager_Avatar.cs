using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public delegate void Delegate_NewAvatar();

public class Manager_Avatar : MonoBehaviour
{
    public static event Delegate_NewAvatar EVENT_NewAvatar;

    //This is like the capslock. If it's true, we always show AMM
    private bool AvatarManagementModeLock = false;
    //This is for tracking AMM and whether it is on or not
    private bool IsInAvatarManagementMode = false;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        
    }

    public void PreviousAlternateAvatar()
    {
        
    }

    public void NextAlternateAvatar()
    {
        
    }

    public void ToggleAvatarManagementModeLock()
    {
        AvatarManagementModeLock = !AvatarManagementModeLock;
    }

    public void ShowAvatarManagementMode(bool ToShow)
    {
        if (AvatarManagementModeLock)
        {
            IsInAvatarManagementMode = AvatarManagementModeLock;
        }
        else
        {
            IsInAvatarManagementMode = ToShow;
        }
    }
}