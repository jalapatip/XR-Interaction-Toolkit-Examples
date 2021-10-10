using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;


public enum XROS_SpeechService
{
    UnityWindowSpeech,
}

//public delegate void Delegate_NewMicrophoneSelected();

public delegate void Delegate_NewMicrophoneList();

public delegate void Delegate_NewDictationResult();
public delegate void Delegate_NewDictationHypothesis();
public delegate void Delegate_NewDictationComplete();
public delegate void Delegate_NewDictationError();
    

//result
//hypothesis
//complete
//error

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

    private int _selectedDeviceId = 0;
    private string _selectedDevice;

    //public static event Delegate_NewMicrophoneSelected Event_NewMicrophoneSelected;
    public static event Delegate_NewMicrophoneList Event_NewMicrophoneList;
    public static event Delegate_NewDictationResult Event_NewDictationResult;
    public static event Delegate_NewDictationHypothesis Event_NewDictationHypothesis;
    public static event Delegate_NewDictationComplete Event_NewDictationComplete;
    public static event Delegate_NewDictationError Event_NewDictationError;
    
    // Start is called before the first frame update
    void Start()
    {
        this.LoadDevices();

        _currentVoiceRecognitionService = new UnityWindowsSpeech();
        InitializeSpeechService();
        SetDeviceById(_selectedDeviceId);
        Debug_RegisteringActions();

        DictationSetup();
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
//                Dev.Log("[Manager_Microphone.cs]" + i + ": " + _recordingDevices[i].ToString());
            }

            Event_NewMicrophoneList?.Invoke();

        }
        else
        {
            _hasMicrophone = false;
        }

//        Dev.Log("[Manager_Microphone.cs] selected device: " + _selectedDevice);
//        Dev.Log("[Manager_Microphone.cs] _hasMicrophone " + _hasMicrophone);
    }

    public void SetDeviceById(int i)
    {
        if (i < 0)
        {
            i = 0;
        }
        else if (i > _recordingDevices.Length)
        {
            i = _recordingDevices.Length - 1;
        }

        _selectedDeviceId = i;
    }

    
    private void Debug_StartRecording()
    {
        if (_hasMicrophone)
        {
            Dev.Log("[Manager_Microphone] Start recording");
            assignedDebugAudioSource.clip = GetAudioClipFromSelectedMicrophoneOnLoop();
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
    public AudioClip GetAudioClipFromSelectedMicrophoneOnLoop()
    {
        //AudioSettings.outputSampleRate by default is 44k
        //Microphone.Start(_selectedDevice, true, 5, 44100);
        return Microphone.Start(_selectedDevice, true, 5, AudioSettings.outputSampleRate);
    }

    public AudioClip StartRecording()
    {
        AudioClip ac = GetAudioClipFromSelectedMicrophoneOnLoop();
        return ac;
    }
    public void EndRecording()
    {
        Microphone.End(this.GetSelectedDevice());
    }

    public void SaveRecording(AudioClip ac)
    {
        this.SaveAsWav(ac);
        this.ConvertToArray(ac);
    }
    public string GetSelectedDevice()
    {
        return _recordingDevices[_selectedDeviceId];
        //return _selectedDevice;
    }

    public string[] GetDeviceList()
    {
        return _recordingDevices;
    }
    public int GetSelectedDeviceId()
    {
        return _selectedDeviceId;
    }

    // public void SetNewSelectedDevice(string s)
    // {
    //     _selectedDevice = s;
    // }

    public void SetNewSelectedDeviceById(int id)
    {
        _selectedDeviceId = id;
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

    byte[] ConvertToArray(AudioClip clip)
    {
        // Convert clip to mp3 bytes array
        //128 is recommend bitray for mp3 files
        byte[] mp3 = WavToMp3.ConvertWavToMp3(clip, 128);
        Debug.Log($"Convert to array {mp3.Length}");
        //string s = "";
        // for( int i=0; i<mp3.Length; i++)
        // {
        //     s += mp3[i];
        //         
        // }
        // Debug.Log(s);
        File.WriteAllBytes(Application.persistentDataPath + "/somefile.mp3", mp3);
        EditorUtility.RevealInFinder(Application.persistentDataPath);
        return mp3;
    }

    void SaveAsMP3(AudioClip clip)
    {
        // Save AudioClip at assets path with defined bitray as mp3
        //128 is recommend bitray for mp3 files
        EncodeMP3.SaveMp3(clip, $"{Application.persistentDataPath}/mp3File", 128);
#if UNITY_EDITOR
        Debug.Log($"Save file to {$"{Application.persistentDataPath}/*"}");
        EditorUtility.RevealInFinder(Application.persistentDataPath);
#endif
    }

    void SaveAsWav(AudioClip clip)
    {
        //var fileName = "wavFile.mp3";
        // Save AudioClip at assets path as wav
        SavWav.SaveWav($"{Application.persistentDataPath}/wavFile", clip);

#if UNITY_EDITOR
        Debug.Log($"Save file to {$"{Application.persistentDataPath}/*"}");
        EditorUtility.RevealInFinder(Application.persistentDataPath);
#endif
    }

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

        if (Input.GetKeyDown(KeyCode.I))
        {
            Dev.Log("I key");
            ConvertToArray(assignedDebugAudioSource.clip);
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            Dev.Log("J key");
            //ConvertToArray(assignedDebugAudioSource.clip);
            
            
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Dev.Log("K key");
            //SaveAsMP3(assignedDebugAudioSource.clip);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Dev.Log("L key");
            //SaveAsWav(assignedDebugAudioSource.clip);
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
            //Dev.Log("Voice Command: \"" + phrase + "\" added.");
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
    
    #region Voice Dictation
    //https://docs.unity3d.com/ScriptReference/Windows.Speech.DictationRecognizer.html
    private DictationRecognizer _dictationRecognizer;

    private string _currentHypothesis;
    private string _currentUtterance;
    
    private void DictationSetup()
    {
        _dictationRecognizer = new DictationRecognizer();

        _dictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat(Time.time + "Dictation result: {0}", text);
            _currentUtterance = text;
            Event_NewDictationResult?.Invoke();
        };

        _dictationRecognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
            _currentHypothesis = text;
            Event_NewDictationHypothesis?.Invoke();
        };

        _dictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
            Event_NewDictationComplete?.Invoke();
        };

        _dictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
            Event_NewDictationError?.Invoke();
        };

        //m_DictationRecognizer.Start();
    }

    public void DictationStart()
    {
        _dictationRecognizer.Start();
    }

    public void DictationStop()
    {
        _dictationRecognizer.Stop();
    }

    public void DictationDispose()
    {
        _dictationRecognizer.Dispose();
    }

    public SpeechSystemStatus DictationStatus()
    {
        return _dictationRecognizer.Status;
    }

    public string GetCurrentUtterance()
    {
        return _currentUtterance;
    }
    public string GetCurrentUtteranceHypothesis()
    {
        return _currentHypothesis;
    }
    #endregion Voice Dictation
}