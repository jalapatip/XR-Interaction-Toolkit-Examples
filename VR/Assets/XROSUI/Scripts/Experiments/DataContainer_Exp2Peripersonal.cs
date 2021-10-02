using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class DataContainer_ExpSmarthome : DataContainer_Base
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

    [FormerlySerializedAs("target")]
    public string targetType;

    public int targetId;
    public String utterance;

    public override void StringToData(string[] d)
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
        tracker1RotQ = new Quaternion(float.Parse(d[i++]), float.Parse(d[i++]), float.Parse(d[i++]),
            float.Parse(d[i++]));
        targetType = d[i++];
        targetId = int.Parse(d[i++]);
        utterance = d[i];
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
               + this.tracker1RotQ.w + ","
               + this.targetType + ","
               + this.targetId + ","
               + this.utterance + ",";
    }

    //public static string HeaderToString()
    public new static string HeaderToString()
    {
        if (_headerString == null)
        {
            _headerString =
                nameof(DataContainer_ExpSmarthome.timestamp) + "," +
                nameof(DataContainer_ExpSmarthome.headPos) + "x," +
                nameof(DataContainer_ExpSmarthome.headPos) + "y," +
                nameof(DataContainer_ExpSmarthome.headPos) + "z," +
                nameof(DataContainer_ExpSmarthome.headRot) + "x," +
                nameof(DataContainer_ExpSmarthome.headRot) + "y," +
                nameof(DataContainer_ExpSmarthome.headRot) + "z," +
                nameof(DataContainer_ExpSmarthome.headRotQ) + "x," +
                nameof(DataContainer_ExpSmarthome.headRotQ) + "y," +
                nameof(DataContainer_ExpSmarthome.headRotQ) + "z," +
                nameof(DataContainer_ExpSmarthome.headRotQ) + "w," +
                nameof(DataContainer_ExpSmarthome.handRPos) + "x," +
                nameof(DataContainer_ExpSmarthome.handRPos) + "y," +
                nameof(DataContainer_ExpSmarthome.handRPos) + "z," +
                nameof(DataContainer_ExpSmarthome.handRRot) + "x," +
                nameof(DataContainer_ExpSmarthome.handRRot) + "y," +
                nameof(DataContainer_ExpSmarthome.handRRot) + "z," +
                nameof(DataContainer_ExpSmarthome.handRRotQ) + "x," +
                nameof(DataContainer_ExpSmarthome.handRRotQ) + "y," +
                nameof(DataContainer_ExpSmarthome.handRRotQ) + "z," +
                nameof(DataContainer_ExpSmarthome.handRRotQ) + "w," +
                nameof(DataContainer_ExpSmarthome.handLPos) + "x," +
                nameof(DataContainer_ExpSmarthome.handLPos) + "y," +
                nameof(DataContainer_ExpSmarthome.handLPos) + "z," +
                nameof(DataContainer_ExpSmarthome.handLRot) + "x," +
                nameof(DataContainer_ExpSmarthome.handLRot) + "y," +
                nameof(DataContainer_ExpSmarthome.handLRot) + "z," +
                nameof(DataContainer_ExpSmarthome.handLRotQ) + "x," +
                nameof(DataContainer_ExpSmarthome.handLRotQ) + "y," +
                nameof(DataContainer_ExpSmarthome.handLRotQ) + "z," +
                nameof(DataContainer_ExpSmarthome.handLRotQ) + "w," +
                nameof(DataContainer_ExpSmarthome.tracker1Pos) + "x," +
                nameof(DataContainer_ExpSmarthome.tracker1Pos) + "y," +
                nameof(DataContainer_ExpSmarthome.tracker1Pos) + "z," +
                nameof(DataContainer_ExpSmarthome.tracker1Rot) + "x," +
                nameof(DataContainer_ExpSmarthome.tracker1Rot) + "y," +
                nameof(DataContainer_ExpSmarthome.tracker1Rot) + "z," +
                nameof(DataContainer_ExpSmarthome.tracker1RotQ) + "x," +
                nameof(DataContainer_ExpSmarthome.tracker1RotQ) + "y," +
                nameof(DataContainer_ExpSmarthome.tracker1RotQ) + "z," +
                nameof(DataContainer_ExpSmarthome.tracker1RotQ) + "w," +
                nameof(DataContainer_ExpSmarthome.targetType) + "," +
                nameof(DataContainer_ExpSmarthome.targetId) + "," +
                nameof(DataContainer_ExpSmarthome.utterance);
            ;
        }

        return _headerString;
    }
}