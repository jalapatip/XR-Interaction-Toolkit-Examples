using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

//Supported Audio Formats
//https://docs.unity3d.com/Manual/AudioFiles.html
public enum ENUM_Audio_Type { master, voice, music, sfx }

public delegate void Delegate_NewVolumeMaster(float newValue);
public delegate void Delegate_NewVolumeMusic(float newValue);
public delegate void Delegate_NewVolumeVoice(float newValue);
public delegate void Delegate_NewVolumeSFX(float newValue);

//Design Note:
//a_source is used for basic system UI sound effects (such as error)
//musice_source is used for music.
//Every other sound effect would be requested and created through an object pooler.
public class Controller_Audio : MonoBehaviour
{
    public int maxLevel = 10;

    private int MasterLevel = 10;
    private int MusicLevel = 10;
    private int VoiceLevel = 10;
    private int SFXLevel = 10;

    public static event Delegate_NewVolumeMaster EVENT_NewVolumeMaster;
    public static event Delegate_NewVolumeMusic EVENT_NewVolumeMusic;
    public static event Delegate_NewVolumeVoice EVENT_NewVolumeVoice;
    public static event Delegate_NewVolumeSFX EVENT_NewVolumeSFX;

    public AudioMixer mixer;

    //Public so we can drag child objects
    public AudioSource AudioSource_Master;
    [Tooltip("Drag child object with an audiosource to be used as the default menu sfx audio source")]
    public AudioSource AudioSource_SFX;
    [Tooltip("Drag child object with an audiosource to be used as the default music source")]
    public AudioSource AudioSource_Music;
    [Tooltip("Drag child object with an audiosource to be used as the default voice source")]
    public AudioSource AudioSource_Voice;
    [Tooltip("Drag an audio file to be the default error sound")]
    public AudioClip AudioClip_Error;
    [Tooltip("Drag a GO_AudioSource Prefab to be used for all audio effects, through ObjectPooling")]
    public GO_AudioSource PF_AudioSource;

    //Tracks the different clips we have loaded
    Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

    //Class Save Data
    [HideInInspector]
    public SettingSaveData Setting;

    void Start()
    {
    }

    private void Awake()
    {
        PF_AudioSource.gameObject.SetActive(false);

        LoadAudioClip("ClickFeedback", "dumdum");
    }

    public void PlaySound(AudioClip clip)
    {
        GO_AudioSource obj = Instantiate(PF_AudioSource, transform);
        obj.PlaySound(clip);
    }

    #region OnSceneLoaded
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Reference
    //https://forum.unity.com/threads/how-to-use-scenemanager-onsceneloaded.399221/
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Dev.Log("Scene Name: " + scene.name + " build index " + scene.buildIndex, Dev.LogCategory.Audio);
        AudioClip newLevelMusicClip;
        if (scene.buildIndex == 0)
        {
            newLevelMusicClip = LoadAudioClip("Dreamland (Loop)");
            //newLevelMusicClip = LoadAudioClip("Land of Knights (Long Loop)");
        }
        else
        {
            newLevelMusicClip = LoadAudioClip("Dreamland (Loop)");
        }

        Dev.Log("Playing Music: " + newLevelMusicClip.name, Dev.LogCategory.Audio);

