using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This interface helps us prepare data to be written by FileWriter.
///
/// e.g.
///         var fileName = "/" + iwtf.OutputFileName();
///        var fileData = iwtf.OutputData();
///        Dev.Log(fileData);
///        System.IO.File.WriteAllText(Application.persistentDataPath + fileName, fileData);
/// </summary>
public interface IWriteToFile
{   
    string OutputFileName();
    string OutputData();   
}
