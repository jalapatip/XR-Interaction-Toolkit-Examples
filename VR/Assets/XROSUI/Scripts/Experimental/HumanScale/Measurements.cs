using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;

public class Measurements : MonoBehaviour
{
    private int _stepCounter = 0;
    public XRGrabInteractable grabInteractable;

    public float armLengthL = 0.0f;
    public float armLengthR = 0.0f;
    public float chestWidth = 0.0f;
    public float height = 0.0f;

    private float kneeL = 0.0f;
    private float kneeR = 0.0f;
    private float elbowLx = 0.0f;
    private float elbowRx = 0.0f;
    private float elbowLy = 0.0f;
    private float elbowRy = 0.0f;
    private float elbowLz = 0.0f;
    private float elbowRz = 0.0f;
    float armL = 0.0f;
    float armR = 0.0f;
    private float shoulderLx = 0.0f;
    private float shoulderRx = 0.0f;
    private float shoulderLy = 0.0f;
    private float shoulderRy = 0.0f;
    float bheight = 0.0f;
    float mheight = 0.0f;

    public Text LeftArmLengthText;

    //public Text RightArmLengthText;
    public Text LeftArmInstructionText;
    public RawImage workflowPose;
    public Texture workflowStep1;
    public Texture workflowStep2;
    public Transform LeftControllerTransform;
    public Transform RightControllerTransform;
    public Transform HMDTransform;
    private List<string[]> rowData = new List<string[]>();

    void Start()
    {
        // UI initialization
        LeftArmLengthText = GameObject.Find("LeftArmLengthText").GetComponent<Text>();
        //RightArmLengthText = GameObject.Find("RightArmLengthText").GetComponent<Text>();
        LeftArmInstructionText = GameObject.Find("LeftArmInstructionText").GetComponent<Text>();
        Image workflowPoseImg = workflowPose.GetComponent<Image>();
        // Function initialization
        LeftControllerTransform = Core.Ins.XRManager.GetLeftDirectController().transform;
        RightControllerTransform = Core.Ins.XRManager.GetRightDirectController().transform;
        HMDTransform = Core.Ins.XRManager.GetXrCamera().transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            MeasureLeftArm();
        }
    }

    private void OnEnable()
    {
        //grabInteractable = GetComponent<XRGrabInteractable>();
        //grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        //grabInteractable.onSelectExit.AddListener(OnSelectExit);
        grabInteractable.onActivate.AddListener(OnActivate);
    }

    private void OnDisable()
    {
        //grabInteractable = GetComponent<XRGrabInteractable>();
        //grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        //grabInteractable.onSelectExit.AddListener(OnSelectExit);
        grabInteractable.onActivate.RemoveListener(OnActivate);
    }

    // private void OnSelectEnter(XRBaseInteractor obj)
    // {
    // }
    //
    // private void OnSelectExit(XRBaseInteractor obj)
    // {
    // }

    private void OnActivate(XRBaseInteractor obj)
    {
        MeasureLeftArm();
    }

    public void MeasureLeftArm()
    {
        var RightControllerPos = RightControllerTransform.position;
        var LeftControllerPos = LeftControllerTransform.position; 
        if (_stepCounter == 0)
        {
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 1. While standing with both arms straight down press the trigger button on both controllers to continue";
            //LeftArmInstructionText.text = $"Step 1. Stand still with your left arm straight down and use right controller to click \"Next\".";
            //workflowPose.texture = workflowStep1;
            //Core.Ins.ScenarioManager.SetFlag("AgreedCalibration", true);
            _stepCounter++;
        }
        else if (_stepCounter == 1)
        {
            // Measure both arms horizontally
            armL = LeftControllerPos.y;
            armR = RightControllerPos.y;
            shoulderRx = RightControllerPos.x;
            shoulderLx = LeftControllerPos.x;
            elbowLx = shoulderLx;
            elbowRx = shoulderRx;
            elbowLz = LeftControllerPos.z;
            elbowRz = RightControllerPos.z;
            LeftArmInstructionText.text =
                $"Step 2. While standing. Position your right arm horizontally and bend your arm 90 degrees horizontally towards your chest. Press both trigger button to continue.";
            // workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 2)
        {
            // Measure both arms horizontally
            shoulderRy = RightControllerPos.y;
            elbowRy = Math.Abs(elbowRz - RightControllerPos.z);
            elbowRy = shoulderRy - elbowRy;
            LeftArmInstructionText.text =
                $"Step 3. While standing. Position your left arm horizontally and bend your arm 90 degrees horizontally towards your chest. Press both trigger button to continue.";
            // workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 3)
        {
            // Measure each shoulder joint
            shoulderLy = LeftControllerPos.y;
            elbowLy = Math.Abs(elbowLz - LeftControllerPos.z);
            elbowLy = shoulderLy - elbowLy;
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 4. While standing. Position your right controller to top of your head and press the trigger button on both controllers to continue";
            //workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 4)
        {
            // Measure height
            height = RightControllerPos.y;
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 5. While standing, position your left controller to your left knee and right controller to your right knee. Press both trigger buttons to continue";
            //workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 5)
        {
            // Measure knee joints
            kneeL = LeftControllerPos.y;
            kneeR = RightControllerPos.y;
            armLengthL = Mathf.Abs(armL - shoulderLy);
            armLengthR = Mathf.Abs(armR - shoulderRy);
            chestWidth = Math.Abs(shoulderLx - shoulderLy);
            DataContainer_User user = new DataContainer_User();
            user.height = height;
            user.LarmLength = armLengthL;
            user.RarmLength = armLengthR;
            user.Lshoulderx = shoulderLx;
            user.Lshouldery = shoulderLy;
            user.Rshoulderx = shoulderRx;
            user.Rshouldery = shoulderLy;
            user.chestWidth = chestWidth;
            user.Lelbowy = elbowLy;
            user.Relbowy = elbowRy;
            user.Lkneey = kneeL;
            user.Rkneey = kneeR;
            user.ConvertToJSON();
            var fileName = "/usercalibration2.json";
            File.WriteAllText(Application.persistentDataPath + fileName, user.JSONdata);
            LeftArmLengthText.text =
                $"Left Arm length: {armLengthL} Right Arm length: {armLengthR} Height: {height} LShoulder: {shoulderLy} RShoulder: {shoulderRy} LElbow: {elbowLy} Relbow: {elbowRy} Lknee: {kneeL} Rknee: {kneeR}";
            workflowPose.texture = null;

            LeftArmInstructionText.text = $"Press \"Start\" to calibrate.";
            _stepCounter = 0;
            
#if UNITY_EDITOR
            EditorUtility.RevealInFinder(Application.persistentDataPath + fileName);
#endif
        }
    }
}