using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GestureAgent : Agent
{
    void Start()
    {
    }
    public DataReplayHelperGesture helper;
    public Transform Headset;
    public Transform LeftController;
    public Transform RightController;
    public int correct = 0;
    public int wrong = 0;
    public string correctGesture;
    public string predictedGesture;
    public override void OnEpisodeBegin()
    {
        helper.RandomPosition();
    }

    public void Update()
    {
        correctGesture = helper.getGesture();
        if (correctGesture != "None")
        {
            RequestDecision();
        }
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
        sensor.AddObservation(Headset.localPosition - LeftController.localPosition);
        sensor.AddObservation(Headset.localPosition - RightController.localPosition);
        sensor.AddObservation(LeftController.localRotation);
        sensor.AddObservation(RightController.localRotation);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        var action = actionBuffers.DiscreteActions[0];
        string gesture;
        switch (action)
        {
            case (int) ENUM_XROS_EquipmentGesture.None:
                gesture = "None";
                break;
            case (int) ENUM_XROS_EquipmentGesture.Up:
                gesture = "Up";
                break;
            case (int) ENUM_XROS_EquipmentGesture.Down:
                gesture = "Down";
                break;
            case (int) ENUM_XROS_EquipmentGesture.Forward:
                gesture = "Forward";
                break;
            case (int) ENUM_XROS_EquipmentGesture.Backward:
                gesture = "Backward";
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
        predictedGesture = gesture;
        if (correctGesture == predictedGesture)
        {
            AddReward(1.0f);
            correct++;
        }
        else
        {
            AddReward(-1.0f);
            wrong++;
        }
        //correctGesture = helper.getGesture();
        
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = (int) ENUM_XROS_EquipmentGesture.Down;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[1] = (int) ENUM_XROS_EquipmentGesture.Up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = (int) ENUM_XROS_EquipmentGesture.Backward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[3] = (int) ENUM_XROS_EquipmentGesture.Forward;
        }
    }
}