        if (newLevelMusicClip)
        {
            AudioSource_Music.clip = newLevelMusicClip;
            AudioSource_Music.loop = true;
            AudioSource_Music.Play();
        }
    }
    #endregion OnSceneLoaded

    #region Play Audio
    public void PlayPauseMusic()
    {
        if (AudioSource_Music.isPlaying)
        {
            AudioSource_Music.Pause();
        }
        else
        {
            AudioSource_Music.UnPause();
        }
    }
    public void PlayMaster(string audioClipName)
    {
        this.PlayMaster(LoadAudioClip(audioClipName));
    }

    public void PlayMaster(AudioClip ac)
    {
        //Make sure we have a valid clip
        if (ac == null) return;

        AudioSource source = this.AudioSource_Master;
        source.clip = ac;
        source.Play();
    }

    public void PlayMusic(string audioClipName)
    {
        this.PlayMusic(LoadAudioClip(audioClipName));
    }

    public void PlayMusic(AudioClip ac)
    {
        //Make sure we have a valid clip
        if (ac == null) return;
        AudioSource source = this.AudioSource_Music;
        AudioSource_Music.loop = true;
        source.clip = ac;
        source.Play();
    }
    public void PlaySfx(string audioClipName)
    {
        this.PlaySfx(LoadAudioClip(audioClipName));
    }

    public void PlaySfx(AudioClip ac)
    {
        //Make sure we have a valid clip
        if (ac == null) return;


        AudioSource source = this.AudioSource_SFX;
        source.clip = ac;
        source.Play();
    }
    public void PlayVoice(string audioClipName)
    {
        this.PlayVoice(LoadAudioClip(audioClipName));
    }

    public void PlayVoice(AudioClip ac)
    {
        //Make sure we have a valid clip
        if (ac == null) return;


        var source = this.AudioSource_Voice;
        source.clip = ac;
        source.Play();
    }

    private void Play3DAudio(AudioClip ac, GameObject go, float pitch = 1f)
    {
        //Make sure we have a valid clip
        if (ac == null) return;

        //Handles pitch variation
        if (pitch != 1)
        {
            pitch = UnityEngine.Random.Range(1 - pitch, 1 + pitch);
        }

        //TODO ObjectPooling
        //GameObject audioObj = PoolManager.Spawn(instance.oneShotPrefab, position, Quaternion.identity);
        //AudioSource source = audioObj.GetComponent<AudioSource>();
        AudioSource source = this.AudioSource_SFX;

        source.clip = ac;
        source.pitch = pitch;
        source.Play();

        //deactivate audio gameobject when the clip stops playing
        //PoolManager.Despawn(audioObj, clip.length);
    }

    public void Play3DAudio(string audioClipName, GameObject go)
    {
        this.Play3DAudio(LoadAudioClip(audioClipName), go);
    }

    public void Play2DAudio(string audioClipName)
    {
        this.Play3DAudio(LoadAudioClip(audioClipName), this.gameObject);
    }
    #endregion Play Audio

    #region Load Audio
    /// <summary>
    /// Loads an Audio Clip based on the resourcename, calls the other LoadAudioClip to handle the request.
    /// </summary>
    /// <param name="resourcename"></param>
    /// <returns></returns>
    private AudioClip LoadAudioClip(string resourcename)
    {
        return LoadAudioClip(resourcename, resourcename);
    }

    /// <summary>
    /// Loads an audio into the audioDictionary based on resourcename, if it hasn't been loaded
    /// </summary>
    /// <param name="name"></param>
    /// <param name="resourcename"></param>
    /// <returns></returns>
    private AudioClip LoadAudioClip(string name, string resourcename)
    {
        AudioClip ac;
        if (audioDictionary.ContainsKey(name))
        {
            ac = audioDictionary[name];
        }
        else
        {
            ac = Resources.Load<AudioClip>(resourcename);
            if (ac != null)
            {
                //Dev.Log(name + " not in dictionary, loading " + resourcename + " in Resources folder", Dev.LogCategory.Audio);
                audioDictionary.Add(name, ac);
            }
            else
            {
                Dev.Log(name + " not in dictionary and " + resourcename + " not in Resources folder. Default to error clip", Dev.LogCategory.Audio);
                //Note: To Address this issue, make sure the audio with the name you want to use is loaded into the audioDictionary.
                audioDictionary.Add(name, AudioClip_Error);
                ac = AudioClip_Error;
            }
        }

        return ac;
    }
    #endregion Load Audio
    
    #region Setting Save Data
    [Serializable]
    public struct SettingSaveData
    {
        public bool soundOn;
        public bool musicOn;
    }

    public SettingSaveData DefaultSaveData()
    {
        SettingSaveData saveData = new SettingSaveData
        {
            soundOn = true,
            musicOn = true,
        };
        return saveData;
    }

    public SettingSaveData GetSaveData()
    {
        return Setting;
    }

    public void LoadSaveData(SaveData saveData)
    {
        Setting = saveData.audioSetting;
    }
    #endregion Setting Save Data
    
    #region GET & SET
    public int GetVolumeLevel(ENUM_Audio_Type type)
    {
        switch (type)
        {
            case ENUM_Audio_Type.master:
                return MasterLevel;
            case ENUM_Audio_Type.music:
                return MusicLevel;
            case ENUM_Audio_Type.voice:
                return VoiceLevel;
            case ENUM_Audio_Type.sfx:
                return SFXLevel;
            default:
                break;
        }
        return 0;
    }

    public void AdjustVolume(int d, ENUM_Audio_Type type)
    {
        int newLevel = GetVolumeLevel(type) + d;

        if (newLevel > maxLevel)
        {
            newLevel = maxLevel;
        }
        else if (newLevel < 0)
        {
            newLevel = 0;
        }

        SetVolume(newLevel, type);
    }

    public void SetVolume(int level, ENUM_Audio_Type type)
    {
        if (level > maxLevel)
        {
            level = maxLevel;
        }
        else if (level < 0)
        {
            level = 0;
        }

        float showVol = Math.Max(0.0001f, (float)level / (float) maxLevel); // the percentage of current volume level
        float f = (float)Mathf.Log10(showVol) * 20f;

        switch (type)
        {
            case ENUM_Audio_Type.master:
                EVENT_NewVolumeMaster?.Invoke(showVol);
                mixer.SetFloat("MasterVolume", f);
                MasterLevel = level;
                break;
            case ENUM_Audio_Type.music:                
                EVENT_NewVolumeMusic?.Invoke(showVol);
                mixer.SetFloat("MusicVolume", f);
                MusicLevel = level;
                break;
            case ENUM_Audio_Type.voice:
                EVENT_NewVolumeVoice?.Invoke(showVol);
                mixer.SetFloat("VoiceVolume", f);
                VoiceLevel = level;
                break;
            case ENUM_Audio_Type.sfx:
                EVENT_NewVolumeSFX?.Invoke(showVol);
                mixer.SetFloat("SFXVolume", f);
                SFXLevel = level;
                break;
            default:
                break;

        }
    }
    #endregion GET & SET
}
