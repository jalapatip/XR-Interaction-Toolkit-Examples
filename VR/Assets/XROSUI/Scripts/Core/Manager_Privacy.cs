using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public delegate void Delegate_NewPrivacy(ENUM_XROS_AnatomyParts e, bool b, ENUM_XROS_PrivacyObserver o);

public delegate void Delegate_NewPrivacyObserver(ENUM_XROS_PrivacyObserver p, bool b);

public delegate void Delegate_NewActivatePrivacyObserver(ENUM_XROS_PrivacyObserver p, bool b);

//public enum 
//public delegate void Delegate_NewUser(string name);
public class Manager_Privacy : MonoBehaviour
{
    public static event Delegate_NewPrivacy EVENT_NewPrivacy;
    public static event Delegate_NewPrivacyObserver EVENT_NewPrivacyObserver;
    public static event Delegate_NewActivatePrivacyObserver EVENT_NewActivatePrivacyObserver;
    private bool _IsVoodooDollDeployed = false;

    private ENUM_XROS_PrivacyObserver _currentPrivacyObserver = ENUM_XROS_PrivacyObserver.Master;

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_Current;
    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_Master = new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_Multiplayer =
        new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_DataCollection =
        new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_System = new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_Deactivated =
        new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_MasterCOPY =
        new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_MultiplayerCOPY =
        new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_DataCollectionCOPY =
        new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary_SystemCOPY =
        new Dictionary<ENUM_XROS_AnatomyParts, bool>();

    public GameObject PF_VoodooBase;
    private GameObject _voodooBase;

    // Start is called before the first frame update
    private void Start()
    {
        var myEnumMemberCount = Enum.GetNames(typeof(ENUM_XROS_AnatomyParts)).Length;
        var anatomyEnumValues = Enum.GetValues(typeof(ENUM_XROS_AnatomyParts)).Cast<ENUM_XROS_AnatomyParts>();
        foreach (var e in anatomyEnumValues)
        {
            AnatomyDictionary_Master.Add(e, true);
            AnatomyDictionary_Multiplayer.Add(e, true);
            AnatomyDictionary_DataCollection.Add(e, false);
            AnatomyDictionary_System.Add(e, true);
            AnatomyDictionary_Deactivated.Add(e, false);
        }

        SwitchPrivacyObserver(_currentPrivacyObserver);
        //UpdateDictionary(CurrentPrivacyObserver);
    }

    private void UpdateDictionary(ENUM_XROS_PrivacyObserver observer)
    {
        switch (observer)
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

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ActivatePrivacyObserver(ENUM_XROS_PrivacyObserver.System, true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ActivatePrivacyObserver(ENUM_XROS_PrivacyObserver.System, false);
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
        EVENT_NewPrivacy?.Invoke(anatomyPart, b, _currentPrivacyObserver);
    }

    public void ToggleAnatomyPart(ENUM_XROS_AnatomyParts anatomyPart, bool b)
    {
        AnatomyDictionary_Current[anatomyPart] = b;
//        print(anatomyPart.ToString() + " " + b);
        EVENT_NewPrivacy?.Invoke(anatomyPart, b, _currentPrivacyObserver);
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
            EVENT_NewPrivacy?.Invoke(anatomy, b, _currentPrivacyObserver);
        }
    }

    public void SwitchPrivacyObserver(ENUM_XROS_PrivacyObserver observer)
    {
//        print("New Observer Type: " +observer);
        this._currentPrivacyObserver = observer;
        EVENT_NewPrivacyObserver?.Invoke(observer, true);

        UpdateDictionary(_currentPrivacyObserver);

        foreach (var entry in AnatomyDictionary_Current)
        {
            // do something with entry.Value or entry.Key
            EVENT_NewPrivacy?.Invoke(entry.Key, entry.Value, _currentPrivacyObserver);
        }
    }

