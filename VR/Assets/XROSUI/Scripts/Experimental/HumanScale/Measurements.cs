using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class Measurements : MonoBehaviour
{
    int stepCounter = 0;
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
    private List<string[]> rowData = new List<string[]>();
    
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
      //  EventsManager.current.onTriggerPress += MeasureLeftArm;
    }

    void Update()
    {
        UpdateGenericPos();
        /*if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            MeasureLeftArm();
        }*/
    }
    
    public XRGrabInteractable grabInteractable; 
    private void OnEnable()
    {
        //grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        grabInteractable.onSelectExit.AddListener(OnSelectExit);
        grabInteractable.onActivate.AddListener(OnActivate);

    }
    
    private void OnSelectEnter(XRBaseInteractor obj)
    {
    }

    private void OnSelectExit(XRBaseInteractor obj)
    {
        
    }
    
    private void OnActivate(XRBaseInteractor obj)
    {
        MeasureLeftArm();
    }

    public void MeasureLeftArm()
    {
        if (stepCounter == 0)
        {
            // Update instruction
            LeftArmInstructionText.text = $"Step 1. While standing with both arms straight down press the trigger button on both controllers to continue";
            //LeftArmInstructionText.text = $"Step 1. Stand still with your left arm straight down and use right controller to click \"Next\".";
            LeftArmMeasureButton.GetComponentInChildren<Text>().text = "Next";
            //workflowPose.texture = workflowStep1;
            //Core.Ins.ScenarioManager.SetFlag("AgreedCalibration", true);
            stepCounter++;

        }
        else if (stepCounter == 1)
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
            LeftArmInstructionText.text = $"Step 2. While standing. Position your right arm horizontally and bend your arm 90 degrees horizontally towards your chest. Press both trigger button to continue.";
            // workflowPose.texture = workflowStep2;
            stepCounter++;
        }
        else if (stepCounter == 2)
        {
            // Measure both arms horizontally
            shoulderRy = RightControllerPos.y;
            elbowRy = Math.Abs(elbowRz - RightControllerPos.z);
            elbowRy = shoulderRy - elbowRy;
            LeftArmInstructionText.text = $"Step 3. While standing. Position your left arm horizontally and bend your arm 90 degrees horizontally towards your chest. Press both trigger button to continue.";
            // workflowPose.texture = workflowStep2;
            stepCounter++;
        }
        else if (stepCounter == 3)
        {
            // Measure each shoulder joint
            shoulderLy = LeftControllerPos.y;
            elbowLy = Math.Abs(elbowLz - LeftControllerPos.z);
            elbowLy = shoulderLy - elbowLy;
            // Update instruction
            LeftArmInstructionText.text = $"Step 4. While standing. Position your right controller to top of your head and press the trigger button on both controllers to continue";
            //workflowPose.texture = workflowStep2;
            stepCounter++;
        }
        else if (stepCounter == 4)
        {
            // Measure height
            height = RightControllerPos.y;
            // Update instruction
            LeftArmInstructionText.text = $"Step 5. While standing, position your left controller to your left knee and right controller to your right knee. Press both trigger buttons to continue";
            //workflowPose.texture = workflowStep2;
            stepCounter++;
        }
        else if (stepCounter == 5)
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
            user.ToString();
            user.ConvertToJSON();
            File.WriteAllText(Application.persistentDataPath + "/usercalibration.json", user.JSONdata);
            LeftArmLengthText.text = $"Left Arm length: {armLengthL} Right Arm length: {armLengthR} Height: {height} LShoulder: {shoulderLy} RShoulder: {shoulderRy} LElbow: {elbowLy} Relbow: {elbowRy} Lknee: {kneeL} Rknee: {kneeR}";
            LeftArmMeasureButton.GetComponentInChildren<Text>().text = "Start";
            workflowPose.texture = null;
            
            LeftArmInstructionText.text = $"Press \"Start\" to calibrate.";
            stepCounter = 0;
        }

    }

    /*private void OnDestroy()
    {
        EventsManager.current.onTriggerPress -= MeasureLeftArm;
    }*/

    void UpdateGenericPos()
    {
        LeftControllerPos = LeftControllerTransform.position;
        RightControllerPos = RightControllerTransform.position;
        HMDPos = HMDTransform.position;
        LeftControllerRot = LeftControllerTransform.rotation.eulerAngles;
        RightControllerRot = RightControllerTransform.rotation.eulerAngles;
        HMDRot = HMDTransform.rotation.eulerAngles;
    }

    /*void save()
    {
        string[] rowDataTemp = new string[10];
        rowDataTemp[0] = "Height";
        rowDataTemp[1] = "LeftArm";
        rowDataTemp[2] = "RightArm";
        rowDataTemp[3] = "LeftShoulderX";
        rowDataTemp[4] = "RightShoulderX";
        rowDataTemp[5] = "LeftShoulderY";
        rowDataTemp[6] = "RightShoulderY";
        rowDataTemp[7] = "LeftElbow";
        rowDataTemp[8] = "RightElbow";
        rowDataTemp[9] = "LeftKnee";
        rowDataTemp[10] = "RightKnee";
        rowData.Add(rowDataTemp)

        rowDataTemp = new string[10];
        rowDataTemp[0] = height;
        rowDataTemp[1] = armL;
        rowDataTemp[2] = "Rig;
        rowDataTemp[3] = "LeftShoulderX";
        rowDataTemp[4] = "RightShoulderX";
        rowDataTemp[5] = "LeftShoulderY";
        rowDataTemp[6] = "RightShoulderY";
        rowDataTemp[7] = "LeftElbow";
        rowDataTemp[8] = "RightElbow";
        rowDataTemp[9] = "LeftKnee";
        rowDataTemp[10] = "RightKnee"; 
    }*/
}

