using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class DataContainer_Exp0 : DataContainer_Base
{
    public float timestamp;
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


    public override void StringToData(string[] d)
    {
        if (d.Length == 41)
        {
            var i = 0;
            timestamp = float.Parse(d[i++]);
            headPos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            headRot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            headRotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handRPos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handRRot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handRRotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handLPos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handLRot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handLRotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            tracker1Pos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            tracker1Rot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            tracker1RotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));    
        }
        else if (d.Length == 25)
        {
            var i = 0;
            timestamp = float.Parse(d[i++]);
            headPos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            headRot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
//            headRotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handRPos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handRRot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
//            handRRotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handLPos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            handLRot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
//            handLRotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            tracker1Pos = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
            tracker1Rot = new Vector3(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
//            tracker1RotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]));
        }
        else
        {
            Debug.Log(d.Length);    
        }
        
        //41 elements
        
    }

    private static string _headerString;

    public override string ToString()
    {
        return "\n" + this.timestamp + ","
               + this.headPos.x + ","
               + this.headPos.y + ","
               + this.headPos.z + ","
               + this.headRot.x + ","
               + this.headRot.y + ","
               + this.headRot.z + ","
               + this.headRotQ.x + ","
               + this.headRotQ.y + ","
               + this.headRotQ.z + ","
               + this.headRotQ.w + ","
               + this.handRPos.x + ","
               + this.handRPos.y + ","
               + this.handRPos.z + ","
               + this.handRRot.x + ","
               + this.handRRot.y + ","
               + this.handRRot.z + ","
               + this.handRRotQ.x + ","
               + this.handRRotQ.y + ","
               + this.handRRotQ.z + ","
               + this.handRRotQ.w + ","
               + this.handLPos.x + ","
               + this.handLPos.y + ","
               + this.handLPos.z + ","
               + this.handLRot.x + ","
               + this.handLRot.y + ","
               + this.handLRot.z + ","
               + this.handLRotQ.x + ","
               + this.handLRotQ.y + ","
               + this.handLRotQ.z + ","
               + this.handLRotQ.w + ","
               + this.tracker1Pos.x + ","
               + this.tracker1Pos.y + ","
               + this.tracker1Pos.z + ","
               + this.tracker1Rot.x + ","
               + this.tracker1Rot.y + ","
               + this.tracker1Rot.z + ","
               + this.tracker1RotQ.x + ","
               + this.tracker1RotQ.y + ","
               + this.tracker1RotQ.z + ","
               + this.tracker1RotQ.w;
    }

    public static string HeaderToString()
    {
        if (_headerString == null)
        {
            _headerString =
                nameof(DataContainer_Exp0.timestamp) + "," +
                nameof(DataContainer_Exp0.headPos) + "x," +
                nameof(DataContainer_Exp0.headPos) + "y," +
                nameof(DataContainer_Exp0.headPos) + "z," +
                nameof(DataContainer_Exp0.headRot) + "x," +
                nameof(DataContainer_Exp0.headRot) + "y," +
                nameof(DataContainer_Exp0.headRot) + "z," +
                nameof(DataContainer_Exp0.headRotQ) + "x," +
                nameof(DataContainer_Exp0.headRotQ) + "y," +
                nameof(DataContainer_Exp0.headRotQ) + "z," +
                nameof(DataContainer_Exp0.headRotQ) + "w," +
                nameof(DataContainer_Exp0.handRPos) + "x," +
                nameof(DataContainer_Exp0.handRPos) + "y," +
                nameof(DataContainer_Exp0.handRPos) + "z," +
                nameof(DataContainer_Exp0.handRRot) + "x," +
                nameof(DataContainer_Exp0.handRRot) + "y," +
                nameof(DataContainer_Exp0.handRRot) + "z," +
                nameof(DataContainer_Exp0.handRRotQ) + "x," +
                nameof(DataContainer_Exp0.handRRotQ) + "y," +
                nameof(DataContainer_Exp0.handRRotQ) + "z," +
                nameof(DataContainer_Exp0.handRRotQ) + "w," +
                nameof(DataContainer_Exp0.handLPos) + "x," +
                nameof(DataContainer_Exp0.handLPos) + "y," +
                nameof(DataContainer_Exp0.handLPos) + "z," +
                nameof(DataContainer_Exp0.handLRot) + "x," +
                nameof(DataContainer_Exp0.handLRot) + "y," +
                nameof(DataContainer_Exp0.handLRot) + "z," +
                nameof(DataContainer_Exp0.handLRotQ) + "x," +
                nameof(DataContainer_Exp0.handLRotQ) + "y," +
                nameof(DataContainer_Exp0.handLRotQ) + "z," +
                nameof(DataContainer_Exp0.handLRotQ) + "w," +
                nameof(DataContainer_Exp0.tracker1Pos) + "x," +
                nameof(DataContainer_Exp0.tracker1Pos) + "y," +
                nameof(DataContainer_Exp0.tracker1Pos) + "z," +
                nameof(DataContainer_Exp0.tracker1Rot) + "x," +
                nameof(DataContainer_Exp0.tracker1Rot) + "y," +
                nameof(DataContainer_Exp0.tracker1Rot) + "z," +
                nameof(DataContainer_Exp0.tracker1RotQ) + "x," +
                nameof(DataContainer_Exp0.tracker1RotQ) + "y," +
                nameof(DataContainer_Exp0.tracker1RotQ) + "z," +
                nameof(DataContainer_Exp0.tracker1RotQ) + "w,";
        }

        return _headerString;
    }
}