using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

//Supported Audio Formats
//https://docs.unity3d.com/Manual/AudioFiles.html
public enum ENUM_Audio_Type
{
    Master,
    Voice,
    Music,
    Sfx
}

#region delegates
//These delegates are used to notify when the audio volume of the 4 types of Audio changes
public delegate void Delegate_NewVolumeMaster(float newValue);
public delegate void Delegate_NewVolumeMusic(float newValue);
public delegate void Delegate_NewVolumeVoice(float newValue);
public delegate void Delegate_NewVolumeSFX(float newValue);
#endregion delegates

//This delegate is used to notify when the music played changes.
//Subscribe to this to dynamically pop up the music that is being played
public delegate void Delegate_NewMusic(string musicName);

//Design Note:
//a_source is used for basic system UI sound effects (such as error)
//musice_source is used for music.
//Every other sound effect would be requested and created through an object pooler.
public class Controller_Audio : MonoBehaviour
{
    #region Events
    public static event Delegate_NewMusic EVENT_NewMusic;
    public static event Delegate_NewVolumeMaster EVENT_NewVolumeMaster;
    public static event Delegate_NewVolumeMusic EVENT_NewVolumeMusic;
    public static event Delegate_NewVolumeVoice EVENT_NewVolumeVoice;
    public static event Delegate_NewVolumeSFX EVENT_NewVolumeSfx;
    #endregion Events
    
    [Tooltip("Customize for volume's max level")]
    public int maxLevel = 10;
    [Tooltip("Assigned using Project Tab")]
    public AudioMixer mixer;

    //Public so we can drag child objects
    public AudioSource AudioSource_Master;

    [Tooltip("Assign in Inspector a GameObject with audioSource to be the default sfx audio source")]
    public AudioSource AudioSource_SFX;

    [Tooltip("Assign in Inspector a GameObject with audioSource to be the default music audio source")]
    public AudioSource AudioSource_Music;

    [Tooltip("Assign in Inspector a GameObject with audioSource to be the default voice audio source")]
    public AudioSource AudioSource_Voice;

    [Tooltip("Drag an audio file to be the default error sound")]
    public AudioClip AudioClip_Error;

    [Tooltip("Drag a GO_AudioSource Prefab to be used for all audio effects, through ObjectPooling")]
    public GO_AudioSource PF_AudioSource;
    
    private int _masterVolumeLevel = 10;
    private int _musicVolumeLevel = 10;
    private int _voiceVolumeLevel = 10;
    private int _sfxVolumeLevel = 10;

    //Tracks the different clips we have loaded
    private Dictionary<string, AudioClip> _audioDictionary = new Dictionary<string, AudioClip>();

    //Class Save Data
    [HideInInspector] public SettingSaveData Setting;

    private void Awake()
    {
        PF_AudioSource.gameObject.SetActive(false);
    }

    public void PlaySound(AudioClip clip)
    {
        var obj = Instantiate(PF_AudioSource, transform);
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Dev.Log("Scene Name: " + scene.name + " build index " + scene.buildIndex, Dev.LogCategory.Audio);
        // AudioClip newLevelMusicClip;
        //
        // if (scene.buildIndex == 0)
        // {
        //     newLevelMusicClip = LoadAudioClip("Dreamland (Loop)");
        //     //newLevelMusicClip = LoadAudioClip("Land of Knights (Long Loop)");
        // }
        // else
        // {
        //     newLevelMusicClip = LoadAudioClip("Dreamland (Loop)");
        // }
        //
        // Dev.Log("Playing Music: " + newLevelMusicClip.name, Dev.LogCategory.Audio);
        //
        // if (newLevelMusicClip)
        // {
        //     AudioSource_Music.clip = newLevelMusicClip;
        //     AudioSource_Music.loop = true;
        //     AudioSource_Music.Play();
        // }
    }
    #endregion OnSceneLoaded

    //These are just optional APIs for others to use that's hopefully easier to use.
    //We might remove them and just have everyone use PlayAudio and supply their own Audio Type
    #region PlayAudioVariations
    public void PlayMaster(string audioClipName)
    {
        PlayAudio(audioClipName, ENUM_Audio_Type.Master);
    }

    public void PlayMaster(AudioClip ac)
    {
        PlayAudio(ac, ENUM_Audio_Type.Master);
    }

    public void PlayMusic(string audioClipName)
    {
        this.PlayAudio(audioClipName, ENUM_Audio_Type.Music);
    }

    public void PlayMusic(AudioClip ac)
    {
        PlayAudio(ac, ENUM_Audio_Type.Music);
    }

    public void PlaySfx(string audioClipName)
    {
        PlayAudio(audioClipName, ENUM_Audio_Type.Sfx);
    }

    public void PlaySfx(AudioClip ac)
    {
        PlayAudio(ac, ENUM_Audio_Type.Sfx);
    }

    public void PlayVoice(string audioClipName)
    {
        PlayAudio(audioClipName, ENUM_Audio_Type.Voice);
    }

    public void PlayVoice(AudioClip ac)
    {
        PlayAudio(ac, ENUM_Audio_Type.Voice);
    }
    #endregion PlayAudioVariations
    
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

