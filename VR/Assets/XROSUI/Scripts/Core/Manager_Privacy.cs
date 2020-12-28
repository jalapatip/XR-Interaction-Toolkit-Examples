using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public delegate void Delegate_NewPrivacy(ENUM_XROS_AnatomyParts e, bool b);

//public delegate void Delegate_NewUser(string name);
public class Manager_Privacy : MonoBehaviour
{
    public static event Delegate_NewPrivacy EVENT_NewPrivacy;

    //public static event Delegate_NewUser EVENT_NewUser;
    Dictionary<ENUM_XROS_AnatomyParts, bool> AnatomyDictionary = new Dictionary<ENUM_XROS_AnatomyParts,bool>();
    
    // Start is called before the first frame update
    private void Start()
    {
        var myEnumMemberCount = Enum.GetNames(typeof(ENUM_XROS_AnatomyParts)).Length;
        var values = Enum.GetValues(typeof(ENUM_XROS_AnatomyParts)).Cast<ENUM_XROS_AnatomyParts>();
        foreach(var e in values)
        {
            AnatomyDictionary.Add(e, true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        DebugUpdate();
    }

    private void DebugUpdate()
    {
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
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Hand);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleAnatomyPart(ENUM_XROS_AnatomyParts.Background);
        }
    }

    public void ToggleAnatomyPart(ENUM_XROS_AnatomyParts anatomyPart)
    {
        bool b = !AnatomyDictionary[anatomyPart];
        AnatomyDictionary[anatomyPart] = b;
        print(anatomyPart.ToString() + " " + b);
        EVENT_NewPrivacy?.Invoke(anatomyPart, b);
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