using System;
using UnityEngine;

[Serializable]
public class DataContainer_Base
{
    public virtual void StringToData(string[] d)
    {
    }

    public virtual string ToString()
    {
        return "";
    }

    public virtual string HeaderToString()
    {
        return "";
    }
}