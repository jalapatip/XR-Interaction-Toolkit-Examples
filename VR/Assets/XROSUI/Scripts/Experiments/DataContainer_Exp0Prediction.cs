using System;
using UnityEngine;

[Serializable]
public class DataContainer_Exp0Prediction : DataContainer_Base
{
    //public float timestamp;
    public Vector3 tracker1Pos;
    public Vector3 tracker1Rot;
    public Quaternion tracker1RotQ;


    public override void StringToData(string[] d)
    {
        var i = 0;
        //timestamp = float.Parse(d[i++]);
        tracker1Pos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
        tracker1Rot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
    }

    private static string _headerString;

    public override string ToString()
    {
        return "\n"// + this.timestamp + ","
               + this.tracker1Pos.x + ","
               + this.tracker1Pos.y + ","
               + this.tracker1Pos.z + ","
               + this.tracker1Rot.x + ","
               + this.tracker1Rot.y + ","
               + this.tracker1Rot.z + ",";
    }

    public static string HeaderToString()
    {
        if (_headerString == null)
        {
            _headerString =
                //nameof(DataContainer_Exp0Prediction.timestamp) + "," +
                nameof(DataContainer_Exp0Prediction.tracker1Pos) + "x," +
                nameof(DataContainer_Exp0Prediction.tracker1Pos) + "y," +
                nameof(DataContainer_Exp0Prediction.tracker1Pos) + "z," +
                nameof(DataContainer_Exp0Prediction.tracker1Rot) + "x," +
                nameof(DataContainer_Exp0Prediction.tracker1Rot) + "y," +
                nameof(DataContainer_Exp0Prediction.tracker1Rot) + "z,";
        }

        return _headerString;
    }
}