using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors.Reflection;

public class WaistTrackerAgentStackedVector : Agent
{
    public Exp0Powen_Reward currentAttempt;
    
    //This would probably makes more sense as a factor of the user's height. for example. 0.58*user height
    public float yOffset = -0.8f;
    void Start()
    {
    }

    public DataReplayHelper helper;
    public Transform Headset;
    public Transform LeftController;
    public Transform RightController;
    public Transform Waist;

    public static  int stackedNumber = 10;
    public override void OnEpisodeBegin()
    {
        // Move the target to a new spot
        // Target.localPosition = new Vector3(Random.value * 8 - 4,
        //     0.5f,
        //     Random.value * 8 - 4);
        helper.RandomPosition();
        //print("On Episode Begin: " + Headset.position + " vs " + Headset.localPosition);
        this.transform.localPosition = Headset.localPosition + new Vector3(0, yOffset, 0);
        //print(this.transform.position + " vs " + Headset.position);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnEpisodeBegin();    
        }
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
        // Target and Agent positions
        // sensor.AddObservation(Headset.localPosition);
        // sensor.AddObservation(Headset.localRotation);
        // sensor.AddObservation(LeftController.localPosition);
        // sensor.AddObservation(LeftController.localRotation);
        // sensor.AddObservation(RightController.localPosition);
        // sensor.AddObservation(RightController.localRotation);
        // sensor.AddObservation(this.transform.localPosition);
        // sensor.AddObservation(this.transform.localRotation);

        sensor.AddObservation(LocalHeadPos);
        sensor.AddObservation(LocalHeadRot);
        sensor.AddObservation(LocalLeftPos);
        sensor.AddObservation(LocalLeftRot);
        sensor.AddObservation(LocalRightPos);
        sensor.AddObservation(LocalRightRot);
        sensor.AddObservation(LocalAgentPos);
        sensor.AddObservation(LocalAgentRot);

        // Agent velocity
        //sensor.AddObservation(rBody.velocity.x);
        //sensor.AddObservation(rBody.velocity.y);
        //sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 1;

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.y = actionBuffers.ContinuousActions[1];
        controlSignal.z = actionBuffers.ContinuousActions[2];
        // rBody.AddForce(controlSignal * forceMultiplier);
        this.transform.localPosition += controlSignal * forceMultiplier;
        
        // Rewards
        var distanceToTarget = Vector3.Distance(this.transform.localPosition, Waist.localPosition);


        switch (currentAttempt)
        {
            case Exp0Powen_Reward.RewardAttempt1:
                Attempt1(distanceToTarget);
                break;
            case Exp0Powen_Reward.RewardAttempt2:
                Attempt2(distanceToTarget);
                break;
            case Exp0Powen_Reward.RewardAttempt3:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Attempt1(float distanceToTarget)
    {
        // Reached target
        if (distanceToTarget < 0.3f)
        {
            SetReward(2*(1-distanceToTarget));
        }
        if (distanceToTarget < 0.5f)
        {
            SetReward(1-distanceToTarget);
        }
        else if (distanceToTarget < 1f)
        {
            SetReward(-distanceToTarget);
        }
        else 
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
    
    //2130 s with CPU inference device
    //2150 s with GPU inference device
    //2555 s wiith burst inference device
    private void Attempt2(float distanceToTarget)
    {
        // Reached target
        if (distanceToTarget < 0.3f)
        {
            SetReward(2*(1-distanceToTarget));
        }
        if (distanceToTarget < 0.5f)
        {
            SetReward(1-distanceToTarget);
        }
        else if (distanceToTarget < 1f)
        {
            SetReward(-distanceToTarget);
        }
        else 
        {
            SetReward(-100f);
            EndEpisode();
        }
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        //continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[2] = Input.GetAxis("Vertical");
    }
}