using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

public class DataCollection_Exp0ReplayV2 : MonoBehaviour
{
    public string filePath = "Assets/XROSUI/ML_Model/Data_Exp0/";
    public string fileName = "Exp0_ 2021-02-19-02-12-11 - Duplicates Removed,csv";
    public string fileName2 = "TestData_Input_Predictions (1).csv";
    public GameObject ReplayHeadset;
    public GameObject ReplayHandR;
    public GameObject ReplayHandL;
    public GameObject ReplayTracker;
    public GameObject PredictedTracker;
    
    private List<DataContainer_Exp0> dataList = new List<DataContainer_Exp0>();
    private List<DataContainer_Exp0Prediction> predictionList = new List<DataContainer_Exp0Prediction>();
    private List<string> stringList;

    public TextAsset scalerSource;
    private Dictionary<string, Scaler> _scalers = new Dictionary<string, Scaler>();
    
    // Start is called before the first frame update
    void Start()
    {
        Scalers scalers = JsonUtility.FromJson<Scalers>(scalerSource.text);
        foreach (Scaler scaler in scalers.scalers)
        {
            _scalers.Add(scaler.type, scaler);
        }

        stringList = new List<string>();
        ReadTextFile(filePath + fileName);
        parseList();

        stringList = new List<string>();
        ReadTextFile(filePath + fileName2);
        parseList2();
    }

    private void ReadTextFile(string path)
    {
        var inp_stm = new StreamReader(path);

        while (!inp_stm.EndOfStream)
        {
            var inp_ln = inp_stm.ReadLine();

            stringList.Add(inp_ln);
        }

        inp_stm.Close();
    }

    void parseList()
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

//        print("Count: " + parsedList.Count);
        for (int i = 0; i < parsedList.Count; i++)
        {
            DataContainer_Exp0 d = new DataContainer_Exp0();
            d.StringToData(parsedList[i]);
            dataList.Add(d);
        }
    }

    void parseList2()
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

//        print("Count: " + parsedList.Count);
        for (int i = 0; i < parsedList.Count; i++)
        {
            DataContainer_Exp0Prediction d = new DataContainer_Exp0Prediction();
            d.StringToData(parsedList[i]);
            predictionList.Add(d);
        }
    }
    
    public bool startPlayback = false;

    private float startTime = 0;

    private int currentIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            Debug.Log("M is pressed");
            startPlayback = true;
            startTime = Time.time;
        }

        if (startPlayback)
        {
            //if (Time.time > startTime + dataList[currentIndex].timestamp)
            {
                ModifyPosition();
            }
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
        if (currentIndex < dataList.Count)
        {
            //print("currentIndex: " + currentIndex + " at " + Time.time);
            ReplayHeadset.transform.localPosition = dataList[currentIndex].headPos;
            ReplayHeadset.transform.localRotation = dataList[currentIndex].headRotQ;
            ReplayHandR.transform.localPosition = dataList[currentIndex].HandRPos;
            ReplayHandR.transform.localRotation = dataList[currentIndex].handRRotQ;
            ReplayHandL.transform.localPosition = dataList[currentIndex].handLPos;
            ReplayHandL.transform.localRotation = dataList[currentIndex].handLRotQ;
            ReplayTracker.transform.localPosition = dataList[currentIndex].tracker1Pos;
            ReplayTracker.transform.localRotation = dataList[currentIndex].tracker1RotQ;

            if (currentIndex - 11 > 0)
            {
                var beforeScaler = predictionList[currentIndex - 11].tracker1Pos;
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
                
                beforeScaler = predictionList[currentIndex - 11].tracker1Rot;
                afterScaler = Vector3.zero;
                afterScaler.x = _scalers["tracker1Rotx"].InverseTransform(beforeScaler.x);
                afterScaler.y = _scalers["tracker1Roty"].InverseTransform(beforeScaler.y);
                afterScaler.z = _scalers["tracker1Rotz"].InverseTransform(beforeScaler.z);

                PredictedTracker.transform.localEulerAngles = afterScaler;
            }
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
    }

    public void RandomPosition()
    {
        currentIndex = (int) UnityEngine.Random.Range(0, dataList.Count);
        //Debug.Log(currentIndex);
    }
}