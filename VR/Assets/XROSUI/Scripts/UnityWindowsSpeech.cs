using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public interface IVoiceRecognitionService
{
    void InitializeSerivce(Dictionary<string, Action> newDictionary);
    void StartListeningForKeywords();
    void StopListeningForKeywords();
    void RecognizeSpeech(String s);
    string GetName();
    bool IsInitialized();
    bool IsListeningForKeywords();
}
/// <summary>
///
///
/// Usage Notice: error if you create a new KeywordRecognizer with the same word
/// </summary>
public class UnityWindowsSpeech : IVoiceRecognitionService
{
    //private Manager_Microphone _microphone;
    private KeywordRecognizer keywordRecognizer;
    private bool _initialized = false;
    private bool _isListeningForKeywords = false;

    private Dictionary<string, Action> VoiceCommandDictionary;
    
    public void InitializeSerivce(Dictionary<string, Action> newDictionary)
    {
        VoiceCommandDictionary = newDictionary;
        Dev.Log("Voice Command Size " + VoiceCommandDictionary.Count);
        if (VoiceCommandDictionary.Count > 0)
        {
            //TODO error if you create a new KeywordRecognizer with the same word
            if (keywordRecognizer != null)
            {
                keywordRecognizer.Dispose();    
            }
            
            keywordRecognizer = new KeywordRecognizer(VoiceCommandDictionary.Keys.ToArray());

            keywordRecognizer.OnPhraseRecognized += RecognizeSpeech0;
            
            _initialized = true;
        }
    }

    public string GetName()
    {
        return "Unity.Windows.Speech";
    }

    public bool IsInitialized()
    {
        return _initialized;
    }
    public bool IsListeningForKeywords()
    {
        return _isListeningForKeywords;
    }

    private void RecognizeSpeech0(PhraseRecognizedEventArgs speech)
    {
        this.RecognizeSpeech(speech.text);
    }
    public void RecognizeSpeech(string s)
    {
        Debug.Log(s);

        Core.Ins.Microphone.HandleRecognizedSpeech(s);
    }

    public void StartListeningForKeywords()
    {
        if (_initialized)
        {
            keywordRecognizer.Start();
            _isListeningForKeywords = true;
        }
    }

    public void StopListeningForKeywords()
    {
        if (_initialized)
        {
            keywordRecognizer.Stop();
            _isListeningForKeywords = false;
        }
    }
}