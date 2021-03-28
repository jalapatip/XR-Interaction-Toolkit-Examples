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
    public DataReplayHelper helper;
    public Transform Headset;
    public Transform LeftController;
    public Transform RightController;

    public override void OnEpisodeBegin()
    {
        helper.RandomPosition();
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
        sensor.AddObservation(Headset.localPosition - LeftController.localPosition);
        sensor.AddObservation(Headset.localPosition - RightController.localPosition);
        sensor.AddObservation(LeftController.localRotation);
        sensor.AddObservation(RightController.localRotation);
    }
    
    public int count = 0;
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        var action = actionBuffers.DiscreteActions[0];

        /*if (DataReplayHelper.getCurrentGesture() == (ENUM_XROS_EquipmentGesture) action)
        {
            AddReward(1.0f);
        }
        else
        {
            AddReward(-1.0f);
        }*/
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