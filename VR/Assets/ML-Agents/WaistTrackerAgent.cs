using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors.Reflection;


public class WaistTrackerAgent : Exp0_Agent_Base_Powen
{
    public override void OnEpisodeBegin()
    {
        helper.RandomPosition();
        //print("On Episode Begin: " + Headset.position + " vs " + Headset.localPosition);
        this.transform.localPosition = Headset.localPosition + new Vector3(0, yOffset, 0);
        //print(this.transform.position + " vs " + Headset.position);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Attempt1 & 2
        // Target and Agent positions
        sensor.AddObservation(Headset.localPosition - this.transform.localPosition);
        sensor.AddObservation(Quaternion.Euler(Headset.localRotation.eulerAngles - this.transform.localRotation.eulerAngles));
        sensor.AddObservation(LeftController.localPosition -this.transform.localPosition);
        sensor.AddObservation(Quaternion.Euler(LeftController.localRotation.eulerAngles - this.transform.localRotation.eulerAngles));
        sensor.AddObservation(RightController.localPosition - this.transform.localPosition);
        sensor.AddObservation(Quaternion.Euler(RightController.localRotation.eulerAngles - this.transform.localRotation.eulerAngles));
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.localRotation);
        
        //Attempt 3 Normalized
        // Target and Agent positions
        
        // sensor.AddObservation(NormalizedFloat(Headset.localPosition.x));
        // sensor.AddObservation(NormalizedFloat(Headset.localPosition.y));
        // sensor.AddObservation(NormalizedFloat(Headset.localPosition.z)); 
        // var normalizedRotation1 = NormalizeRotation(Headset.localRotation);
        // sensor.AddObservation(normalizedRotation1);
        //
        // sensor.AddObservation(LeftController.localPosition);
        // var normalizedRotation2 = NormalizeRotation(LeftController.localRotation);
        // sensor.AddObservation(normalizedRotation2);
        //
        // sensor.AddObservation(RightController.localPosition);
        // var normalizedRotation3 = NormalizeRotation(RightController.localRotation);
        // sensor.AddObservation(normalizedRotation3);
        //
        // sensor.AddObservation(this.transform.localPosition);
        // var normalizedRotation4 = NormalizeRotation(transform.localRotation);
        // sensor.AddObservation(normalizedRotation4);
    }

    // private float NormalizedFloat(float currentValue)
    // {
    //     
    //     var normalizedValue = (currentValue - minValue) / (maxValue - minValue);
    //     return normalizedValue;
    // }


    // private Vector3 NormalizePosition(int i, Vector3 position)
    // {
    //     var normalizedVector = Vector3.zero; 
    //     normalizedVector3.x = (position.x - )
    // }

    private Vector3 NormalizeRotation(Quaternion rotation)
    {
        Vector3 normalized = rotation.eulerAngles / 180.0f - Vector3.one; //[-1, 1]
        //Vector3 normalized = rotation.eulerAngles / 360.0f;  // [0,1]
        return normalized;
    }

    public float forceMultiplier = 1;

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.y = actionBuffers.ContinuousActions[1];
        controlSignal.z = actionBuffers.ContinuousActions[2];
        this.transform.localPosition += controlSignal * forceMultiplier;
        
        // Rewards
        AssignReward();
    }
}