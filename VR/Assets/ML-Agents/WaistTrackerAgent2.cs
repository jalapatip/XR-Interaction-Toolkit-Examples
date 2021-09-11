using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class WaistTrackerAgent2 : Agent
{
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
    const int KNoAction = 0;  // do nothing!
    const int KUp = 1;
    const int KDown = 2;
    const int KLeft = 3;
    const int KRight = 4;
    const int KIn = 5;
    const int KOut = 6;
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

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Headset.localPosition);
        sensor.AddObservation(Headset.localRotation);
        sensor.AddObservation(LeftController.localPosition);
        sensor.AddObservation(LeftController.localRotation);
        sensor.AddObservation(RightController.localPosition);
        sensor.AddObservation(RightController.localRotation);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.localRotation);

        // Agent velocity
        //sensor.AddObservation(rBody.velocity.x);
        //sensor.AddObservation(rBody.velocity.y);
        //sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 0.005f;
    public float distance = 100.0f;
    float[] aDistnace = {100.0f,100.0f,100.0f,100.0f,100.0f,100.0f,100.0f,100.0f,100.0f,100.0f};
    //private float sum = 0.0f;

    public int count = 0;
   /* public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.y = actionBuffers.ContinuousActions[1];
        controlSignal.z = actionBuffers.ContinuousActions[2];
        // rBody.AddForce(controlSignal * forceMultiplier);
        this.transform.localPosition += controlSignal * forceMultiplier;
        
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Waist.localPosition);

        // Reached target
        if (distanceToTarget > 1.5f) 
        {
            AddReward(-1.0f);
            EndEpisode();
        }
        else if (distanceToTarget > 0.5f) 
        {
            AddReward(-0.9f);
        }
        else if (distanceToTarget > 0.25f)
        {
            AddReward(-0.5f);
        }
        else if (distanceToTarget == 0.0f)
        {
            AddReward(1.0f);
            //EndEpisode();
        }
        else if (distanceToTarget < 0.05f)
        {
            AddReward(0.95f);
        }
        else if (distanceToTarget < 0.1f)
        {
            AddReward(0.9f);
        }
        else if (distanceToTarget < 0.25f)
        {
            AddReward(0.5f);
        }
        else
        {
            //SetReward(-1.0f);
            EndEpisode();
        }
    }*/
   
    
    /*public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        float multiplier = 0.0f;
        
   
        var action = actionBuffers.DiscreteActions[0];
        var force = actionBuffers.DiscreteActions[1];

        //var targetPos = transform.position;
        switch (action)
        {
            case KNoAction:
                // do nothing
                break;
            case KRight:
                controlSignal.x = 1;
                break;
            case KLeft:
                controlSignal.x = -1;
                break;
            case KUp:
                controlSignal.y = 1;
                break;
            case KDown:
                controlSignal.y = -1;
                break;
            case KIn:
                controlSignal.z = 1;
                break;
            case KOut:
                controlSignal.z = -1;
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
        
        switch (force)
        {
            case KNoAction:
                // do nothing
                break;
            case KUp:
                multiplier = 0.0001f; 
                break;
            case KDown:
                multiplier = 0.001f; 
                break;
            case KLeft:
                multiplier = 0.01f; 
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
        
        this.transform.localPosition += controlSignal * multiplier;
        //this.transform.localPosition += controlSignal * forceMultiplier;
        
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Waist.localPosition);

        // Reached target
        if (distanceToTarget > 5.0f) 
        {
            AddReward(-5.0f);
        }
        if (distanceToTarget > 3.0f) 
        {
            AddReward(-3.0f);
        }
        else if (distanceToTarget > 2.5f) 
        {
            AddReward(-2.5f);
        }
        if (distanceToTarget > 2.0f) 
        {
            AddReward(-5.0f);
            EndEpisode();
        }
        else if (distanceToTarget > 1.5f) 
        {
            AddReward(-2.0f);
        }
        else if (distanceToTarget > 1.0f)
        {
            AddReward(-1.0f);
        }
        else if (distanceToTarget > 0.75f)
        {
            AddReward(-0.75f);
        }
        else if (distanceToTarget > 0.5f)
        {
            AddReward(-0.5f);
        }
        else if (distanceToTarget == 0.0f)
        {
            AddReward(5.0f);
            //EndEpisode();
        }
        else if (distanceToTarget < 0.0001f)
        {
            AddReward(3.0f);
            //EndEpisode();
        }
        else if (distanceToTarget < 0.001f)
        {
            AddReward(2.0f);
        }
        else if (distanceToTarget < 0.005f)
        {
            AddReward(1.5f);
        }
        else if (distanceToTarget < 0.01f)
        {
            AddReward(1.0f);
        }
        else if (distanceToTarget < 0.05f)
        {
            AddReward(0.5f);
        }
        else if (distanceToTarget < 0.1f)
        {
            AddReward(0.25f);
        }
        else if (distanceToTarget < 0.125f)
        {
            AddReward(0.1f);
        }
        else if (distanceToTarget < 0.25f)
        {
            AddReward(-0.1f);
        }
        else if (distanceToTarget < 0.5f)
        {
            AddReward(-0.25f);
        }
        else
        {
            //SetReward(-1.0f);
            EndEpisode();
        }
    }*/

    /* public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        float multiplier = 0.0f;

        var action = actionBuffers.DiscreteActions[0];
        //var force = actionBuffers.DiscreteActions[1];
        
        switch (action)
        {
            case KNoAction:
                // do nothing
                break;
            case KRight:
                controlSignal.x = 1;
                break;
            case KLeft:
                controlSignal.x = -1;
                break;
            case KUp:
                controlSignal.y = 1;
                break;
            case KDown:
                controlSignal.y = -1;
                break;
            case KIn:
                controlSignal.z = 1;
                break;
            case KOut:
                controlSignal.z = -1;
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
        
       switch (force)
        {
            case KNoAction:
                // do nothing
                break;
            case KUp:
                multiplier = 0.01f; 
                break;
            case KDown:
                multiplier = 0.1f; 
                break;
            case KLeft:
                multiplier = 0.1f; 
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
        
        //this.transform.localPosition += controlSignal * multiplier;
        this.transform.localPosition += controlSignal * forceMultiplier;
        
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Waist.localPosition);
        
        if (distanceToTarget > 1.0f)
        {
            AddReward(-5.0f);
        }
        else if (distanceToTarget > 0.75f)
        {
            AddReward(-2.75f);
        }
        else if (distanceToTarget > 0.5f)
        {
            AddReward(-0.5f);
        }
        else if (distanceToTarget > 0.25f)
        {
            AddReward(-0.1f);
        }
        else if (distanceToTarget > 0.125f)
        {
            AddReward(0.15f);
        }
        else if (distanceToTarget > 0.1f)
        {
            AddReward(0.25f);
        }
        else if (distanceToTarget > 0.05f)
        {
            AddReward(0.5f);
        }
        else if (distanceToTarget > 0.01f)
        {
            AddReward(0.8f);
        }
        else if (distanceToTarget > 0.005f)
        {
            AddReward(0.9f);
        }
        else if (distanceToTarget > 0.001f)
        {
            AddReward(1.0f);
        }
        else if (distanceToTarget > 0.0001f)
        {
            AddReward(2.0f);
            //EndEpisode();
        }
        else if (distanceToTarget == 0.0f)
        {
            AddReward(5.0f);
            //EndEpisode();
        }
        else
        {
            //SetReward(-1.0f);
            EndEpisode();
        }
    }*/
    public Rigidbody rBody;
      public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        //float multiplier = 0.0f;
        var action = actionBuffers.DiscreteActions[0];
        //var force = actionBuffers.DiscreteActions[1];
        switch (action)
        {
            case KNoAction:
                // do nothing
                break;
            case KRight:
                controlSignal.x = 1;
                break;
            case KLeft:
                controlSignal.x = -1;
                break;
            case KUp:
                controlSignal.y = 1;
                break;
            case KDown:
                controlSignal.y = -1;
                break;
            case KIn:
                controlSignal.z = 1;
                break;
            case KOut:
                controlSignal.z = -1;
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
        /*switch (force)
        {
            case KNoAction:
                multiplier = 0.005f;
                break;
            case KUp:
                multiplier = 0.005f;
                break;
            case KDown:
                multiplier = 0.005f;
                break;
            /*case KLeft:
                multiplier = 0.0005f;
                break;
            case KRight:
                multiplier = 0.0f;
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }*/
        this.transform.localPosition += controlSignal * forceMultiplier;
        //rBody.AddForce(controlSignal*forceMultiplier);
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Waist.localPosition);
        
        if (distanceToTarget < distance)
        {
            AddReward(1.0f);
            //sum -= distanceToTarget;
            //distance = distanceToTarget;
        }
        /*else if (distanceToTarget > distance)
        {
            AddReward(-1.0f);
        }*/
        else 
        {
            AddReward(-1.0f);
            //sum += distanceToTarget;
        }
        /*if (distanceToTarget < 0.01f)
        {
            AddReward(0.5f);
        }
        else if (distanceToTarget < 0.05f)
        {
            AddReward(0.25f);
        }
        else if (distanceToTarget < 0.1f)
        {
            AddReward(0.25f);
        }
        else
        {
            AddReward(-0.5f);
        }*/
        /*aDistnace[count] = distanceToTarget;
        float sum = 0.0f;
        float average = 0.0f;
        for (int i = 0; i < aDistnace.Length; i++)
        {
            sum += aDistnace[i];
        }

        average = sum / aDistnace.Length;

        distance = average;
        count++;
        count = count % 10;*/
        distance = distanceToTarget;
    }
    /*public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        //print(Input.GetAxis("Horizontal"));
        //print(Input.GetAxis("Vertical"));
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        //continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[2] = Input.GetAxis("Vertical");
    }*/
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = KNoAction;
        discreteActionsOut[1] = KDown;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = KRight;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = KUp;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = KLeft;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = KDown;
        }
    }
}