using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SpatialTracking;

public enum MicrophoneSetting
{
    Mute,
    Record,
    Debug,
    PushToTalk
}

public class Microphone_UI_Changes : MonoBehaviour
{
    public Toggle toggle;
    public MicrophoneSetting micSet;

    // Start is called before the first frame update
    void Start()
    {
        if (micSet == MicrophoneSetting.Mute)
        {
            toggle.onValueChanged.AddListener(CallMute);
        }
        else if (micSet == MicrophoneSetting.Record)
        {
            toggle.onValueChanged.AddListener(CallRecord);
        }
        else if (micSet == MicrophoneSetting.Debug)
        {
            toggle.onValueChanged.AddListener(CallDebug);
        }
        else if (micSet == MicrophoneSetting.PushToTalk)
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
