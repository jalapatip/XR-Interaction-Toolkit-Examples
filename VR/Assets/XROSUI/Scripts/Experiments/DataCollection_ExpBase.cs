using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 
/// </summary>
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

/// <summary>
/// 
/// </summary>
public class DataCollection_ExpBase : MonoBehaviour, IWriteToFile
{
    //public string ExpName = "ExpX (Default Value)";
    public virtual string ExpName { get; set; } = "ExpX (Default Value)";
    
    //private List<DataContainer_Base> dataList = new List<DataContainer_Base>();
    //protected List<DataContainer_Base> dataList = new List<DataContainer_Base>();

    private List<DataContainer_Base> _list = new List<DataContainer_Base>();
    
    protected List<DataContainer_Base> dataList
    {
        get => _list;
        set => _list = value;
    }
    
    public virtual string OutputFileName()
    {
        return ExpName + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".csv";
    }

    public virtual string OutputData()
    {
        var sb = new StringBuilder();
        sb.Append(this.OutputHeaderString());
        foreach (var d in dataList)
        {
            sb.Append(d.ToString());
        }
        return sb.ToString();
    }

    protected bool _isRecording = false;
    
    public virtual string GetGoalString()
    {
        return "Goal Not Set";
    }

    public virtual string OutputHeaderString()
    {
        return "override this function";
    }

    public virtual int GetTotalEntries()
    {
        return dataList.Count;
    }

    public virtual void RemoveLastEntry()
    {
        dataList.RemoveAt(dataList.Count);
    }

    #region stable
    public bool IsRecording()
    {
        return _isRecording;
    }
    
    public virtual void StartRecording()
    {
        Dev.Log("Start Recording " + ExpName);
        _isRecording = true;
    }

    public virtual void StopRecording()
    {
        Dev.Log("Stop Recording" + ExpName);
        _isRecording = false;
    }
    
    private string CompileDataAsJson()
    {
        var expData = JsonUtility.ToJson(dataList);
        return expData;
    }
    #endregion stable
    
    public virtual void Update()
    {
        
    }

    public virtual void LateUpdate()
    {
        
    }

    public virtual void SaveExperimentData()
    {
        Core.Ins.DataCollection.SaveGeneralData(this);
    }
}