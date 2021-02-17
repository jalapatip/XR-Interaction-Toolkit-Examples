using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Measurements : MonoBehaviour
{
    int stepCounter = 0;
    public float armLengthL = 0.0f;
    public float armLengthR = 0.0f;
    public float height = 0.0f;
    float straightDownL = 0.0f;
    float straightDownR = 0.0f;
    float horizontalL = 0.0f;
    float horizontalR = 0.0f;
    float bheight = 0.0f;
    float mheight = 0.0f;
    public Button LeftArmMeasureButton;
    public Button UpdateFromFileButton;
    public Button NextPageButton;
    public Text LeftArmLengthText;
    //public Text RightArmLengthText;
    public Text LeftArmInstructionText;
    public RawImage workflowPose;
    public Texture workflowStep1;
    public Texture workflowStep2;
    public Transform LeftControllerTransform;
    public Transform RightControllerTransform;
    public Transform HMDTransform;
    Vector3 LeftControllerPos;
    Vector3 RightControllerPos;
    Vector3 HMDPos;
    Vector3 LeftControllerRot;
    Vector3 RightControllerRot;
    Vector3 HMDRot;

    void Start()
    {
        // UI initialization
        Button measureBtn = LeftArmMeasureButton.GetComponent<Button>();
        measureBtn.onClick.AddListener(MeasureLeftArm);
        LeftArmLengthText = GameObject.Find("LeftArmLengthText").GetComponent<Text>();
        //RightArmLengthText = GameObject.Find("RightArmLengthText").GetComponent<Text>();
        LeftArmInstructionText = GameObject.Find("LeftArmInstructionText").GetComponent<Text>();
        Image workflowPoseImg = workflowPose.GetComponent<Image>();
        // Function initialization
        //LeftController = GameObject.Find("LeftController");
        LeftControllerTransform = Core.Ins.XRManager.GetLeftDirectController().transform;
        //RightController = GameObject.Find("RightController");
        RightControllerTransform = Core.Ins.XRManager.GetRightDirectController().transform;
        //HMD = GameObject.Find("HMD");
        HMDTransform = Core.Ins.XRManager.GetXrCamera().transform;
    }

    void Update()
    {
        UpdateGenericPos();
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            MeasureLeftArm();
        }
    }

    public void MeasureLeftArm()
    {
        if (stepCounter == 0)
        {
            // Update instruction
            //LeftArmInstructionText.text = $"Step 1. Stand still with your left and right arm straight down and press the trigger button on both controllers to continue";
            LeftArmInstructionText.text = $"Step 1. Stand still with your left arm straight down and use right controller to click \"Next\".";
            LeftArmMeasureButton.GetComponentInChildren<Text>().text = "Next";
            workflowPose.texture = workflowStep1;
            //Core.Ins.ScenarioManager.SetFlag("AgreedCalibration", true);
            stepCounter++;

        }
        else if (stepCounter == 1)
        {
            // Measure when left arm is straight down
            straightDownL = LeftControllerPos.y;
            //straightDownR = RightControllerPos.y;
            // Update instruction
            //LeftArmInstructionText.text = $"Step 2.Raise your left and right arm in parallel to ground while holding other body parts stationary. Press the trigger button on both controllers to continue";
            //LeftArmInstructionText.text = $"Step 2.Raise your left arm in parallel to ground while holding other body parts stationary. Again, use right controller to click \"Next\".";
            LeftArmInstructionText.text = $"Step 2. Stand still with your right arm straight down and use left controller to click \"Next\".";
            
            workflowPose.texture = workflowStep2;

            stepCounter++;
        }
        else if (stepCounter == 2)
        {
            // Measure when left arm is straight down
            //straightDownL = LeftControllerPos.y;
            straightDownR = RightControllerPos.y;
            // Update instruction
            //LeftArmInstructionText.text = $"Step 2.Raise your left and right arm in parallel to ground while holding other body parts stationary. Press the trigger button on both controllers to continue";
            LeftArmInstructionText.text = $"Step 3.Raise your left arm in parallel to ground while holding other body parts stationary. Again, use right controller to click \"Next\".";
            //LeftArmInstructionText.text = $"Step 2. Stand still with your right arm straight down and use left controller to click \"Next\".";
            
            //workflowPose.texture = workflowStep2;

            stepCounter++;
        }
        else if (stepCounter == 3)
        {
            // Measure when left arm is straight down
            //straightDownL = LeftControllerPos.y;
            horizontalL = LeftControllerPos.y;
            // Update instruction
            //LeftArmInstructionText.text = $"Step 2.Raise your left and right arm in parallel to ground while holding other body parts stationary. Press the trigger button on both controllers to continue";
            LeftArmInstructionText.text = $"Step 4.Raise your right arm in parallel to ground while holding other body parts stationary. Again, use left controller to click \"Next\".";
            //LeftArmInstructionText.text = $"Step 2. Stand still with your right arm straight down and use left controller to click \"Next\".";
            
            workflowPose.texture = workflowStep2;

            stepCounter++;
        }
        else if (stepCounter == 4)
        {
            // Measure when left arm is straight down
            //straightDownL = LeftControllerPos.y;
            //bheight = HMDPos.y;
            bheight = LeftControllerPos.y;
            // Update instruction
            //LeftArmInstructionText.text = $"Step 2.Raise your left and right arm in parallel to ground while holding other body parts stationary. Press the trigger button on both controllers to continue";
            LeftArmInstructionText.text = $"Step 5.Put left controller to the ground. Click \"Next\".";
            //LeftArmInstructionText.text = $"Step 2. Stand still with your right arm straight down and use left controller to click \"Next\".";
            
            workflowPose.texture = workflowStep2;

            stepCounter++;
        }
        else if (stepCounter == 5)
        {
            // Measure when left arm is straight down
            //straightDownL = LeftControllerPos.y;
            //bheight = HMDPos.y;
            mheight = LeftControllerPos.y;
            // Update instruction
            //LeftArmInstructionText.text = $"Step 2.Raise your left and right arm in parallel to ground while holding other body parts stationary. Press the trigger button on both controllers to continue";
            LeftArmInstructionText.text = $"Step 6.Put left controller to the top of your head. Click \"Next\".";
            //LeftArmInstructionText.text = $"Step 2. Stand still with your right arm straight down and use left controller to click \"Next\".";
            
            workflowPose.texture = workflowStep2;

            stepCounter++;
        }
        else if (stepCounter == 6)
        {
            // Measure when left arm is raised to horizontal
            //horizontalL = LeftControllerPos.y;
            horizontalR = RightControllerPos.y;
            armLengthL = Mathf.Abs(straightDownL - horizontalL);
            armLengthR = Mathf.Abs(straightDownR - horizontalR);
            height = Mathf.Abs(bheight - mheight);
            LeftArmLengthText.text = $"Left Arm length: {armLengthL} Right Arm length: {armLengthR} Height: {height}";
            //RightArmLengthText.text = $"Right Arm length: {armLengthR}";
            LeftArmMeasureButton.GetComponentInChildren<Text>().text = "Start";
            workflowPose.texture = null;
            
            LeftArmInstructionText.text = $"Measure the length of arm. Press \"Start\" to measure.";
            stepCounter = 0;
        }

    }

    void UpdateGenericPos()
    {
        LeftControllerPos = LeftControllerTransform.position;
        RightControllerPos = RightControllerTransform.position;
        HMDPos = HMDTransform.position;
        LeftControllerRot = LeftControllerTransform.rotation.eulerAngles;
        RightControllerRot = RightControllerTransform.rotation.eulerAngles;
        HMDRot = HMDTransform.rotation.eulerAngles;
    }
}
