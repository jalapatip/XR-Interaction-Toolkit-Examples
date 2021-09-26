using System;
using UnityEngine;

[Serializable]
public class DataContainer_Exp0PredictionA2 : DataContainer_Base
{
    //public float timestamp;
    public Vector3 tracker1Pos;
    public Vector3 tracker1Rot;
    public Quaternion tracker1RotQ;


    public override void StringToData(string[] d)
    {
        var i = 19 + 7;
        //timestamp = float.Parse(d[i++]);
        tracker1Pos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
        tracker1RotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]),
            float.Parse(d[i++]));
    }

    private static string _headerString;

    public override string ToString()
    {
        return "\n" // + this.timestamp + ","
               + this.tracker1Pos.x + ","
               + this.tracker1Pos.y + ","
               + this.tracker1Pos.z + ","
               + this.tracker1RotQ.x + ","
               + this.tracker1RotQ.y + ","
               + this.tracker1RotQ.z + ","
               + this.tracker1RotQ.w + ",";
    }

    //public static string HeaderToString()
    public new static string HeaderToString()
    {
        if (_headerString == null)
        {
            _headerString =
                //nameof(DataContainer_Exp0Prediction.timestamp) + "," +
                nameof(DataContainer_Exp0Prediction.tracker1Pos) + "x," +
                nameof(DataContainer_Exp0Prediction.tracker1Pos) + "y," +
                nameof(DataContainer_Exp0Prediction.tracker1Pos) + "z," +
                nameof(DataContainer_Exp0Prediction.tracker1RotQ) + "x," +
                nameof(DataContainer_Exp0Prediction.tracker1RotQ) + "y," +
                nameof(DataContainer_Exp0Prediction.tracker1RotQ) + "z," +
                nameof(DataContainer_Exp0Prediction.tracker1RotQ) + "w,";
        }

        return _headerString;
    }
}