    public void ActivatePrivacyObserver(ENUM_XROS_PrivacyObserver observerType, bool active)
    {
        switch (observerType)
        {
            case ENUM_XROS_PrivacyObserver.Master:
                Helper(ref AnatomyDictionary_Master, ref AnatomyDictionary_MasterCOPY, active);
                break;
            case ENUM_XROS_PrivacyObserver.Multiplayer:
                Helper(ref AnatomyDictionary_Multiplayer, ref AnatomyDictionary_MultiplayerCOPY, active);
                break;
            case ENUM_XROS_PrivacyObserver.DataCollection:
                Helper(ref AnatomyDictionary_DataCollection, ref AnatomyDictionary_DataCollectionCOPY, active);
                break;
            case ENUM_XROS_PrivacyObserver.System:
                Helper(ref AnatomyDictionary_System, ref AnatomyDictionary_SystemCOPY, active);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
//         
//         Dictionary<ENUM_XROS_AnatomyParts, bool> d1 = new Dictionary<ENUM_XROS_AnatomyParts, bool>();
//         Dictionary<ENUM_XROS_AnatomyParts, bool> d2 = new Dictionary<ENUM_XROS_AnatomyParts, bool>();
//
//         switch (observerType)
//         {
//             case ENUM_XROS_PrivacyObserver.Master:
//                 d1 = AnatomyDictionary_Master;
//                 d2 = AnatomyDictionary_MasterCOPY;
//                 //Helper(AnatomyDictionary_Master, AnatomyDictionary_MasterCOPY, active);
//                 break;
//             case ENUM_XROS_PrivacyObserver.Multiplayer:
//                 d1 = AnatomyDictionary_Multiplayer;
//                 d2 = AnatomyDictionary_MultiplayerCOPY;
//                 //Helper(AnatomyDictionary_Multiplayer, AnatomyDictionary_MultiplayerCOPY, active);
//                 break;
//             case ENUM_XROS_PrivacyObserver.DataCollection:
//                 d1 = AnatomyDictionary_DataCollection;
//                 d2 = AnatomyDictionary_DataCollectionCOPY;
//                 //Helper(AnatomyDictionary_DataCollection, AnatomyDictionary_DataCollectionCOPY, active);
//                 break;
//             case ENUM_XROS_PrivacyObserver.System:
//                 d1 = AnatomyDictionary_System;
//                 d2 = AnatomyDictionary_SystemCOPY;
//                 //AnatomyDictionary_Current = AnatomyDictionary_System;
//                 break;
//             default:
//                 throw new ArgumentOutOfRangeException();
//         }
//
//         if (active)
//         {
//             //print("activate " + d1.Count + " vs " + d2.Count);
//             //AnatomyDictionary_DataCollection = AnatomyDictionary_DataCollectionCOPY;
//             d1 = d2;
//         }
//         else
//         {
//             //print("deactivate " + d1.Count + " vs " + d2.Count);
//             d2 = d1;
//             d1 = AnatomyDictionary_Deactivated;
//             //AnatomyDictionary_DataCollectionCOPY = AnatomyDictionary_DataCollection;
//             //AnatomyDictionary_DataCollection = AnatomyDictionary_Deactivated;
//         }
//
//         switch (observerType)
//         {
//             case ENUM_XROS_PrivacyObserver.Master:
//                 AnatomyDictionary_Master = d1;
//                 AnatomyDictionary_MasterCOPY = d2;
//                 break;
//             case ENUM_XROS_PrivacyObserver.Multiplayer:
//                 AnatomyDictionary_Multiplayer = d1;
//                 AnatomyDictionary_MultiplayerCOPY = d2;
//                 break;
//             case ENUM_XROS_PrivacyObserver.DataCollection:
//                 AnatomyDictionary_DataCollection = d1;
//                 AnatomyDictionary_DataCollectionCOPY = d2;
//                 break;
//             case ENUM_XROS_PrivacyObserver.System:
//                 AnatomyDictionary_System = d1;
//                 AnatomyDictionary_SystemCOPY = d2;
//                 break;
//             default:
//                 throw new ArgumentOutOfRangeException();
//         }
//
// //        print("current " + _currentPrivacyObserver);
//         SwitchPrivacyObserver(_currentPrivacyObserver);
    }

    private void Helper(ref Dictionary<ENUM_XROS_AnatomyParts, bool> d1, ref Dictionary<ENUM_XROS_AnatomyParts, bool> d2,
        bool active)
    {
        if (active)
        {
//            print("activate");
            d1 = d2;
        }
        else
        {
//            print("deactivate");
            d2 = d1;
            d1 = AnatomyDictionary_Deactivated;
        }

//        print("current " + _currentPrivacyObserver);
        SwitchPrivacyObserver(_currentPrivacyObserver);
    }

    public ENUM_XROS_PrivacyObserver GetCurrentPrivacyObserver()
    {
        return _currentPrivacyObserver;
    }
}


public class User_Privacy_Setting_Data
{
    public bool ShowEye = true;
    public bool ShowHead = true;
    public bool ShowHand = true;
    public bool ShowHeart = true;
}
