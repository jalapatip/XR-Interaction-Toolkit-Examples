using System;
using UnityEngine;

[Serializable]
public class DataContainer_Exp0
{
    public float timestamp;
    public Vector3 headPos;
    public Vector3 headRot; //Euler Angles
    public Quaternion headRotQ; //Quaternion
    public Vector3 HandRPos { get; set; }
    public Vector3 handRRot;
    public Quaternion handRRotQ;
    public Vector3 handLPos;
    public Vector3 handLRot;
    public Quaternion handLRotQ;
    public Vector3 tracker1Pos;
    public Vector3 tracker1Rot;
    public Quaternion tracker1RotQ;


    public void StringToData(string[] d)
    {
        timestamp = float.Parse(d[1]);
        headPos = new Vector3(float.Parse(d[2]), float.Parse(d[3]), float.Parse(d[4]));
        headRot = new Vector3(float.Parse(d[5]), float.Parse(d[6]), float.Parse(d[7]));
        headRotQ = new Quaternion(float.Parse(d[8]), float.Parse(d[9]), float.Parse(d[10]), float.Parse(d[11]));
        HandRPos = new Vector3(float.Parse(d[12]), float.Parse(d[13]), float.Parse(d[14]));
        handRRot = new Vector3(float.Parse(d[15]), float.Parse(d[16]), float.Parse(d[17]));
        handRRotQ = new Quaternion(float.Parse(d[18]), float.Parse(d[19]), float.Parse(d[20]), float.Parse(d[21]));
        handLPos = new Vector3(float.Parse(d[22]), float.Parse(d[23]), float.Parse(d[24]));
        handLRot = new Vector3(float.Parse(d[25]), float.Parse(d[26]), float.Parse(d[27]));
        handLRotQ = new Quaternion(float.Parse(d[28]), float.Parse(d[29]), float.Parse(d[30]), float.Parse(d[31]));
        tracker1Pos = new Vector3(float.Parse(d[32]), float.Parse(d[33]), float.Parse(d[34]));
        tracker1Rot = new Vector3(float.Parse(d[35]), float.Parse(d[36]), float.Parse(d[37]));
        tracker1RotQ = new Quaternion(float.Parse(d[38]), float.Parse(d[39]), float.Parse(d[40]), float.Parse(d[41]));
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
                      + this.HandRPos.x + ","
                      + this.HandRPos.y + ","
                      + this.HandRPos.z + ","
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
                      nameof(DataContainer_Exp0.HandRPos) + "x," +
                      nameof(DataContainer_Exp0.HandRPos) + "y," +
                      nameof(DataContainer_Exp0.HandRPos) + "z," +
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