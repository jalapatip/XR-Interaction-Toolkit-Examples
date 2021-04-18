using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.MLAgents;
using UnityEngine;
using Random = System.Random;

/*public enum ReplayDataType
{
    head,
    handR,
    handL,
    tracker1
}*/

public class DataReplayManagerGesture : MonoBehaviour
{
    #region Singleton Setup

    public static DataReplayManagerGesture Ins { get; private set; } = null;

    private void SingletonAwake()
    {
        // if the static reference to singleton has already been initialized somewhere AND it's not this one, then this
        // GameObject is a duplicate and should not exist
        //Debug.Log(" is an active object " + this.GetInstanceID());
        if (Ins != null && Ins != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Ins = this;
            //So this singleton will stay when we change scenes.
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    #endregion Singleton Setup


    public string filePath = "Assets/XROSUI/ML_Model/Data_Exp1/";

    public List<string> fileNames = new List<string>();
    //public string fileName = "Exp0_ 2021-02-19-02-12-11 - Duplicates Removed.csv";

    private List<DataContainer_Exp1GesturesPosition> currentDataList = new List<DataContainer_Exp1GesturesPosition>();
    private List<DataContainer_User> userDataList = new List<DataContainer_User>();
    //private List<DataCollection_ExpGestures> currentDataList = new List<DataCollection_ExpGestures>();
    //private List<string> stringList = new List<string>();

    //var i = UnityEngine.Random.Range(0, fileNames.Count);
    public string file = "none";
    private void Awake()
    {
        SingletonAwake();

        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        
        string fileName = "";
        if (fileNames.Count > 0)
        {
            for (int i = 0; i < fileNames.Count; i++)
            {
                fileName = fileNames[i];
                Debug.Log("Random is " + i + ". Using fileName " + fileName);
                file = fileName;
                
                ReadTextFile(fileName);                
            }
        }
        else
        {
            Debug.LogError("DataReplayManager.cs is not assigned any file names");
        }
       // print("filename: " + fileName);
       // print("FileSize: " + this.GetMaxIndex());
        
    }

    private void EnvironmentReset()
    {
        
    }

    private void ReadTextFile(string fileName)
    {
        var inp_stm = new StreamReader(filePath + fileName);

        List<string> stringList = new List<string>();
        while (!inp_stm.EndOfStream)
        {
            var inp_ln = inp_stm.ReadLine();

            stringList.Add(inp_ln);
        }

        inp_stm.Close();
        if (fileName.Equals("UserCalibration.csv"))
        {
            ParseList2(stringList);
        }
        else
        {
            ParseList(stringList);
        }
    }

    private void ParseList(List<string> stringList)
    {
        List<string[]> parsedList = new List<string[]>();
        for (int i = 1; i < stringList.Count; i++)
        {
            string[] temp = stringList[i].Split(',');
            for (int j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim(); //removed the blank spaces
            }

            parsedList.Add(temp);
        }
        //you should now have a list of arrays, ewach array can ba appied to the script that's on the Sprite
        //you'll have to figure out a way to push the data the sprite

        for (int i = 0; i < parsedList.Count; i++)
        {
            DataContainer_Exp1GesturesPosition d = new DataContainer_Exp1GesturesPosition();
            d.StringToData(parsedList[i]);
            currentDataList.Add(d);
        }
    }
    
    private void ParseList2(List<string> stringList)
    {
        List<string[]> parsedList = new List<string[]>();
        for (int i = 1; i < stringList.Count; i++)
        {
            string[] temp = stringList[i].Split(',');
            for (int j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim(); //removed the blank spaces
            }

            parsedList.Add(temp);
        }
        //you should now have a list of arrays, ewach array can ba appied to the script that's on the Sprite
        //you'll have to figure out a way to push the data the sprite

        for (int i = 0; i < parsedList.Count; i++)
        {
            DataContainer_User d = new DataContainer_User();
            d.StringToData(parsedList[i]);
            userDataList.Add(d);
        }
    }

    public string GetFileName()
    {
        return file;
    }
    public int GetMaxIndex()
    {
        return currentDataList.Count;
    }

    public Vector3 GetPosition(int currentIndex, ReplayDataType type)
    {
        Vector3 v = Vector3.zero;
        switch (type)
        {
            case ReplayDataType.head:
                v = currentDataList[currentIndex].headPos;
                break;
            case ReplayDataType.handR:
                v = currentDataList[currentIndex].handRPos;
                break;
            case ReplayDataType.handL:
                v = currentDataList[currentIndex].handLPos;
                break;
            case ReplayDataType.tracker1:
                v = currentDataList[currentIndex].tracker1Pos;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return v;
    }

    public Quaternion GetRotation(int currentIndex, ReplayDataType type)
    {
        Quaternion v = Quaternion.identity;
        switch (type)
        {
            case ReplayDataType.head:
                v = currentDataList[currentIndex].headRotQ;
                break;
            case ReplayDataType.handR:
                v = currentDataList[currentIndex].handRRotQ;
                break;
            case ReplayDataType.handL:
                v = currentDataList[currentIndex].handLRotQ;
                break;
            case ReplayDataType.tracker1:
                v = currentDataList[currentIndex].tracker1RotQ;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return v;
    }
    
    public String GetGesture(int currentIndex)
    {
        String v = currentDataList[currentIndex].gesture;
        return v;
    }

    public String GetUserID(int currentIndex)
    {
        String v = currentDataList[currentIndex].userID;
        return v;
    }
}