using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
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
    private float shoulderLz = 0.0f;
    private float shoulderRz = 0.0f;
    private float kneeLx = 0.0f;
    private float kneeRx = 0.0f;
    private float kneeWidth = 0.0f;
    private float headsetHeight = 0.0f;
    private float waist = 0.0f;
    private float footL = 0.0f;
    private float footR = 0.0f;
    private float handL = 0.0f;
    private float handR = 0.0f;
    private float hipL = 0.0f;
    private float hipR = 0.0f;
    float bheight = 0.0f;
    float mheight = 0.0f;
    private bool calibrationComplete = false;
    private Vector2 headV;

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
    public GameObject leftShoulder;
    public GameObject rightShoulder;
    public GameObject leftElbow;
    public GameObject rightElbow;
    public GameObject leftKnee;
    public GameObject rightKnee;
    public GameObject head;
    public GameObject midWaist;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject rightFoot;
    public GameObject leftFoot;
    public GameObject leftHip;
    public GameObject rightHip;

    void Start()
    {
        // UI initialization
        LeftArmLengthText = GameObject.Find("LeftArmLengthText").GetComponent<Text>();
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
            Calibrate();
        }

        /*if (calibrationComplete)
        {
            SetBody();
        }*/
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
        Calibrate();
    }

    private void Calibrate()
    {
        var RightControllerPos = RightControllerTransform.position;
        var LeftControllerPos = LeftControllerTransform.position;
        var HMDPos = HMDTransform.position;
        var x = HMDPos.x;
        var y = HMDPos.y;
        var z = HMDPos.z;
        if (_stepCounter == 0)
        {
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 1. While standing with both arms straight down press the trigger button on both controllers to continue";
            //workflowPose.texture = workflowStep1;
            _stepCounter++;
        }
        else if (_stepCounter == 1)
        {
            // Measure both arms Vertically
            
            armL = LeftControllerPos.y;
            armR = RightControllerPos.y;
            handL = LeftControllerPos.y;
            handR = RightControllerPos.y;
            headsetHeight = HMDPos.y;
            //x = HMDPos.x;
            //var lShoulder = math.abs(x - LeftControllerPos.x);
            Vector2 pointA = new Vector2(HMDPos.x, HMDPos.z);
            headV = pointA;
            Vector2 pointB = new Vector2(RightControllerPos.x, RightControllerPos.z);
            
            //var rShoulder = math.abs(x + RightControllerPos.x);
            //shoulderRx = (float) Math.Sqrt((rShoulder*rShoulder)+(RightControllerPos.z*RightControllerPos.z));
            //shoulderLx = (float) Math.Sqrt((LeftControllerPos.x*LeftControllerPos.x)+(LeftControllerPos.z*LeftControllerPos.z));
            shoulderRx = Vector2.Distance(pointA, pointB);
            pointB = new Vector2(LeftControllerPos.x, LeftControllerPos.z);
            shoulderLx = Vector2.Distance(pointA, pointB);
            shoulderRz = RightControllerPos.z;
            shoulderLz = LeftControllerPos.z;
            chestWidth = Math.Abs(shoulderLx + shoulderRx);
            elbowLx = shoulderLx;
            elbowRx = shoulderRx;
            LeftArmInstructionText.text =
                $"Step 2. While standing with arms straight down by your side. Bend both elbows 90 degrees so that they are parallel with the floor. Press both trigger button to continue.";
            // workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 2)
        {
            // Measure both elbows at 90 degrees
            //elbowRz = RightControllerPos.z;
            //elbowLz = LeftControllerPos.z;
            elbowRy = RightControllerPos.y;
            elbowLy = LeftControllerPos.y;
            LeftArmInstructionText.text =
                $"Step 3. While standing with both arms straight down by your side. Raise both arms 90 degrees so that they are parallel to the floor. Press both trigger button to continue.";
            // workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 3)
        {
            // Measure both arms parallel to the floor
            shoulderRy = RightControllerPos.y;
            shoulderLy = LeftControllerPos.y;
            //elbowRy = shoulderRy - elbowRy;
            //elbowLy = shoulderLy - elbowLy;
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 4. While standing. Position your right controller to top of your head and press the trigger button on both controllers to continue";
            //workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 4)
        {
            // Measure both arms parallel to the floor
            height = RightControllerPos.y;
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 4. While standing. Position your right controller to your mid waist and activate the cube to continue";
            //workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 5)
        {
            // Measure height
            waist = RightControllerPos.y;
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 5. While standing, position your left controller to your left hip and right controller to your right hip. Press both trigger buttons to continue";
            //workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 6)
        {
            // Measure height
            hipL = LeftControllerPos.y;
            hipR = RightControllerPos.y;
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 5. While standing, position your left controller to your left knee and right controller to your right knee. Press both trigger buttons to continue";
            //workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 7)
        {
            // Measure height
            kneeL = LeftControllerPos.y;
            kneeR = RightControllerPos.y;
            //kneeRx = (float) Math.Sqrt((RightControllerPos.x*RightControllerPos.x)+(RightControllerPos.z*RightControllerPos.z));
            //kneeLx = (float) Math.Sqrt((LeftControllerPos.x*LeftControllerPos.x)+(LeftControllerPos.z*LeftControllerPos.z));
            Vector2 pointA = new Vector2(HMDPos.x, HMDPos.z);
            Vector2 pointB = new Vector2(RightControllerPos.x, RightControllerPos.z);
            kneeRx = Vector2.Distance(headV, pointB);
            pointB = new Vector2(LeftControllerPos.x, LeftControllerPos.z);
            kneeLx = Vector2.Distance(headV, pointB);
            kneeWidth = Math.Abs(kneeRx + kneeLx);
            // Update instruction
            LeftArmInstructionText.text =
                $"Step 5. While standing, position your left controller to your left foot and right controller to your right foot. Press both trigger buttons to continue";
            //workflowPose.texture = workflowStep2;
            _stepCounter++;
        }
        else if (_stepCounter == 8)
        {
            // Measure knee joints
            footL = LeftControllerPos.y;
            footR = RightControllerPos.y;
            armLengthL = Mathf.Abs(armL - shoulderLy);
            armLengthR = Mathf.Abs(armR - shoulderRy);
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
            SetBody();
            LeftArmLengthText.text =
                $"Left Arm length: {armLengthL} Right Arm length: {armLengthR} Height: {height} LShoulder: {shoulderLy} RShoulder: {shoulderRy} LElbow: {elbowLy} Relbow: {elbowRy} Lknee: {kneeL} Rknee: {kneeR} chest: {chestWidth} kneew: {kneeWidth}";
            workflowPose.texture = null;

            LeftArmInstructionText.text = $"Press \"Start\" to calibrate.";
            _stepCounter = 0;
            calibrationComplete = true;
            
#if UNITY_EDITOR
            EditorUtility.RevealInFinder(Application.persistentDataPath + fileName);
#endif
        }
    }

    private void SetBody()
    {
        var HMDPos = HMDTransform.position;
        var x = 0;
        var y = HMDPos.y;
        var z = HMDPos.z + 2;
        var shoulderOffset = chestWidth / 2;
        var kneeOffset = kneeWidth / 2;
        head.transform.position = new Vector3(x, headsetHeight, z);
        leftShoulder.transform.position = new Vector3(x - shoulderOffset, shoulderLy, z);
        rightShoulder.transform.position = new Vector3(x + shoulderOffset, shoulderRy, z);
        leftElbow.transform.position = new Vector3(x - shoulderOffset, elbowLy, z);
        rightElbow.transform.position = new Vector3(x + shoulderOffset, elbowRy, z);
        leftKnee.transform.position = new Vector3(x - kneeOffset, kneeL, z);
        rightKnee.transform.position = new Vector3(x + kneeOffset, kneeR, z);
        midWaist.transform.position = new Vector3(x, waist, z);
        leftHand.transform.position = new Vector3(x - shoulderOffset, handL, z);
        rightHand.transform.position = new Vector3(x + shoulderOffset, handR, z);
        leftFoot.transform.position = new Vector3(x - kneeOffset, footL, z);
        rightFoot.transform.position = new Vector3(x + kneeOffset, footR, z);
        leftHip.transform.position = new Vector3(x - kneeOffset, hipL, z);
        rightHip.transform.position = new Vector3(x + kneeOffset, hipR, z);
    }
    
}