using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.IO;
using TMPro;

public enum TextDisplayType { Hint, Audio, System };
public enum FlagsInControllerScenario { AuthenticateWithHeart, Flag1, Flag2 }

public class Controller_Scenario : MonoBehaviour
{
    public bool bHasEvent = false;
    TextMeshProUGUI Text_Hint;
    TextMeshProUGUI Text_Audio;
    TextMeshProUGUI Text_System;
    TextAsset json;

    public bool AddFlag(string flagID)
    {//add this function so when a new XROS_Event is created, a flagID will be automatically created too.
        if (flagDictionary.ContainsKey(flagID))
        {
            return false;
        }
        else
        {
            flagDictionary.Add(flagID, false);
            return true;
        }
    }
    public int GetCurrentEventID()
    {
        return this.currentEventId;
    }

    XROS_Event[] events;
    public XROS_EventTrigger[] eventTriggers;
    Dictionary<string, bool> flagDictionary = new Dictionary<string, bool>();
    public float m_Waiting;

    int currentEventId = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!bHasEvent)
        {
            return;
        }
        Dev.Log("Start: " + Application.dataPath + "/XROSUI/JSON/XROS_Event.json");

        m_Waiting = -1f;//default value -1
        // events = new XROS_Event[3];
        // events[0] = XROS_Event.CreateEvent(this, TextDisplayType.Hint, "Move your controller to the front of your eyes until you feel a vibration.Then push the grip button to enable Augmented Vision",true,"AV");
        // events[1] = XROS_Event.CreateEvent(this, TextDisplayType.Hint, "Grab your user credentials located in your chest and place it on the lock",true,"OpenDoor");
        // events[2] = XROS_Event.CreateEvent(this, TextDisplayType.Hint, "You have successfully authenticated!",3);
        // //Convert to JSON
        // string eventToJson = JsonHelper.ToJson(events, true);
        // File.WriteAllText(Application.dataPath+"/XROSUI/JSON/XROS_Event.json",eventToJson);
        //code above is used to create json file, but you don't need to use it. 
        json = Resources.Load("JSON/XROS_Event") as TextAsset;
        string jsonString = json.text;
        //Dev.Log(jsonString);


        // string jsonString = File.ReadAllText(Application.dataPath + "/XROSUI/JSON/XROS_Event.json");//read the file
        Dev.Log("Start JSON Length: " + jsonString.Length);
        events = JsonHelper.FromJson<XROS_Event>(jsonString);//deserialize it
        CheckFlag();//make sure every flag in the list is unique.
        Initializer();
    }
    private bool CheckFlag()
    {
        foreach (XROS_Event e in events)
        {
            if (e.HasPrerequisite && !this.AddFlag(e.prerequisiteFlagId))
                throw new Exception("Flag ID has already been taken");//throw exception when people are adding redundent flagID
        }
        return true;
    }

    public void SetFlag(string flagID, bool value)
    {
        if(!bHasEvent)
        {
            return;
        }
        if (flagDictionary.ContainsKey(flagID))
        {
            flagDictionary[flagID] = value;
        }
        else
        {
            //alert people that they had typos.
            //throw new Exception("Flag ID: " + flagID + " not exists");
            //Dev.LogError("flagID: " + flagID + " not exists");
            Dev.LogWarning("Flag ID: " + flagID + " not exists");
        }
    }
    public bool GetFlag(string flagID)
    {
        return flagDictionary[flagID];
    }

    // Update is called once per frame
    void Update()
    {
        if(!bHasEvent)
        {
            return;
        }
        // if (Input.GetKeyDown(KeyCode.Alpha9))
        // {
        //     ProcessEvent();
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     this.SetFlag("AuthenticateWithHeart", true);
        // }
        ProcessEvent();//check if the text panel need to go to the next event.
        EventTrigger();
    }

    private void ProcessEvent()
    {
        XROS_Event currentEvent = events[currentEventId];
        if (!currentEvent.HasPrerequisite || (currentEvent.HasPrerequisite && GetFlag(currentEvent.prerequisiteFlagId)))
        {
            // print(m_Waiting);
            if (m_Waiting < 0)
            {// time is up, go to the new event.
                currentEventId++;//sequence+1
                if (currentEventId < events.Length)
                {//make sure we have not reached to the end.
                    currentEvent = events[currentEventId];
                    m_Waiting = events[currentEventId].secondsToWait;
                    switch (currentEvent.TargetText)
                    {
                        case TextDisplayType.Hint:
                            Text_Hint.text = currentEvent.content;
                            print("HINT:" + currentEvent.content);
                            break;
                        case TextDisplayType.Audio:
                            Text_Audio.text = currentEvent.content;
                            break;
                        case TextDisplayType.System:
                            Text_System.text = currentEvent.content;
                            break;
                        default:
                            print("Cannot handle " + currentEvent.TargetText);
                            break;
                    }
                }
                else
                {//Reach to the end of events so clear the text.
                    Text_Hint.text = "";
                    Text_Audio.text = "";
                    Text_System.text = "";
                }
            }
            else
            {
                m_Waiting -= Time.deltaTime;// this is the timer for event, event disappears when time's up.
            }
        }
    }

    void EventTrigger(){
        foreach (XROS_EventTrigger trigger in eventTriggers)
        {
            if(currentEventId==trigger.EventID){
                trigger.ToDo.Invoke();
            }
        }
    }

    void Initializer()
    {
        XROS_Event currentEvent = events[currentEventId];
        m_Waiting = events[currentEventId].secondsToWait;
        switch (currentEvent.TargetText)
        {
            case TextDisplayType.Hint:
                Text_Hint.text = currentEvent.content;
                print("HINT_TEXT:" + currentEvent.content);
                break;
            case TextDisplayType.Audio:
                Text_Audio.text = currentEvent.content;
                break;
            case TextDisplayType.System:
                Text_System.text = currentEvent.content;
                break;
            default:
                print("Cannot handle " + currentEvent.TargetText);
                break;
        }
    }

    //This is for Text Panels to register themselves to the ScenarioManager
    public void Register(TextMeshProUGUI text, TextDisplayType type)
    {
        switch (type)
        {
            case TextDisplayType.Hint:
                Text_Hint = text;
                break;
            case TextDisplayType.Audio:
                Text_Audio = text;
                break;
            case TextDisplayType.System:
                Text_System = text;
                break;
            default:
                print("Cannot handle " + type);
                break;
        }
    }
}

public static class JsonHelper//helper for json process, do not change.
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
