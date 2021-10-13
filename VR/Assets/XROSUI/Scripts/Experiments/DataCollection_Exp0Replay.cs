using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class DataCollection_Exp0Replay : MonoBehaviour
{
    [Header("Check this bool if you want to start with it playing")]
    public bool startPlayback = false;

    [FormerlySerializedAs("filePath")]
    [Header("Modify using inspector.")]
    public string file1Path = "Assets/XROSUI/ML_Model/Data_Exp0/";

    [FormerlySerializedAs("fileName")]
    public string file1Name = "Exp0_ 2021-02-19-02-12-11 - Duplicates Removed";

    public string file1Extension = ".csv";

    [Header("Assign via Inspector")]
    public GameObject ReplayHeadset;

    public GameObject ReplayHandR;
    public GameObject ReplayHandL;
    public GameObject ReplayTracker;

    private float startTime = 0;

    private int currentIndex = 0;

    //private List<string> _stringList;
    //Modify these when trying different experiment data..
    private List<DataContainer_Exp0> _dataList = new List<DataContainer_Exp0>();
    private DataContainer_Exp0 _dataContainer;

    // Start is called before the first frame update
    void Start()
    {
        var stringList = ReadTextFile(file1Path + file1Name + file1Extension);
        var parsedList = ParseStringListToDataList(stringList);
        _dataList = CsvListToDataList<DataContainer_Exp0>(parsedList);
    }

    private List<string> ReadTextFile(string path)
    {
        var stringList = new List<string>();
        try
        {
            var inp_stm = new StreamReader(path);
            while (!inp_stm.EndOfStream)
            {
                var inp_ln = inp_stm.ReadLine();

                stringList.Add(inp_ln);
            }

            inp_stm.Close();
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.ToString());
        }

        return stringList;
    }

    private List<string[]> ParseStringListToDataList(List<string> stringList)
    {
        var parsedList = new List<string[]>();
        for (var i = 1; i < stringList.Count; i++)
        {
            var temp = stringList[i].Split(',');
            for (var j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim(); //removed the blank spaces
            }

            parsedList.Add(temp);
        }

        return parsedList;
    }

    private List<T> CsvListToDataList<T>(List<string[]> csvList) where T : DataContainer_Base, new()
    {
        //you should now have a list of arrays, ewach array can ba appied to the script that's on the Sprite
        //you'll have to figure out a way to push the data the sprite
        var dataList = new List<T>();
        print("[DataCollection_Exp0Replay] CSV Row Count: " + csvList.Count);
        for (var i = 0; i < csvList.Count; i++)
        {
            T dataContainer = new T();
            dataContainer.StringToData(csvList[i]);
            dataList.Add(dataContainer);
        }

        return dataList;
    }

// Update is called once per frame
    void Update()
    {
        if (startPlayback)
        {
            ModifyPosition();
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            StartPlayback(!startPlayback);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            ModifyPosition();
        }

        if (Input.GetKeyUp(KeyCode.N))
        {
            RandomPosition();
        }
    }

    private void StartPlayback(bool b)
    {
        Debug.Log("M is pressed");
        startPlayback = b;
        startTime = Time.time;
    }

    private int startingX = 210;
    private int debugButtonWidth = 200;
    private int debugButtonHeight = 50;

    void OnGUI()
    {
        string button1string = "";
        if (startPlayback)
        {
            button1string = "Stop Playback";
        }
        else
        {
            button1string = "Start Playback";
        }

        if (GUI.Button(new Rect(startingX, 10, debugButtonWidth, debugButtonHeight), button1string))
        {
            StartPlayback(!startPlayback);
        }

        if (GUI.Button(new Rect(startingX, 60, debugButtonWidth, debugButtonHeight), "Modify Position by 1"))
        {
            ModifyPosition();
        }

        if (GUI.Button(new Rect(startingX, 110, debugButtonWidth, debugButtonHeight), "Random Position"))
        {
            RandomPosition();
        }

        // if (GUI.Button(new Rect(startingX, 160, debugButtonWidth, debugButtonHeight), "Save Experiment"))
        // {
        //     SaveExperimentData();
        // }

        //GUI.Label(new Rect(startingX, 210, debugButtonWidth, debugButtonHeight), this.GetGoalString());
    }

    private void ModifyPosition()
    {
        if (currentIndex < _dataList.Count)
        {
//            print("currentIndex: " + currentIndex + " at " + Time.time);
            ReplayHeadset.transform.localPosition = _dataList[currentIndex].headPos;
            ReplayHeadset.transform.localRotation = _dataList[currentIndex].headRotQ;
            ReplayHandR.transform.localPosition = _dataList[currentIndex].handRPos;
            ReplayHandR.transform.localRotation = _dataList[currentIndex].handRRotQ;
            ReplayHandL.transform.localPosition = _dataList[currentIndex].handLPos;
            ReplayHandL.transform.localRotation = _dataList[currentIndex].handLRotQ;
            ReplayTracker.transform.localPosition = _dataList[currentIndex].tracker1Pos;
            ReplayTracker.transform.localRotation = _dataList[currentIndex].tracker1RotQ;

            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
    }

    private void RandomPosition()
    {
        currentIndex = (int)UnityEngine.Random.Range(0, _dataList.Count);
        //Debug.Log(currentIndex);
    }
}