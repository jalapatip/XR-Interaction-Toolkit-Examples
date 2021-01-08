using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public delegate void Delegate_NewAvatar();

public class Manager_Avatar : MonoBehaviour
{
    //public static event Delegate_NewAvatar EVENT_NewAvatar;

    private List<VA_AvatarBase> _avatarList = new List<VA_AvatarBase>();
    private int _currentAvatarId = 0;

    private AudioListener _mainListener;
    
    //This is like the capslock. If it's true, we always show AMM
    private bool AvatarManagementModeLock = false;
    //This is for tracking AMM and whether it is on or not
    private bool IsInAvatarManagementMode = false;
    // Start is called before the first frame update
    private void Start()
    {
        _mainListener = Camera.main.GetComponent<AudioListener>();
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
        if (_currentAvatarId > 0)
        {
            _currentAvatarId--;
        }
    }

    public void NextAlternateAvatar()
    {
        if (_currentAvatarId < _avatarList.Count)
        {
            _currentAvatarId++;
        }
    }

    public void RegisterAvatar(VA_AvatarBase ava)
    {
        _avatarList.Add(ava);
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

    //not used atm
    public void RegisterMainListener(AudioListener l)
    {
        _mainListener = l;
    }
    public void DisableMainListener(bool b)
    {
        _mainListener.enabled = !b;
    }
}