using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

//public delegate void Delegate_NewUser(string name);
public class Manager_DataCollection : MonoBehaviour
{
    private DataCollection_ExpBase currentExperiment;
    //public DataCollection_ExpBase currentExperiment;
    public DataCollection_UserFeatures currentUser;
    private bool _isRecording = false;

    public bool IsRecording()
    {
        return _isRecording;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    public void StartRecording()
    {
        //Dev.Log("Start Recording");
        currentExperiment.StartRecording();
    }

    public void StopRecording()
    {
        currentExperiment.StopRecording();
    }

    private void LateUpdate()
    {
        if (!currentExperiment)
            return;

        //currentExperiment.LateUpdate();        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!currentExperiment)
            return;

//        currentExperiment.Update();
        DebugUpdate();
    }

    private int debugButtonWidth = 200;
    private int debugButtonHeight = 50;
    void OnGUI()
    {
        if (!currentExperiment)
            return;
        
        if (GUI.Button(new Rect(10, 10, debugButtonWidth, debugButtonHeight), "Start Experiment"))
        {
            StartRecording();
        }

        if (GUI.Button(new Rect(10, 110, debugButtonWidth, debugButtonHeight), "Stop Experiment"))
        {
            StartRecording();
        }
        if (GUI.Button(new Rect(10, 210, debugButtonWidth, debugButtonHeight), "Save Experiment"))
        {
            SaveExperimentData();
        }
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartRecording();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopRecording();
        }

        //EVENT_NewUser?.Invoke(s);
        if (Input.GetKeyDown(KeyCode.A))
        {
            SaveExperimentData();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("[Debug] DataCollection: WriteAsJson");
        }
    }

    public void SaveExperimentData()
    {
        Dev.Log("[Debug] DataCollection: WriteAsCsv");
        WriteToFile(currentExperiment);
        //WriteToFile(currentUser);
    }

    private void WriteToFile(IWriteToFile iwtf)
    {
        //var fileName = "/"+currentExperiment.ExpName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".csv";
//        print(currentExperiment.OutputFileName());
//        print(currentExperiment.OutputData());
        var fileName = "/" + iwtf.OutputFileName();
        var fileData = iwtf.OutputData();
        Dev.Log(fileData);
        System.IO.File.WriteAllText(Application.persistentDataPath + fileName, fileData);
        //var userFeaturesFileName = 
        Dev.Log(Application.persistentDataPath);

#if UNITY_EDITOR
        EditorUtility.RevealInFinder(Application.persistentDataPath + fileName);
#endif
    }

    public DataCollection_ExpBase GetCurrentExperiment()
    {
        return this.currentExperiment;
    }

    public string GetGoalString()
    {
        return this.currentExperiment.GetGoalString();
    }

    public int GetTotalEntries()
    {
        return this.currentExperiment.GetTotalEntries();
    }

    // public void ChangeExperimentType(T t)
    // {
    //     return this.currentExperiment.ChangeExperimentType(t);
    // }
    public void RemoveLastEntry()
    {
        this.currentExperiment.RemoveLastEntry();
    }
    
    public void RegisterExperiment(DataCollection_ExpBase newExperiment)
    {
        this.currentExperiment = newExperiment;
    }

    public void RegisterUserData(DataCollection_UserFeatures newUserData)
    {
        this.currentUser = newUserData;
    }


}