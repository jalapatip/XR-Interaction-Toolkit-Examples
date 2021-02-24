using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DataCollection_ExpInterface
{
    string OutputFileName();
    string OutputData();
    bool IsRecording();
    void StartRecording();
    void StopRecording();
    void Update();
    void LateUpdate();
}
public class DataCollection_ExpBase : MonoBehaviour, IWriteToFile
{
    public string ExpName = "ExpX (Default Value)";

    public virtual string OutputFileName()
    {
        return ExpName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".csv";
    }

    public virtual string OutputData()
    {
        return "ExpBase Default Data";
    }
    
    protected bool _isRecording = false;
    
    public bool IsRecording()
    {
        return _isRecording;
    }
    
    public virtual void StartRecording()
    {
        Dev.Log("Start Recording");
        _isRecording = true;
    }

    public virtual void StopRecording()
    {
        Dev.Log("Stop Recording");
        _isRecording = false;
    }

    public virtual void Update()
    {
        
    }

    public virtual void LateUpdate()
    {
        
    }
}
