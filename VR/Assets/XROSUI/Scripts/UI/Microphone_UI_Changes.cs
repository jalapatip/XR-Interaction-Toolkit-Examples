using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SpatialTracking;

public enum Enum_MicrophoneSetting
{
    Mute,
    Record,
    Debug,
    PushToTalk
}

public class Microphone_UI_Changes : MonoBehaviour
{
    public Toggle toggle;
    public Enum_MicrophoneSetting micSet;

    // Start is called before the first frame update
    void Start()
    {
        if (micSet == Enum_MicrophoneSetting.Mute)
        {
            toggle.onValueChanged.AddListener(CallMute);
        }
        else if (micSet == Enum_MicrophoneSetting.Record)
        {
            toggle.onValueChanged.AddListener(CallRecord);
        }
        else if (micSet == Enum_MicrophoneSetting.Debug)
        {
            toggle.onValueChanged.AddListener(CallDebug);
        }
        else if (micSet == Enum_MicrophoneSetting.PushToTalk)
        {
            toggle.onValueChanged.AddListener(CallPushToTalk);
        }
    }
    
    private void CallMute(bool b)
    {
        Core.Ins.Microphone.SetMute(b);
    }

    private void CallRecord(bool b)
    {
        Core.Ins.Microphone.SetRecord(b);
    }

    private void CallDebug(bool b)
    {
        Core.Ins.Microphone.SetRecord(b);
    }

    private void CallPushToTalk(bool b)
    {
        Core.Ins.Microphone.SetRecord(b);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
