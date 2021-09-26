using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public enum XROS_SpeechService
{
    UnityWindowSpeech,
}

/// <summary>
/// Vive's microphone: Microphone (2- USB Audio Device)
/// 
/// Reference: https://www.youtube.com/watch?v=GHc9RF258VA&list=PLJqYhYXx9Wpe43s7_36andLevASAILGyy&index=2
/// </summary>
public class Manager_Microphone : MonoBehaviour
{
    //Assign this audiosource from hierarchy to use for debug. 
    //This should be part of the prefab containing this script
    public AudioSource assignedDebugAudioSource;

    //this tracks whether there are any connected microphones
    private bool _hasMicrophone = false;

    //This tracks all the available recording devices
    private string[] _recordingDevices;

    int _currentSelectedDeviceId;
    private string _selectedDevice;

    // Start is called before the first frame update
    void Start()
    {
        this.LoadDevices();

        _currentVoiceRecognitionService = new UnityWindowsSpeech();
        this.InitializeSpeechService();
        Debug_RegisteringActions();
    }

    #region setting up Microphone

    //This is called in the beginning and whenever the user loads new devices.
    public void LoadDevices()
    {
        if (Microphone.devices.Length > 0)
        {
            _hasMicrophone = true;
            _recordingDevices = Microphone.devices;

            for (int i = 0; i < _recordingDevices.Length; i++)
            {
                Dev.Log("[Manager_Microphone.cs]" + i + ": " + _recordingDevices[i].ToString());
            }

            _selectedDevice = _recordingDevices[0].ToString();
        }
        else
        {
            _hasMicrophone = false;
        }

//        Dev.Log("[Manager_Microphone.cs] selected device: " + _selectedDevice);
//        Dev.Log("[Manager_Microphone.cs] _hasMicrophone " + _hasMicrophone);
    }

    public void SetDevice(int i)
    {
        if (i < 0)
        {
            i = 0;
        }
        else if (i > _recordingDevices.Length)
        {
            i = _recordingDevices.Length - 1;
        }

        _currentSelectedDeviceId = i;
    }

    private void Debug_StartRecording()
    {
        if (_hasMicrophone)
        {
            Dev.Log("[Manager_Microphone] Start recording");
            assignedDebugAudioSource.clip = GetAudioClipFromSelectedMicrophone();
            assignedDebugAudioSource.Play();
        }
        else
        {
            Dev.Log("[Manager_Microphone] _hasMicrophone is false");
        }
    }

    private void Debug_StopRecording()
    {
        if (_hasMicrophone)
        {
            Dev.Log("[Debug] Stop recording");
            assignedDebugAudioSource.Stop();
        }
    }


    /// <summary>
    /// Use this
    /// </summary>
    /// <returns></returns>
    public AudioClip GetAudioClipFromSelectedMicrophone()
    {
        //AudioSettings.outputSampleRate by default is 44k
        //Microphone.Start(_selectedDevice, true, 5, 44100);
        return Microphone.Start(_selectedDevice, true, 5, AudioSettings.outputSampleRate);
    }

    public string GetSelectedDevice()
    {
        return _selectedDevice;
    }

    public void SetNewSelectedDevice(string s)
    {
        _selectedDevice = s;
    }

    public void SetMute(bool mute)
    {
        if (mute)
        {
            assignedDebugAudioSource.mute = false;
        }
        else
        {
            assignedDebugAudioSource.mute = true;
        }
    }

    public void SetRecord(bool record)
    {
        if (record)
        {
            Debug_StartRecording();
        }
        else
        {
            Debug_StopRecording();
        }
    }

    public void SetDebug(bool debug)
    {
        if (debug)
        {
        }
        else
        {
        }
    }

    public void SetPushToTalk(bool pushToTalk)
    {
        if (pushToTalk)
        {
        }
        else
        {
        }
    }

    #endregion setting up Microphone


    // Update is called once per frame
    void Update()
    {
        DebugUpdate();
    }

