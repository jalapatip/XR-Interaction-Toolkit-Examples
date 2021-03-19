using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

public enum Exp0Powen_Reward
{
    RewardAttempt1,
    RewardAttempt2,
    RewardAttempt3,
    RewardAttempt4,
}
public class Exp0_Agent_Base_Powen : Agent
{
    [FormerlySerializedAs("currentAttempt")]
    public Exp0Powen_Reward currentReward;
    
    //This would probably makes more sense as a factor of the user's height. for example. 0.58*user height
    public float yOffset = -0.8f;
    
    public DataReplayHelper helper;
    public Transform Headset;
    public Transform LeftController;
    public Transform RightController;
    public Transform Waist;

    public void AssignReward()
    {
        var distanceToTarget = Vector3.Distance(this.transform.localPosition, Waist.localPosition);
        
        switch (currentReward)
        {
            case Exp0Powen_Reward.RewardAttempt1:
                RewardAttempt1(distanceToTarget);
                break;
            case Exp0Powen_Reward.RewardAttempt2:
                RewardAttempt2(distanceToTarget);
                break;
            case Exp0Powen_Reward.RewardAttempt3:
                RewardAttempt3(distanceToTarget);
                break;
            case Exp0Powen_Reward.RewardAttempt4:
                RewardAttempt4(distanceToTarget);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void RewardAttempt1(float distanceToTarget)
    {
        // Reached target
        if (distanceToTarget < 0.2f)
        {
            //1~0.7
            SetReward((1-distanceToTarget));
        }
        if (distanceToTarget < 0.5f)
        {
            //
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
    public void RewardAttempt2(float distanceToTarget)
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
    
    public void RewardAttempt3(float distanceToTarget)
    {
        // Reached target
        if (distanceToTarget < 0.3f)
        {
            SetReward((1-distanceToTarget));
        }
        if (distanceToTarget < 0.5f)
        {
            SetReward((1-distanceToTarget)/2);
        }
        else if (distanceToTarget < 1f)
        {
            SetReward(-distanceToTarget/10);
        }
        else 
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
    public void RewardAttempt4(float distanceToTarget)
    {
        // Reached target
        if (distanceToTarget < 0.1f)
        {
            SetReward((1-distanceToTarget));
        }
        if (distanceToTarget < 0.3f)
        {
            SetReward((1-distanceToTarget)/4);
        }
        else if (distanceToTarget < 1f)
        {
            SetReward(-distanceToTarget/10);
        }
        else 
        {
            SetReward(-1f);
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
