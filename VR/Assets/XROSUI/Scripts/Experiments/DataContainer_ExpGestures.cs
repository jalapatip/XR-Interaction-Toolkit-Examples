using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public struct DataContainer_ExpGesturesPosition
{
    public Vector3 headPos;
    public Vector3 headRot; //Euler Angles
    public Quaternion headRotQ; //Quaternion
    public Vector3 handRPos;
    public Vector3 handRRot;
    public Quaternion handRRotQ;
    public Vector3 handLPos;
    public Vector3 handLRot;
    public Quaternion handLRotQ;
}

[Serializable]
public class DataContainer_ExpGestures
{
    public List<DataContainer_ExpGesturesPosition> positions;
    public Gesture gesture;
    private static string _headerString;
    
    public override string ToString()
    {
        var toReturn = "";
        foreach (var position in this.positions)
        {
            toReturn += position.headPos.x + "," + position.headPos.y + "," + position.headPos.z + ","
                        + position.headRot.x + "," + position.headRot.y + "," + position.headRot.z + ","
                        + position.headRotQ.x + "," + position.headRotQ.y + "," + position.headRotQ.z + "," + position.headRotQ.w + ","
                        + position.handRPos.x + "," + position.handRPos.y + "," + position.handRPos.z + ","
                        + position.handRRot.x + "," + position.handRRot.y + "," + position.handRRot.z + ","
                        + position.handRRotQ.x + "," + position.handRRotQ.y + "," + position.handRRotQ.z + "," + position.handRRotQ.w + ","
                        + position.handLPos.x + "," + position.handLPos.y + "," + position.handLPos.z + ","
                        + position.handLRot.x + "," + position.handLRot.y + "," + position.handLRot.z + ","
                        + position.handLRotQ.x + "," + position.handLRotQ.y + "," + position.handLRotQ.z + "," + position.handLRotQ.w + ",";
        }

        toReturn += gesture;
        return toReturn;
    }
    
    public static string HeaderToString()
    {
        if (_headerString == null)
        {
            _headerString = "";
            for (int index = 0; index < DataCollection_ExpGestures.samplesPerGesture; index++)
            {
                _headerString += nameof(DataContainer_ExpGesturesPosition.headPos) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headPos) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headPos) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headRot) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headRot) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headRot) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headRotQ) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headRotQ) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headRotQ) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.headRotQ) + "w" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRPos) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRPos) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRPos) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRRot) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRRot) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRRot) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRRotQ) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRRotQ) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRRotQ) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handRRotQ) + "w" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLPos) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLPos) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLPos) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLRot) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLRot) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLRot) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLRotQ) + "x" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLRotQ) + "y" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLRotQ) + "z" + index + ","
                                 + nameof(DataContainer_ExpGesturesPosition.handLRotQ) + "w" + index + ",";
            }

            _headerString += nameof(gesture);
        }
        return _headerString;
    }
}