    void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Dev.Log("O key");
            Debug_StartRecording();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Dev.Log("P key");
            assignedDebugAudioSource.Play();
        }
    }

    #region Voice Recognition

    /// Ref:
    /// https://www.youtube.com/watch?v=29vyEOgsW8s&t=383s
    /// The video says you need to set the Edit/Project Setting/Player/Windows Player and give access to the microphone.
    /// I think that's only needed for Windows Devices such as Hololens or Microsoft Surface.
    /// To have that option, you need to install the module "Universal Windows Platform Build Support"
    /// https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/voice-input-in-unity
    private List<IVoiceRecognitionService> _listOfVoiceRecognitionServices;

    private IVoiceRecognitionService _currentVoiceRecognitionService;
    //private bool _SpeechServiceInitialized = false;
    //private bool _listeningForKeywords;

    private Dictionary<string, Action> VoiceCommandDictionary = new Dictionary<string, Action>();
    //XROS_SpeechService currentSpeechService;

    public void InitializeSpeechService()
    {
        if (CheckForServiceExists())
        {
        }
        else
        {
            //TODO add default speech service
            Dev.LogWarning("Falling back to default Speech Service");
        }

        if (CheckForServiceExists())
        {
            _currentVoiceRecognitionService.InitializeSerivce(VoiceCommandDictionary);
            //Dev.Log("[Voice Command] Initializing " + _currentVoiceRecognitionService.GetName());
            //Dev.Log("[Voice Command] Initializing ... " + _currentVoiceRecognitionService.IsInitialized());
        }
    }

    public bool CheckForServiceAvailability()
    {
        return CheckForServiceExists() && CheckForServiceInitialization();
    }

    private bool CheckForServiceExists()
    {
        if (_currentVoiceRecognitionService != null)
        {
            return true;
        }

        Dev.LogWarning("[Voice Command] currentVoiceRecognitionService is null");

        return false;
    }

    private bool CheckForServiceInitialization()
    {
        if (_currentVoiceRecognitionService.IsInitialized())
        {
            return true;
        }

        Dev.LogWarning("[Voice Command] currentVoiceRecognitionService " + _currentVoiceRecognitionService.GetName() +
                       " is not initialized");
        return false;
    }


    /// <summary>
    /// This is for debugging only, removal candidate
    /// </summary>
    private void Debug_RegisteringActions()
    {
        RegisterVoiceCommand("set timer", TestAction1);
        RegisterVoiceCommand("cancel", TestAction2);
        //RegisterAction("sword", TestAction3);
    }

    public void RegisterVoiceCommand(string phrase, Action action)
    {
        if (VoiceCommandDictionary.ContainsKey(phrase))
        {
            Dev.Log("Voice Command already registered: " + phrase);
        }
        else
        {
//            Dev.Log("Voice Command Added: " + phrase);
            Dev.Log("Voice Command: \"" + phrase + "\" added. Corresponding method is: " + action.ToString());
            VoiceCommandDictionary.Add(phrase, action);
        }
    }

    public void ToggleListeningForKeywords()
    {
        if (!CheckForServiceAvailability())
        {
            return;
        }

        if (_currentVoiceRecognitionService.IsListeningForKeywords())
        {
            _currentVoiceRecognitionService.StopListeningForKeywords();
        }
        else
        {
            _currentVoiceRecognitionService.StartListeningForKeywords();
        }

        Dev.Log("[Voice Command] is listening to key words: " +
                _currentVoiceRecognitionService.IsListeningForKeywords());
    }

    public void HandleRecognizedSpeech(string text)
    {
        VoiceCommandDictionary[text].Invoke();
    }


    private void TestAction1()
    {
        Debug.Log("Test Action 1");
    }

    private void TestAction2()
    {
        Debug.Log("Test Action 2");
    }

    private void TestAction3()
    {
        Debug.Log("Test Action 3");
    }

    #endregion Voice Recognition
}