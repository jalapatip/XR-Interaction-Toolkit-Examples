using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.IO;
using TMPro;

public enum TextDisplayType
{
    Hint,
    Audio,
    System
};

public enum FlagsInControllerScenario
{
    AuthenticateWithHeart,
    Flag1,
    Flag2
}

public class Controller_Scenario : MonoBehaviour
{
    //Only set this to true in scenes that will use the even system
    public bool bHasEvent = false;

    private TextMeshProUGUI _textHint;
    private TextMeshProUGUI _textAudio;
    private TextMeshProUGUI _textSystem;
    private TextAsset _jsonAsTextAsset;

    public int GetCurrentEventId() => _currentEventId;

    private XROS_Event[] _eventsArray;
    public XROS_EventTrigger[] eventTriggers;

    private Dictionary<string, bool> _flagDictionary = new Dictionary<string, bool>();

    //TODO change to Property
    public float Waiting = -1f;
    private int _currentEventId = 0;

    private string filename = "JSON/XROS_Event";

    // Start is called before the first frame update
    private void Start()
    {
        if (!bHasEvent)
        {
            return;
        }

        Waiting = -1f; //default value -1
        // events = new XROS_Event[3];
        // events[0] = XROS_Event.CreateEvent(this, TextDisplayType.Hint, "Move your controller to the front of your eyes until you feel a vibration.Then push the grip button to enable Augmented Vision",true,"AV");
        // events[1] = XROS_Event.CreateEvent(this, TextDisplayType.Hint, "Grab your user credentials located in your chest and place it on the lock",true,"OpenDoor");
        // events[2] = XROS_Event.CreateEvent(this, TextDisplayType.Hint, "You have successfully authenticated!",3);
        // //Convert to JSON
        // string eventToJson = JsonHelper.ToJson(events, true);
        // File.WriteAllText(Application.dataPath+"/XROSUI/JSON/XROS_Event.json",eventToJson);
        //code above is used to create json file, but you don't need to use it. 
        _jsonAsTextAsset = Resources.Load(filename) as TextAsset;
        if (_jsonAsTextAsset != null)
        {
            var jsonString = _jsonAsTextAsset.text;
            _eventsArray = JsonHelper.FromJson<XROS_Event>(jsonString); //deserialize it
        }
        else
        {
            Dev.LogError("JSON/XROS_Event cannt be found", Dev.LogCategory.Event);
        }

        //make sure every flag in the list is unique.
        ValidateFlag();
        Initialize();
    }

    private void ValidateFlag()
    {
        foreach (var e in _eventsArray)
        {
            if (e.HasPrerequisite && !this.AddFlag(e.prerequisiteFlagId))
                //throw exception when people are adding redundant flagID
                throw new Exception("Flag ID has already been taken");
        }

        Dev.Log("All flags have been validated", Dev.LogCategory.Event);
    }

    //Create a flagID if it doesn't already exist in the dictionary. Used to track events
    private bool AddFlag(string flagID)
    {
        if (_flagDictionary.ContainsKey(flagID))
        {
            return false;
        }
        else
        {
            _flagDictionary.Add(flagID, false);
            return true;
        }
    }

    public void SetFlag(string flagID, bool value)
    {
        if (!bHasEvent)
        {
            return;
        }

        if (_flagDictionary.ContainsKey(flagID))
        {
            _flagDictionary[flagID] = value;
        }
        else
        {
            //alert people that they had typos.
            Dev.LogWarning("Flag ID: " + flagID + " not exists");
        }
    }

    public bool GetFlag(string flagId)
    {
        if (!bHasEvent)
        {
            return false;
        }

        if (_flagDictionary.ContainsKey((flagId)))
        {
            return _flagDictionary[flagId];
        }

        Dev.LogWarning($"{flagId} cannot be found", Dev.LogCategory.Event);
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bHasEvent)
        {
            return;
        }

        DebugUpdate();
        ProcessEvent(); //check if the text panel need to go to the next event.
        EventTrigger();
    }

    //Track Debug Inputs here
    //https://docs.google.com/spreadsheets/d/1NMH43LMlbs5lggdhq4Pa4qQ569U1lr_O7HSHESEantU/edit#gid=0
    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ProcessEvent();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            this.SetFlag("AuthenticateWithHeart", true);
        }
    }

    private void ProcessEvent()
    {
        var currentEvent = _eventsArray[_currentEventId];
        if (!currentEvent.HasPrerequisite || (currentEvent.HasPrerequisite && GetFlag(currentEvent.prerequisiteFlagId)))
        {
            // print(m_Waiting);
            if (Waiting < 0)
            {
                // time is up, go to the new event.
                _currentEventId++; //sequence+1
                if (_currentEventId < _eventsArray.Length)
                {
                    //make sure we have not reached to the end.
                    currentEvent = _eventsArray[_currentEventId];
                    Waiting = _eventsArray[_currentEventId].secondsToWait;
                    switch (currentEvent.TargetText)
                    {
                        case TextDisplayType.Hint:
                            _textHint.text = currentEvent.content;
                            Dev.Log("HINT:" + currentEvent.content, Dev.LogCategory.Event);
                            break;
                        case TextDisplayType.Audio:
                            _textAudio.text = currentEvent.content;
                            break;
                        case TextDisplayType.System:
                            _textSystem.text = currentEvent.content;
                            break;
                        default:
                            Dev.LogError("Cannot handle " + currentEvent.TargetText, Dev.LogCategory.Event);
                            break;
                    }
                }
                else
                {
                    //Reach to the end of events so clear the text.
                    _textHint.text = "";
                    _textAudio.text = "";
                    _textSystem.text = "";
                }
            }
            else
            {
                Waiting -= Time.deltaTime; // this is the timer for event, event disappears when time's up.
            }
        }
    }

    private void EventTrigger()
    {
        foreach (var trigger in eventTriggers)
        {
            if (_currentEventId == trigger.EventID)
            {
                trigger.ToDo.Invoke();
            }
        }
    }

    private void Initialize()
    {
        var currentEvent = _eventsArray[_currentEventId];
        Waiting = currentEvent.secondsToWait;
        switch (currentEvent.TargetText)
        {
            case TextDisplayType.Hint:
                _textHint.text = currentEvent.content;
                Dev.Log("HINT_TEXT:" + currentEvent.content, Dev.LogCategory.Event);
                break;
            case TextDisplayType.Audio:
                _textAudio.text = currentEvent.content;
                break;
            case TextDisplayType.System:
                _textSystem.text = currentEvent.content;
                break;
            default:
                Dev.LogWarning("Cannot handle " + currentEvent.TargetText, Dev.LogCategory.Event);
                break;
        }
    }

    //This is for Text Panels to register themselves to the ScenarioManager
    public void Register(TextMeshProUGUI text, TextDisplayType type)
    {
        switch (type)
        {
            case TextDisplayType.Hint:
                _textHint = text;
                break;
            case TextDisplayType.Audio:
                _textAudio = text;
                break;
            case TextDisplayType.System:
                _textSystem = text;
                break;
            default:
                Dev.LogWarning("Cannot handle " + type, Dev.LogCategory.Event);
                break;
        }
    }
}