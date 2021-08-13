using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class DataCollection_UserFeatures : MonoBehaviour, IWriteToFile
{
    private DataContainer_User userFeatures = new DataContainer_User();
    private string UserName = "Mark";
    //private string calibrationFileName = "usercalibration.json"; //new edit

   
    private void Start() 
    {
       //UserName = "Mark";
       // ReloadXrDevices();
    }
    /*public void checkCalibrationFile() 
    {
            //string calibrationFileName = UserName + "_usercalibration.json"; //need some way to know which user it belongs to.
            //if (System.IO.File.Exists(Application.persistentDataPath + "/" + calibrationFileName)) 
            //{
                userFeatures = JsonUtility.FromJson<DataContainer_User>(Application.persistentDataPath + "/" + "usercalibration.json");
            //}
            //else
            //{
             //go through calibration process
            //}
    }*/

   

    public string OutputFileName()
    {
        return UserName + "_calibrationData_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".csv";
    }

    public string OutputData()
    {
        print("output data");
        string jsonAsString = File.ReadAllText(Application.persistentDataPath + "/" + "usercalibration.json");
        print(jsonAsString);
        userFeatures = JsonUtility.FromJson<DataContainer_User>(jsonAsString);
        var sb = new StringBuilder();
        sb.Append(DataContainer_User.HeaderToString());
        sb.Append(userFeatures.ToString());
        return sb.ToString();
    }
}
