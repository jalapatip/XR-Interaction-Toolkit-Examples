using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for Dev tools. Right now only consists of Dev Log feature
///We move the Log feature under our own Dev.Log so we can easily disable/enable the error messages if necessary.
///The messages printed here should go to a log file.
///Note: Print is only available in mohobehavior object. 
/// </summary>


public delegate void EventHandler_NewLog(string logMessage);

public static class Dev
{
    public static event EventHandler_NewLog EVENT_NewLog;

    #region DevLog
//Input - use this to track all player Input? This could be expanded into a Log System
//IO - related to file system input, output.
//Unit - anything to do with Unit
//UnitAI - anything to do with Unit decision making
//UI - Unity UI
//ControlGroup - anything to do with CG
//GameMode 
//VR - VR
//Performance - for improving performance
//Utility - utility, tools
//Debug - debug features
//Network - Photon, network related
//Cheat - for showing cheats are activated
//Localization - for localizing languages
//Customization - User Customization, Preference, etc
//Other
    public enum LogCategory { Input, IO, Unit, UI, UnitAI, ControlGroup, GameMode, VR, Performance, Utility, Debug, Network, Cheat, Other, Localization, Customization, Event, Camera, Tool, SteamVR, Audio, Gameplay };

    public static void Log(object message)
    {
//#if UNITY_EDITOR
//        if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLog))
        {
            Dev.Log(message, LogCategory.Other);
        }
//#endif
    }
    /*
    public static void Log(object message, Object context)
    {
        if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLog))
        {
            Dev.Log(message, context);
        }
    }
    */

    //Unity LogType
    //Error
    //LogType used for Errors
    //Assert
    //LogType used for Asserts. (These could also indicate an error inside Unity itself.)
    //Warning
    //LogType used for Warnings
    //Log
    //LogType used for regular log Messages
    //Exception
    //LogType used for Exceptions
    /*
    public static void Log(object message, Object context, UnityEngine.LogType logType)
    {
        if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLog))
        {
            Debug.Log(message, context);
        }
    }*/

    public static void Log(object message, LogCategory logCategory)
    {
        //Dev.Log(message, null, logCategory);
        LogActual("" + message, null);
    }

    /*
    public static void Log(object message, Object context, LogCategory logCategory)
    {
        if (!Mgr_PlayerPrefs.instance)
        {
            Debug.LogError("Mgr_PlayerPrefs not found");
            return;
        }
        switch (logCategory)
        {
            case LogCategory.Input:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogInput))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Unit:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogUnit))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.UI:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogUI))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.UnitAI:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogUnitAI))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.GameMode:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogGameMode))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Performance:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogPerformance))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Network:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogNetwork))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.ControlGroup:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogControlGroup))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Utility:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogUtility))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Cheat:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogCheat))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.IO:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogCheat))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Localization:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogLocalization))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Customization:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogCustomization))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Event:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogEvent))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Camera:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogCamera))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            case LogCategory.Audio:
                if (Mgr_PlayerPrefs.instance.GetDevSetting(Mgr_PlayerPrefs.DevSetting.bEnableDebugLogAudio))
                {
                    LogActual("[" + logCategory.ToString() + "] " + message, context);
                }
                break;
            default:
                LogActual("[*" + logCategory.ToString() + "] " + message, context);
                break;
        }
    }
    */
    private static void LogActual(string s, Object context)
    {
        EVENT_NewLog?.Invoke(s);
        Debug.Log(s);
    }

    public static void LogError(string s)
    {
        EVENT_NewLog?.Invoke(s);
        Debug.LogError(s);
    }

    public static void LogWarning(string s)
    {
        EVENT_NewLog?.Invoke(s);
        Debug.LogWarning(s);
    }
    #endregion Dev Log
}
