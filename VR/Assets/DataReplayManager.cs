using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

public enum ReplayDataType
{
    head, handR, handL, tracker1
}

public class DataReplayManager : MonoBehaviour
{
    
    #region Singleton Setup
    public static DataReplayManager Ins { get; private set; } = null;

    private void Awake()
    {
        // if the static reference to singleton has already been initialized somewhere AND it's not this one, then this
        // GameObject is a duplicate and should not exist
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

    
    public string filePath = "Assets/XROSUI/ML_Model/Data_Exp0/";
    public string fileName = "Exp0_ 2021-02-19-02-12-11 - Duplicates Removed.csv";

    private List<DataContainer_Exp0> dataList = new List<DataContainer_Exp0>();
    private List<string> stringList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        ReadTextFile();
    }

    private void ReadTextFile()
    {
        var inp_stm = new StreamReader(filePath + fileName);

        while (!inp_stm.EndOfStream)
        {
            var inp_ln = inp_stm.ReadLine();

            stringList.Add(inp_ln);
        }

        inp_stm.Close();

        ParseList();
    }

    private void ParseList()
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
            DataContainer_Exp0 d = new DataContainer_Exp0();
            d.StringToData(parsedList[i]);
            dataList.Add(d);
        }
    }

    public int GetMaxIndex()
    {
        return dataList.Count;
    }

    public Vector3 GetPosition(int currentIndex, ReplayDataType type)
    {
        // ReplayHeadset.transform.localPosition = dataList[currentIndex].headPos;
        // ReplayHeadset.transform.localRotation = dataList[currentIndex].headRotQ;
        // ReplayHandR.transform.localPosition = dataList[currentIndex].HandRPos;
        // ReplayHandR.transform.localRotation = dataList[currentIndex].handRRotQ;
        // ReplayHandL.transform.localPosition = dataList[currentIndex].handLPos;
        // ReplayHandL.transform.localRotation = dataList[currentIndex].handLRotQ;
        // ReplayTracker.transform.localPosition = dataList[currentIndex].tracker1Pos;
        // ReplayTracker.transform.localRotation = dataList[currentIndex].tracker1RotQ;

        Vector3 v = Vector3.zero;
        switch (type)
        {
            case ReplayDataType.head:
                v = dataList[currentIndex].headPos;
                break;
            case ReplayDataType.handR:
                v = dataList[currentIndex].HandRPos;
                break;
            case ReplayDataType.handL:
                v = dataList[currentIndex].handLPos;
                break;
            case ReplayDataType.tracker1:
                v = dataList[currentIndex].tracker1Pos;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return v;
    }
    
    public Quaternion GetRotation(int currentIndex, ReplayDataType type)
    {
        // ReplayHeadset.transform.localPosition = dataList[currentIndex].headPos;
        // ReplayHeadset.transform.localRotation = dataList[currentIndex].headRotQ;
        // ReplayHandR.transform.localPosition = dataList[currentIndex].HandRPos;
        // ReplayHandR.transform.localRotation = dataList[currentIndex].handRRotQ;
        // ReplayHandL.transform.localPosition = dataList[currentIndex].handLPos;
        // ReplayHandL.transform.localRotation = dataList[currentIndex].handLRotQ;
        // ReplayTracker.transform.localPosition = dataList[currentIndex].tracker1Pos;
        // ReplayTracker.transform.localRotation = dataList[currentIndex].tracker1RotQ;

        Quaternion v = Quaternion.identity;
        switch (type)
        {
            case ReplayDataType.head:
                v = dataList[currentIndex].headRotQ;
                break;
            case ReplayDataType.handR:
                v= dataList[currentIndex].handRRotQ;
                break;
            case ReplayDataType.handL:
                v= dataList[currentIndex].handLRotQ;
                break;
            case ReplayDataType.tracker1:
                v = dataList[currentIndex].tracker1RotQ;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return v;
    }
}