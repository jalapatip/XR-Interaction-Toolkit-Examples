using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public delegate void Delegate_NewAvatar();

public class Manager_Avatar : MonoBehaviour
{
    //public static event Delegate_NewAvatar EVENT_NewAvatar;

    public GameObject PF_AlternateAvatar;
    public GameObject PF_MiniAvatar;

    public GameObject MiniAvatarLocation;
    
    private GameObject _newAlternateAvatar;    
    private List<VA_AvatarBase> _avatarList = new List<VA_AvatarBase>();
    private int _currentAvatarId = 0;

    private AudioListener _mainListener;
    
    //This is like the capslock. If it's true, we always show AMM
    private bool AvatarManagementModeLock = false;
    //This is for tracking AMM and whether it is on or not
    private bool IsInAvatarManagementMode = false;
    // Start is called before the first frame update

    private void Awake()
    {
        this.MiniAvatarLocation = GameObject.Find("MiniAvatarLocation");
    }
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

    public void SelectAlternateAvatar(int id)
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
        AddMiniAvatar(ava);
    }

    private void AddMiniAvatar(VA_AvatarBase ava)
    {
        
        var mini = Instantiate(PF_MiniAvatar, Vector3.zero, Quaternion.identity);
        //print(mini.transform.position);
        mini.transform.SetParent(MiniAvatarLocation.transform, false);
        //print(mini.transform.position);
        //mini.transform.position = Vector3.zero;
        var position = mini.transform.position + new Vector3(0.15f*_avatarList.Count, 0, 0);
        mini.transform.position = position;
        var miniScript = mini.GetComponent<VA_AvatarDoll>();
        miniScript.Setup(ava);

        mini.SetActive(ava._deployed);
        
        //miniScript.ShowMiniavatar(false);
        //var go2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //go2.transform.SetParent(MiniAvatarLocation.transform, false);
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

    public GameObject GetNewAlternateAvatar()
    {
//        Dev.Log("New Alternate Avatar");
        
        _newAlternateAvatar = Instantiate(PF_AlternateAvatar, Vector3.zero, Quaternion.identity);
        _newAlternateAvatar.SetActive(false);
        return _newAlternateAvatar;
        //throw new System.NotImplementedException();
    }
}