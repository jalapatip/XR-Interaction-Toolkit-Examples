using System;
using UnityEngine;

[Serializable]
public class DataContainer_Base
{
    //This takes a string of input to convert for use in Unity (e.g. assign 0.5 to Position.x)
    public virtual void StringToData(string[] d)
    {
    }

    //This takes Unity values and convert it to a string for the CSV file (e.g 0.5, 0.8, 0.3)
    public override string ToString()
    {
        return "";
    }

    //The content of this method should specify a string of the header to be used in the CSV file (e.g. HandPosX, HandPosY)
    public virtual string HeaderToString()
    {
        return "";
    }
}