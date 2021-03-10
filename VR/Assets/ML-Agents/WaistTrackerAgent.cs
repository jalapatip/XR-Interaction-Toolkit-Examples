using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class WaistTrackerAgent : Agent
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
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Waist.localPosition);

        // Reached target
        if (distanceToTarget < 0.1f)
        {
            SetReward(1.0f);
            EndEpisode();
//            print("Success");
        }

        // Fell off platform
        else if (distanceToTarget > 2.0f) 
        {
//            print("Fail");
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        //print(Input.GetAxis("Horizontal"));
        //print(Input.GetAxis("Vertical"));
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        //continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[2] = Input.GetAxis("Vertical");
    }
}