    public void PlayAudio(string audioClipName, ENUM_Audio_Type type)
    {
        PlayAudio(LoadAudioClip(audioClipName), type);
    }

    public void PlayAudio(AudioClip ac, ENUM_Audio_Type type)
    {
        //Make sure we have a valid clip
        if (ac == null) return;

        var source = AudioSource_Master;

        switch (type)
        {
            case ENUM_Audio_Type.Master:
                source = AudioSource_Master;
                break;
            case ENUM_Audio_Type.Voice:
                source = AudioSource_Voice;
                break;
            case ENUM_Audio_Type.Music:
                source = AudioSource_Music;
                source.loop = true;
                EVENT_NewMusic?.Invoke(ac.name);
                Dev.Log("Now playing: " + ac.name, Dev.LogCategory.Audio);
                break;
            case ENUM_Audio_Type.Sfx:
                source = this.AudioSource_SFX;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        source.clip = ac;
        source.Play();
    }

    private float _pitchTolerance = 0.5f;
    private void Play3DAudio(AudioClip ac, GameObject go, float pitchVariance = 1f)
    {
        //Make sure we have a valid clip
        if (ac == null) return;

        //Handles pitch variation
        if (Math.Abs(pitchVariance - 1) < _pitchTolerance)
        {
            pitchVariance = UnityEngine.Random.Range(1 - pitchVariance, 1 + pitchVariance);
        }

        //TODO ObjectPooling
        //GameObject audioObj = PoolManager.Spawn(instance.oneShotPrefab, position, Quaternion.identity);
        //AudioSource source = audioObj.GetComponent<AudioSource>();
        var source = this.AudioSource_SFX;

        source.clip = ac;
        source.pitch = pitchVariance;
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
    /// Loads an Audio Clip based on the resourceName, calls the other LoadAudioClip to handle the request.
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    private AudioClip LoadAudioClip(string resourceName)
    {
        return LoadAudioClip(resourceName, resourceName);
    }

    /// <summary>
    /// Loads an audio into the audioDictionary based on resourceName, if it hasn't been loaded
    /// </summary>
    /// <param name="audioName"></param>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    private AudioClip LoadAudioClip(string audioName, string resourceName)
    {
        AudioClip ac;
        if (_audioDictionary.ContainsKey(audioName))
        {
            ac = _audioDictionary[audioName];
        }
        else
        {
            ac = Resources.Load<AudioClip>(resourceName);
            if (ac != null)
            {
                //Dev.Log(name + " not in dictionary, loading " + resourceName + " in Resources folder", Dev.LogCategory.Audio);
                _audioDictionary.Add(audioName, ac);
            }
            else
            {
                Dev.Log(
                    audioName + " not in dictionary and " + resourceName +
                    " not in Resources folder. Default to error clip",
                    Dev.LogCategory.Audio);
                //Note: To Address this issue, make sure the audio with the name you want to use is loaded into the audioDictionary.
                _audioDictionary.Add(audioName, AudioClip_Error);
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

    #region Volume GET & SET
    public int GetVolumeLevel(ENUM_Audio_Type type)
    {
        switch (type)
        {
            case ENUM_Audio_Type.Master:
                return _masterVolumeLevel;
            case ENUM_Audio_Type.Music:
                return _musicVolumeLevel;
            case ENUM_Audio_Type.Voice:
                return _voiceVolumeLevel;
            case ENUM_Audio_Type.Sfx:
                return _sfxVolumeLevel;
            default:
                break;
        }

        return 0;
    }

    /// <summary>
    /// This method adjusts the volume by the given integer amount
    /// </summary>
    /// <param name="d"></param>
    /// <param name="type"></param>
    public void AdjustVolume(int d, ENUM_Audio_Type type)
    {
        var newLevel = GetVolumeLevel(type) + d;

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

    /// <summary>
    /// This method sets the volume to the given integer amount
    /// </summary>
    /// <param name="level"></param>
    /// <param name="type"></param>
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

        var showVol = Math.Max(0.0001f, (float) level / (float) maxLevel); // the percentage of current volume level
        var f = (float) Mathf.Log10(showVol) * 20f;

        switch (type)
        {
            case ENUM_Audio_Type.Master:
                EVENT_NewVolumeMaster?.Invoke(showVol);
                mixer.SetFloat("MasterVolume", f);
                _masterVolumeLevel = level;
                break;
            case ENUM_Audio_Type.Music:
                EVENT_NewVolumeMusic?.Invoke(showVol);
                mixer.SetFloat("MusicVolume", f);
                _musicVolumeLevel = level;
                break;
            case ENUM_Audio_Type.Voice:
                EVENT_NewVolumeVoice?.Invoke(showVol);
                mixer.SetFloat("VoiceVolume", f);
                _voiceVolumeLevel = level;
                break;
            case ENUM_Audio_Type.Sfx:
                EVENT_NewVolumeSfx?.Invoke(showVol);
                mixer.SetFloat("SFXVolume", f);
                _sfxVolumeLevel = level;
                break;
            default:
                break;
        }
    }
    #endregion Volume GET & SET
}