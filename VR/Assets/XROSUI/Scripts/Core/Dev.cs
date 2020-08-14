using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

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
        Dev.Log(message, LogCategory.Other);
    }

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

    public static void Log(object message, LogCategory logCategory)
    {
        //Dev.Log(message, null, logCategory);
        LogActual("[" + logCategory.ToString() + "] " + message, null);
    }

 /*
    public static void Log(object message, Object context, LogCategory logCategory)
    {
        switch (logCategory)
        {
            case LogCategory.Input:

                break;
            case LogCategory.Unit:

                break;
            case LogCategory.UI:

                break;
            case LogCategory.UnitAI:

                break;
            case LogCategory.GameMode:

                break;
            case LogCategory.Performance:

                break;
            case LogCategory.Network:

                break;
            case LogCategory.ControlGroup:

                break;
            case LogCategory.Utility:

                break;
            case LogCategory.Cheat:

                break;
            case LogCategory.IO:

                break;
            case LogCategory.Localization:

                break;
            case LogCategory.Customization:

                break;
            case LogCategory.Event:

                break;
            case LogCategory.Camera:

                break;
            case LogCategory.Audio:

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

    public static void CheckAssignment<T>(T t, Transform go)
    {
        
        if (t != null) return;
        
        
        var hierarchyPath = go.name;
        do
        {
            go = go.transform.parent;
            hierarchyPath = go.name + "/" + hierarchyPath;
        } while (go.transform.parent != false);
        hierarchyPath = go.name + "/" + hierarchyPath;
        Dev.LogWarning("Missing Inspector Assignment in " + hierarchyPath);
    }
}
