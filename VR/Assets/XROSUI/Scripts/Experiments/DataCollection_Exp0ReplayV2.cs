using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
/// <summary>
/// This is different from DataCollection_Exp0Replay in that it uses scalar to convert data
/// </summary>
public class DataCollection_Exp0ReplayV2 : MonoBehaviour
{
    [Header("Check this bool if you want to start with it playing")]
    public bool startPlayback = false;
    
    [FormerlySerializedAs("filePath")]
    [Header("Modify using inspector.")]
    public string file1Path = "Assets/XROSUI/ML_Model/Data_Exp0/";
    public string file2Path = "Assets/XROSUI/ML_Model/Data_Exp0/";
    [FormerlySerializedAs("fileName")]
    public string file1Name = "Exp0_ 2021-02-19-02-12-11 - Duplicates Removed";
    [FormerlySerializedAs("fileName2")]
    public string file2Name = "TestData_Input_Predictions (1)";
    public string file1Extension = ".csv";
    public string file2Extension = ".csv";
    public TextAsset scalerSource;
    
    [Header("Assign via Inspector")]
    public GameObject ReplayHeadset;
    public GameObject ReplayHandR;
    public GameObject ReplayHandL;
    public GameObject ReplayTracker;
    public GameObject PredictedTracker;
    
    private float _startTime = 0;
    private int _currentIndex = 0;
    private List<DataContainer_Exp0> _dataList = new List<DataContainer_Exp0>();
    private List<DataContainer_Exp0Prediction> _predictionList = new List<DataContainer_Exp0Prediction>();
    private List<string> _stringList;

    
    private Dictionary<string, Scaler> _scalers = new Dictionary<string, Scaler>();
    
    // Start is called before the first frame update
    void Start()
    {
        var scalers = JsonUtility.FromJson<Scalers>(scalerSource.text);
        foreach (var scaler in scalers.scalers)
        {
            _scalers.Add(scaler.type, scaler);
        }

        var stringList = ReadTextFile(file1Path + file1Name + file1Extension);
        var parsedList = ParseStringListToDataList(stringList);
        _dataList = CsvListToDataList<DataContainer_Exp0>(parsedList);
        
        
        var stringList2 = ReadTextFile(file2Path + file2Name + file2Extension);
        var parsedList2 = ParseStringListToDataList(stringList2);
        _predictionList = CsvListToDataList<DataContainer_Exp0Prediction>(parsedList2);
    }

    private List<string> ReadTextFile(string path)
    {
        var inp_stm = new StreamReader(path);
        var stringList = new List<string>();
        while (!inp_stm.EndOfStream)
        {
            var inp_ln = inp_stm.ReadLine();

            stringList.Add(inp_ln);
        }

        inp_stm.Close();
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
    
    private List<T> CsvListToDataList<T>(List<string[]> csvList) where T : DataContainer, new()
    {
        //you should now have a list of arrays, ewach array can ba appied to the script that's on the Sprite
        //you'll have to figure out a way to push the data the sprite
        var dataList = new List<T>();
        print("Count: " +csvList.Count);
        for (var i = 0; i < csvList.Count; i++)
        {
            var dataContainer = new T();
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
            Debug.Log("M is pressed");
            startPlayback = true;
            _startTime = Time.time;
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

    void ModifyPosition()
    {
//        Debug.Log("Modify Position");
        if (_currentIndex < _dataList.Count)
        {
            //print("currentIndex: " + currentIndex + " at " + Time.time);
            ReplayHeadset.transform.localPosition = _dataList[_currentIndex].headPos;
            ReplayHeadset.transform.localRotation = _dataList[_currentIndex].headRotQ;
            ReplayHandR.transform.localPosition = _dataList[_currentIndex].handRPos;
            ReplayHandR.transform.localRotation = _dataList[_currentIndex].handRRotQ;
            ReplayHandL.transform.localPosition = _dataList[_currentIndex].handLPos;
            ReplayHandL.transform.localRotation = _dataList[_currentIndex].handLRotQ;
            ReplayTracker.transform.localPosition = _dataList[_currentIndex].tracker1Pos;
            ReplayTracker.transform.localRotation = _dataList[_currentIndex].tracker1RotQ;

            if (_currentIndex - 11 > 0)
            {
                var beforeScaler = _predictionList[_currentIndex - 11].tracker1Pos;
//                Debug.Log("B: " + beforeScaler.y.ToString());
                Vector3 afterScaler = Vector3.zero;
                afterScaler.x = _scalers["tracker1Posx"].InverseTransform(beforeScaler.x);
                afterScaler.y = _scalers["tracker1Posy"].InverseTransform(beforeScaler.y);
                afterScaler.z = _scalers["tracker1Posz"].InverseTransform(beforeScaler.z);
//                Debug.Log("A: " + afterScaler.y.ToString());                    
//                PredictedTracker.transform.localPosition = dataList[currentIndex].headPos - afterScaler;
                PredictedTracker.transform.localPosition = afterScaler;
//                Debug.Log("H: " + ReplayHeadset.transform.localPosition.y.ToString());
//                Debug.Log("P: " + PredictedTracker.transform.localPosition.y.ToString());
                
                beforeScaler = _predictionList[_currentIndex - 11].tracker1Rot;
                afterScaler = Vector3.zero;
                afterScaler.x = _scalers["tracker1Rotx"].InverseTransform(beforeScaler.x);
                afterScaler.y = _scalers["tracker1Roty"].InverseTransform(beforeScaler.y);
                afterScaler.z = _scalers["tracker1Rotz"].InverseTransform(beforeScaler.z);

                PredictedTracker.transform.localEulerAngles = afterScaler;
            }
            _currentIndex++;
        }
        else
        {
            _currentIndex = 0;
        }
    }

    private void RandomPosition()
    {
        _currentIndex = (int) UnityEngine.Random.Range(0, _dataList.Count);
        //Debug.Log(currentIndex);
    }
}