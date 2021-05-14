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
    public int correctNaive;
    public int wrongNaive;
    public string naiveGesture;
    public override void OnEpisodeBegin()
    {
        helper.RandomPosition();
    }

    public void Update()
    {
        /*correctGesture = helper.getGesture();
        if (correctGesture != "None")
        {
            RequestDecision();
        }*/
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
        //sensor.AddObservation(Headset.localPosition - LeftController.localPosition);
        sensor.AddObservation(Headset.localPosition - RightController.localPosition);
       // sensor.AddObservation(LeftController.localPosition);
        //sensor.AddObservation(LeftController.localRotation);
        sensor.AddObservation(RightController.localPosition);
        sensor.AddObservation(RightController.localRotation);
        /*sensor.AddObservation(helper.height);
        sensor.AddObservation(helper.LarmLength);
        sensor.AddObservation(helper.RarmLength);
        sensor.AddObservation(helper.Lshoulderx);
        sensor.AddObservation(helper.Rshoulderx);
        sensor.AddObservation(helper.Lshouldery);
        sensor.AddObservation(helper.Rshouldery);
        sensor.AddObservation(helper.chestWidth);
        sensor.AddObservation(helper.Lelbowy);
        sensor.AddObservation(helper.Relbowy);
        sensor.AddObservation(helper.Lkneey);
        sensor.AddObservation(helper.Rkneey);*/
    }
    public int count = 0;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //count++;
        //print("trueActionCount: " + count);
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
            /*case (((int) ENUM_XROS_EquipmentGesture.RotateClockwise) - 2) :
                gesture = "RotateClockwise";
                break;
            case (((int) ENUM_XROS_EquipmentGesture.RotateCounterclockwise) - 2):
                gesture = "RotateCounterclockwise";
                break;
            case (((int) ENUM_XROS_EquipmentGesture.UForward) - 2):
                gesture = "UForward";
                break;
            case (((int) ENUM_XROS_EquipmentGesture.ArchBackward) -2):
                gesture = "ArchBackward";
                break;*/
           
            default:
                throw new ArgumentException("Invalid action value");
        }
        //correctGesture = helper.getGesture();
        //print("correctGesture: " + correctGesture);
        predictedGesture = gesture;
        if (correctGesture.Equals(predictedGesture))
        {
            AddReward(1.0f);
            correct++;
        }
        else
        {
            AddReward(-1.0f);
            wrong++;
        }

        correctNaive = helper.correctNaive;
        wrongNaive = helper.wrongNaive;
        naiveGesture = helper.naiveGesture;
        //correctGesture = helper.getGesture();
        EndEpisode();
    }

    public void SetCorrectGesture(string gesture)
    {
        correctGesture = gesture;
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