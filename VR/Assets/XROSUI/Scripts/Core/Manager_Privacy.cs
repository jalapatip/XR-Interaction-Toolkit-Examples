using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public delegate void Delegate_NewPrivacy(ENUM_XROS_AnatomyParts e, bool b);
public delegate void Delegate_NewPrivacyObserver(ENUM_XROS_PrivacyObserver p, bool b);

//public enum 
//public delegate void Delegate_NewUser(string name);
public class Manager_Privacy : MonoBehaviour
{
    public static event Delegate_NewPrivacy EVENT_NewPrivacy;
    public static event Delegate_NewPrivacyObserver EVENT_NewPrivacyObserver;
    private bool _IsVoodooDollDeployed = false;

    private ENUM_XROS_PrivacyObserver CurrentPrivacyObserver = ENUM_XROS_PrivacyObserver.Master;

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_Current;
    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_Master = new Dictionary<ENUM_XROS_AnatomyParts,bool>();
    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_Multiplayer = new Dictionary<ENUM_XROS_AnatomyParts,bool>();
    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_DataCollection = new Dictionary<ENUM_XROS_AnatomyParts,bool>();
    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_System = new Dictionary<ENUM_XROS_AnatomyParts,bool>();
    
    public GameObject PF_VoodooBase;
    private GameObject _voodooBase;
    
    // Start is called before the first frame update
    private void Start()
    {
        var myEnumMemberCount = Enum.GetNames(typeof(ENUM_XROS_AnatomyParts)).Length;
        var anatomyEnumValues = Enum.GetValues(typeof(ENUM_XROS_AnatomyParts)).Cast<ENUM_XROS_AnatomyParts>();
        foreach(var e in anatomyEnumValues)
        {
            AnatomyDictionary_Master.Add(e, true);
            AnatomyDictionary_Multiplayer.Add(e, true);
            AnatomyDictionary_DataCollection.Add(e, false);
            AnatomyDictionary_System.Add(e, true);
        }
        
        UpdateDictionary(CurrentPrivacyObserver);
    }

    private void UpdateDictionary(ENUM_XROS_PrivacyObserver observer)
    {
        switch(observer)
        {
            case ENUM_XROS_PrivacyObserver.Master:
                AnatomyDictionary_Current = AnatomyDictionary_Master;
                break;
            case ENUM_XROS_PrivacyObserver.Multiplayer:
                AnatomyDictionary_Current = AnatomyDictionary_Multiplayer;
                break;
            case ENUM_XROS_PrivacyObserver.DataCollection:
                AnatomyDictionary_Current = AnatomyDictionary_DataCollection;
                break;
            case ENUM_XROS_PrivacyObserver.System:
                AnatomyDictionary_Current = AnatomyDictionary_System;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchPrivacyObserver(ENUM_XROS_PrivacyObserver.Master);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchPrivacyObserver(ENUM_XROS_PrivacyObserver.Multiplayer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchPrivacyObserver(ENUM_XROS_PrivacyObserver.System);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchPrivacyObserver(ENUM_XROS_PrivacyObserver.DataCollection);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ActivateIncognitoMode(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ActivateIncognitoMode(false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Eyes);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Mouth);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Ear);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Head);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Heart);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Feet);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Hands);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Background);
        }
    }

    public void ToggleAnatomyPart(ENUM_XROS_AnatomyParts anatomyPart)
    {
        bool b = !AnatomyDictionary_Current[anatomyPart];
        AnatomyDictionary_Current[anatomyPart] = b;
//        print(anatomyPart.ToString() + " " + b);
        EVENT_NewPrivacy?.Invoke(anatomyPart, b);
    }
    public void ToggleAnatomyPart(ENUM_XROS_AnatomyParts anatomyPart, bool b)
    {
        AnatomyDictionary_Current[anatomyPart] = b;
//        print(anatomyPart.ToString() + " " + b);
        EVENT_NewPrivacy?.Invoke(anatomyPart, b);
    }
    
    
    public void DeployVoodooDoll(bool b)
    {
        Debug.Log("Deploy Voodoo Doll " + b);
        _IsVoodooDollDeployed = b;
    }
    
    public bool IsVoodooDollDeployed()
    {
        return _IsVoodooDollDeployed;
    }

    public GameObject GetVoodooBase()
    {
    //    print("voodoobase");
        return _voodooBase;
    }

    public void ActivateIncognitoMode(bool p0)
    {
        var b = !p0;
//        Dev.Log("Incognito Mode " + b);
        var myEnumMemberCount = Enum.GetNames(typeof(ENUM_XROS_AnatomyParts)).Length;
        var anatomyEnumValues = Enum.GetValues(typeof(ENUM_XROS_AnatomyParts)).Cast<ENUM_XROS_AnatomyParts>();

        foreach (var anatomy in anatomyEnumValues)
        {
            AnatomyDictionary_Current[anatomy] = b;
            EVENT_NewPrivacy?.Invoke(anatomy, b);    
        }
    }

    public void SwitchPrivacyObserver(ENUM_XROS_PrivacyObserver observer)
    {
//        print("New Observer Type: " +observer);
        this.CurrentPrivacyObserver = observer;
        EVENT_NewPrivacyObserver?.Invoke(observer, true);
        
        UpdateDictionary(CurrentPrivacyObserver);
        
        foreach(var entry in AnatomyDictionary_Current)
        {
            // do something with entry.Value or entry.Key
            EVENT_NewPrivacy?.Invoke(entry.Key, entry.Value);
        }
    }
}



public class User_Privacy_Setting_Data
{
    public bool ShowEye = true;
    public bool ShowHead = true;
    public bool ShowHand = true;
    public bool ShowHeart = true;

}
/*
=Eye=
Main Feature = Enable/Disable User's Eye or fall back to default eyes


Eye Color
Natural
Custom Color

Cornea Reflection
Pupil Size

Gaze Direction

Pupilometry?

=Mouth=
Main Feature = Enable/Disable Audio (Mute)

Enable/Disable Background Audio
Voice Filter

Supress Background Noise
Speaker to use
Test Speaker
Test Mic

=Ear???=
Main Feature = Enable/Disable Incoming Audio(?)

Ear Shape

=Hand=
Main Feature = Enable/Disable Hand Tracking Information
Enable/Disable Hand Tracking

=Other Motion Trackers=

=Virtual Background=
Enable/Disable Real Background
Select Background?



=Heart=
Main Feature = Heart Beat
Alias / Identity ?


=Head/Face=
Main Featuer = Enable/Disable Expression Tracking 

=Camera=
Select Camera
Mirror Video
Touch up my appearance

Display Name above character
*/