using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors.Reflection;

public class WaistTrackerAgent_StackedVector : Exp0_Agent_Base_Powen
{
    public override void OnEpisodeBegin()
    {
        helper.RandomPosition();
        //print("On Episode Begin: " + Headset.position + " vs " + Headset.localPosition);
        this.transform.localPosition = Headset.localPosition + new Vector3(0, yOffset, 0);
        //print(this.transform.position + " vs " + Headset.position);
    }

    [Observable(numStackedObservations: 10)]
    Vector3 LocalHeadPos
    {
        get
        {
            return Headset.localPosition - transform.localPosition;
        }
    }

    [Observable(numStackedObservations: 10)]
    Quaternion LocalHeadRot
    {
        get
        {
            
            return Quaternion.Euler(Headset.localRotation.eulerAngles - transform.localRotation.eulerAngles);
        }
    }
    
    [Observable(numStackedObservations: 10)]
    Vector3 LocalLeftPos
    {
        get
        {
            return LeftController.localPosition - transform.localPosition;
        }
    }

    [Observable(numStackedObservations: 10)]
    Quaternion LocalLeftRot
    {
        get
        {
            return Quaternion.Euler(LeftController.localRotation.eulerAngles - transform.localRotation.eulerAngles);
        }
    }
    
    [Observable(numStackedObservations: 10)]
    Vector3 LocalRightPos
    {
        get
        {
            return RightController.localPosition - transform.localPosition;
        }
    }

    [Observable(numStackedObservations: 10)]
    Quaternion LocalRightRot
    {
        get
        {
            return Quaternion.Euler(RightController.localRotation.eulerAngles - transform.localRotation.eulerAngles);
        }
    }
    
    [Observable(numStackedObservations: 10)]
    Vector3 LocalAgentPos
    {
        get
        {
            return transform.localPosition;
        }
    }

    [Observable(numStackedObservations: 10)]
    Quaternion LocalAgentRot
    {
        get
        {
            return transform.localRotation;
        }
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        //Attempt1 & 2
        // Target and Agent positions
        // sensor.AddObservation(Headset.localPosition);
        // sensor.AddObservation(Headset.localRotation);
        // sensor.AddObservation(LeftController.localPosition);
        // sensor.AddObservation(LeftController.localRotation);
        // sensor.AddObservation(RightController.localPosition);
        // sensor.AddObservation(RightController.localRotation);
        // sensor.AddObservation(this.transform.localPosition);
        // sensor.AddObservation(this.transform.localRotation);
        
        //Attempt 2 stacked
        sensor.AddObservation(LocalHeadPos);
        sensor.AddObservation(LocalHeadRot);
        sensor.AddObservation(LocalLeftPos);
        sensor.AddObservation(LocalLeftRot);
        sensor.AddObservation(LocalRightPos);
        sensor.AddObservation(LocalRightRot);
        sensor.AddObservation(LocalAgentPos);
        sensor.AddObservation(LocalAgentRot);
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