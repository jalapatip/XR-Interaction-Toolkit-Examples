using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors.Reflection;

public class WaistTrackerAgent_Rigidbody : Exp0_Agent_Base_Powen
{
    public Rigidbody rBody;
    
    public override void OnEpisodeBegin()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        // Move the target to a new spot
        // Target.localPosition = new Vector3(Random.value * 8 - 4,
        //     0.5f,
        //     Random.value * 8 - 4);
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
            return Headset.localPosition;
        }
    }

    [Observable(numStackedObservations: 10)]
    Quaternion LocalHeadRot
    {
        get
        {
            return Headset.localRotation;
        }
    }
    
    [Observable(numStackedObservations: 10)]
    Vector3 LocalLeftPos
    {
        get
        {
            return LeftController.localPosition;
        }
    }

    [Observable(numStackedObservations: 10)]
    Quaternion LocalLeftRot
    {
        get
        {
            return LeftController.localRotation;
        }
    }
    
    [Observable(numStackedObservations: 10)]
    Vector3 LocalRightPos
    {
        get
        {
            return RightController.localPosition;
        }
    }

    [Observable(numStackedObservations: 10)]
    Quaternion LocalRightRot
    {
        get
        {
            return RightController.localRotation;
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
        //Atempt1 & 2
        // Target and Agent positions
        sensor.AddObservation(Headset.localPosition);
        sensor.AddObservation(Headset.localRotation);
        sensor.AddObservation(LeftController.localPosition);
        sensor.AddObservation(LeftController.localRotation);
        sensor.AddObservation(RightController.localPosition);
        sensor.AddObservation(RightController.localRotation);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.localRotation);
        
        //Attempt 2 stacked
        // sensor.AddObservation(LocalHeadPos);
        // sensor.AddObservation(LocalHeadRot);
        // sensor.AddObservation(LocalLeftPos);
        // sensor.AddObservation(LocalLeftRot);
        // sensor.AddObservation(LocalRightPos);
        // sensor.AddObservation(LocalRightRot);
        // sensor.AddObservation(LocalAgentPos);
        // sensor.AddObservation(LocalAgentRot);

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

        
        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.y);
        sensor.AddObservation(rBody.velocity.z);
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
         rBody.AddForce(controlSignal * forceMultiplier);
        //this.transform.localPosition += controlSignal * forceMultiplier;
        
        // Rewards
        var distanceToTarget = Vector3.Distance(this.transform.localPosition, Waist.localPosition);

        AssignReward();
    }
}