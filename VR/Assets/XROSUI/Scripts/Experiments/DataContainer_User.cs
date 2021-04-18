using System;
using UnityEngine;

[Serializable]
public class DataContainer_User 
{
    public float height;
    public float LarmLength;
    public float RarmLength;
    public float Lshoulderx;
    public float Rshoulderx;
    public float Lshouldery;
    public float Rshouldery;
    public float chestWidth;
    public float Lelbowy;
    public float Relbowy;
    public float Lkneey;
    public float Rkneey;
    public string data;
    public string JSONdata;
    
    public Vector3 headPos;
    public Vector3 headRot; //Euler Angles
    public Quaternion headRotQ; //Quaternion
    public Vector3 handRPos { get; set; }
    public Vector3 handRRot;
    public Quaternion handRRotQ;
    public Vector3 handLPos;
    public Vector3 handLRot;
    public Quaternion handLRotQ;
    public Vector3 tracker1Pos;
    public Vector3 tracker1Rot;
    public Quaternion tracker1RotQ;
    public String gesture;
    public String userID;

    public void StringToData(string[] d)
    {
        var i = 0;
        height = float.Parse(d[i++]);
        LarmLength = float.Parse(d[i++]);
        RarmLength = float.Parse(d[i++]);
        Lshoulderx = float.Parse(d[i++]);
        Lshouldery = float.Parse(d[i++]);
        Rshoulderx = float.Parse(d[i++]);
        Rshouldery = float.Parse(d[i++]);
        chestWidth = float.Parse(d[i++]);
        Lelbowy = float.Parse(d[i++]);
        Relbowy = float.Parse(d[i++]);
        Lkneey = float.Parse(d[i++]);
        Rkneey = float.Parse(d[i]);
    }
    public override string ToString()
    {
        data = "\n" + this.height + "," +
               this.LarmLength + "," +
               this.RarmLength + "," +
               this.Lshoulderx + "," +
               this.Lshouldery + "," +
               this.Rshoulderx + "," +
               this.Rshouldery + "," +
               this.chestWidth + "," +
               this.Lelbowy + "," +
               this.Relbowy + "," +
               this.Lkneey + "," +
               this.Rkneey;
        return data;
    }
    private static string _headerString;
    public static string HeaderToString()
    {
        if (_headerString == null)
        {
            _headerString = nameof(DataContainer_User.height) + "," +
                            nameof(DataContainer_User.LarmLength) + "," +
                            nameof(DataContainer_User.RarmLength) + "," +
                            nameof(DataContainer_User.Lshoulderx) + "," +
                            nameof(DataContainer_User.Lshouldery) + "," +
                            nameof(DataContainer_User.Rshoulderx) + "," +
                            nameof(DataContainer_User.Rshouldery) + "," +
                            nameof(DataContainer_User.chestWidth) + "," +
                            nameof(DataContainer_User.Lelbowy) + "," +
                            nameof(DataContainer_User.Relbowy) + "," +
                            nameof(DataContainer_User.Lkneey) + "," +
                            nameof(DataContainer_User.Rkneey);

        }
        return _headerString;
    }
    
    public static DataContainer_User CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<DataContainer_User>(jsonString);
    }

    public string ConvertToJSON()
    {
        this.JSONdata = JsonUtility.ToJson(this);
        return JSONdata;
    }
}