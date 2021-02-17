using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeasureLeftArmLength : MonoBehaviour
{
    int stepCounter = 0;
    public float armLength = 0.0f;
    float straightDownY = 0.0f;
    float horizontalY = 0.0f;
    int readingFile = 1;
    bool firstSetSkeletonPos = false;
    bool readingJoints = false;
    // float offset_x = 0.0f;     // This is for TEST UI ONLY
    // float offset_y = 0.0f;     // This is for TEST UI ONLY
    // float offset_z = -0.1f;     // This is for TEST UI ONLY
    // UI components
    public Button LeftArmMeasureButton;
    public Button UpdateFromFileButton;
    public Button NextPageButton;
    public Text LeftArmLengthText;
    public Text LeftArmInstructionText;
    public RawImage workflowPose;
    public Texture workflowStep1;
    public Texture workflowStep2;
    public RawImage showSkeletonIdx;
    public Texture jointsImage;
    public Texture bonesImage;
    public GameObject stickSkeleton;
    TMP_Text LeftPrintPanel;
    TMP_Text RightPrintPanel;
    GameObject thisUIPanel;
    GameObject InReachFarthestPlane;
    GameObject InReachProperPlane;
    GameObject InReachFarthestPlaneCaption;
    GameObject InReachProperPlaneCaption;
    // Generic (from systems)
    //GameObject LeftController;
    //GameObject RightController;
    //GameObject HMD;
    public Transform LeftControllerTransform;
    public Transform RightControllerTransform;
    public Transform HMDTransform;
    Vector3 LeftControllerPos;
    Vector3 RightControllerPos;
    Vector3 HMDPos;
    Vector3 LeftControllerRot;
    Vector3 RightControllerRot;
    Vector3 HMDRot;
    // Bone GO of Stick Skeleton
    GameObject Neck;
    GameObject Spine;
    GameObject RightHip;
    GameObject LeftHip;
    GameObject LeftUpperLeg;
    GameObject RightUpperLeg;
    GameObject LeftLowerLeg;
    GameObject RightLowerLeg;
    GameObject LeftFoot;
    GameObject RightFoot;
    GameObject LeftShoulder;
    GameObject RightShoulder;
    GameObject LeftLowerArm;
    GameObject LeftUpperArm;
    GameObject RightLowerArm;
    GameObject RightUpperArm;

    void Start()
    {
        // UI initialization
        Button measureBtn = LeftArmMeasureButton.GetComponent<Button>();
        measureBtn.onClick.AddListener(MeasureLeftArm);
        Button updateFromFileBtn = UpdateFromFileButton.GetComponent<Button>();
        updateFromFileBtn.onClick.AddListener(updateFromFile);
        Button NextPageBtn = NextPageButton.GetComponent<Button>();
        NextPageBtn.onClick.AddListener(showNextPage);
        LeftArmLengthText = GameObject.Find("LeftArmLengthText").GetComponent<Text>();
        LeftArmInstructionText = GameObject.Find("LeftArmInstructionText").GetComponent<Text>();
        Image workflowPoseImg = workflowPose.GetComponent<Image>();
        Image showSkeletonIdxImg = showSkeletonIdx.GetComponent<Image>();

        thisUIPanel = GameObject.Find("UIForUpdate");
        InReachFarthestPlane = GameObject.Find("InReachFarthestPlane");
        InReachProperPlane = GameObject.Find("InReachProperPlane");
        InReachFarthestPlaneCaption = GameObject.Find("InReachFarthestPlaneCaption");
        InReachProperPlaneCaption = GameObject.Find("InReachProperPlaneCaption");
        LeftPrintPanel = GameObject.Find("LeftPrintPanel").GetComponentInChildren<TMP_Text>();
        RightPrintPanel = GameObject.Find("RightPrintPanel").GetComponentInChildren<TMP_Text>();

        Neck = GameObject.Find("Neck");
        Spine = GameObject.Find("Spine");
        RightHip = GameObject.Find("RightHip");
        LeftHip = GameObject.Find("LeftHip");
        LeftUpperLeg = GameObject.Find("LeftUpperLeg");
        RightUpperLeg = GameObject.Find("RightUpperLeg");
        LeftLowerLeg = GameObject.Find("LeftLowerLeg");
        RightLowerLeg = GameObject.Find("RightLowerLeg");
        LeftFoot = GameObject.Find("LeftFoot");
        RightFoot = GameObject.Find("RightFoot");
        LeftShoulder = GameObject.Find("LeftShoulder");
        RightShoulder = GameObject.Find("RightShoulder");
        LeftLowerArm = GameObject.Find("LeftLowerArm");
        LeftUpperArm = GameObject.Find("LeftUpperArm");
        RightLowerArm = GameObject.Find("RightLowerArm");
        RightUpperArm = GameObject.Find("RightUpperArm");

        // Function initialization
        //LeftController = GameObject.Find("LeftController");
        LeftControllerTransform = Core.Ins.XRManager.GetLeftDirectController().transform;
        //RightController = GameObject.Find("RightController");
        RightControllerTransform = Core.Ins.XRManager.GetRightDirectController().transform;
        //HMD = GameObject.Find("HMD");
        HMDTransform = Core.Ins.XRManager.GetXrCamera().transform;

        updateFromFile();
        //stickSkeleton.transform.position = new Vector3(HMDPos.x + 1.0f, HMDPos.y - 0.5f, HMDPos.z);
    }

    void Update()
    {
        UpdateGenericPos();
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            MeasureLeftArm();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            updateFromFile();
        }
    }

    public void MeasureLeftArm()
    {
        if (stepCounter == 0)
        {
            // Update instruction
            LeftArmInstructionText.text = $"Step 1. Stand still with your left arm straight down and use right controller to click \"Next\".";
            LeftArmMeasureButton.GetComponentInChildren<Text>().text = "Next";
            workflowPose.texture = workflowStep1;
            Core.Ins.ScenarioManager.SetFlag("AgreedCalibration", true);
            stepCounter++;
        }
        else if (stepCounter == 1)
        {
            // Measure when left arm is straight down
            straightDownY = LeftControllerPos.y;
            // Update instruction
            LeftArmInstructionText.text = $"Step 2.Raise your left arm in parallel to ground while holding other body parts stationary. Again, use right controller to click \"Next\".";
            workflowPose.texture = workflowStep2;

            stepCounter++;
        }
        else if (stepCounter == 2)
        {
            // Measure when left arm is raised to horizontal
            horizontalY = LeftControllerPos.y;
            armLength = Mathf.Abs(straightDownY - horizontalY);
            // Set length of bones
            Core.Ins.HumanScaleManager.SetArmLength(armLength);
            // IN TEST
            SetBodyLength();
            LeftArmLengthText.text = $"Arm length: {armLength}";
            LeftArmMeasureButton.GetComponentInChildren<Text>().text = "Start";
            workflowPose.texture = null;
            // Change scale of stickSkeleton accordingly
            stickSkeleton.transform.localScale = new Vector3(armLength * 0.009f, armLength * 0.009f, armLength * 0.009f);
            //stickSkeleton.transform.position = new Vector3(HMDPos.x - 0.8f, HMDPos.y - 0.80f, HMDPos.z);
            // Change position and scale of in-reach planes accordingly
            InReachFarthestPlane.transform.position = new Vector3(HMDPos.x, HMDPos.y - 0.3f, HMDPos.z);
            InReachProperPlane.transform.position = new Vector3(HMDPos.x, HMDPos.y - 0.3f, HMDPos.z);
            InReachFarthestPlane.transform.localScale = new Vector3(armLength * 780f, 1.5f, armLength * 780f);
            InReachProperPlane.transform.localScale = new Vector3(armLength * 0.6f * 780f, 2.5f, armLength * 0.6f * 780f);

            InReachFarthestPlaneCaption.transform.position = new Vector3(HMDPos.x, HMDPos.y - 0.28f, HMDPos.z + armLength * 0.7f);
            InReachProperPlaneCaption.transform.position = new Vector3(HMDPos.x, HMDPos.y - 0.28f, HMDPos.z + armLength * 0.7f * 0.6f);
            //InReachFarthestPlaneCaption.transform.localScale = new Vector3(18f, 1.5f, 18f);
            //InReachProperPlaneCaption.transform.localScale = new Vector3(18f, 2.5f, 18f);
            // Update UI position
            UpdateUIPos(thisUIPanel);

            // Set appear for a short time to visualize
            /*
            setAppear(InReachFarthestPlane, 5.0f);
            setAppear(InReachProperPlane, 5.0f);
            setAppear(InReachFarthestPlaneCaption, 5.0f);
            setAppear(InReachProperPlaneCaption, 5.0f);
            setAppear(thisUIPanel, 5.0f);
            */

            LeftArmInstructionText.text = $"Measure the length of arm. Press \"Start\" to measure.";
            Core.Ins.ScenarioManager.SetFlag("FinishedCalibration", true);//tell the core your work is done
            stepCounter = 0;
        }

    }

    IEnumerator setAppear(GameObject GO, float period)
    {
        GO.SetActive(true);
        yield return new WaitForSeconds(period);
        GO.SetActive(false);
    }

    void SetBodyLength()
    {
        float singleRatio = armLength / 3.5f;
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.Neck, singleRatio * 0.5f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.Spine, singleRatio * 2.5f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.RightHip, singleRatio * 0.5f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.LeftHip, singleRatio * 0.5f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.LeftUpperLeg, singleRatio * 2.5f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.RightUpperLeg, singleRatio * 2.5f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.LeftLowerLeg, singleRatio * 2.0f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.RightLowerLeg, singleRatio * 2.0f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.LeftFoot, singleRatio * 1.0f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.RightFoot, singleRatio * 1.0f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.LeftShoulder, singleRatio * 1.2f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.RightShoulder, singleRatio * 1.2f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.LeftLowerArm, singleRatio * 2.0f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.LeftUpperArm, singleRatio * 1.5f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.RightLowerArm, singleRatio * 2.0f);
        Core.Ins.HumanScaleManager.SetBoneLength((int)BoneIdx.RightUpperArm, singleRatio * 1.5f);
    }

    void showNextPage()
    {
        if (readingJoints == false)
        {
            // Show data of bones now
            showBonePage();
            readingJoints = true;
        }
        else
        {
            // Show data of joints now
            showJointPage();
            readingJoints = false;
        }
    }

    void showJointPage()
    {
        // Update image
        showSkeletonIdx.texture = jointsImage;
        // Update data
        var jointsData = getJointsData();
        LeftPrintPanel.text = jointsData.Item1;
        RightPrintPanel.text = jointsData.Item2;
        // Update button
        NextPageButton.GetComponentInChildren<Text>().text = "Show Joints";
    }

    void showBonePage()
    {
        // Update image
        showSkeletonIdx.texture = bonesImage;
        // Update data
        var bonesData = getBonesData();
        LeftPrintPanel.text = bonesData.Item1;
        RightPrintPanel.text = bonesData.Item2;
        // Update button
        NextPageButton.GetComponentInChildren<Text>().text = "Show Bones";
    }

    (string, string) getBonesData()
    {
        string outputLeft = "", outputRight = "";
        for (int i = 0; i < 8; i++)
        {
            outputLeft += (BoneIdx)i;
            outputLeft += ": ";
            outputLeft += Core.Ins.HumanScaleManager.GetBoneLength(i).ToString("0.000");
            outputLeft += System.Environment.NewLine;
        }
        for (int i = 8; i < 16; i++)
        {
            outputRight += (BoneIdx)i;
            outputRight += ": ";
            outputRight += Core.Ins.HumanScaleManager.GetBoneLength(i).ToString("0.000");
            outputRight += System.Environment.NewLine;
        }

        return (outputLeft, outputRight);
    }

    (string, string) getJointsData()
    {
        string outputLeft = "", outputRight = "";
        for (int i = 0; i < 9; i++)
        {
            outputLeft += i.ToString("0");
            outputLeft += ": ";
            outputLeft += Core.Ins.HumanScaleManager.GetJointPosition(i).ToString("0.00");
            outputLeft += System.Environment.NewLine;
        }
        for (int i = 9; i < 17; i++)
        {
            outputRight += i.ToString("0");
            outputRight += ": ";
            outputRight += Core.Ins.HumanScaleManager.GetJointPosition(i).ToString("0.00");
            outputRight += System.Environment.NewLine;
        }

        return (outputLeft, outputRight);
    }

    void updateFromFile()
    {
        // Read in skeleton
        if (readingFile == 1)
        {
            updateSkeletonFromFile("testSkeleton1");
            UpdateFromFileButton.GetComponentInChildren<Text>().text = "Update from File 2";
            readingFile = 2;
        }
        else
        {
            updateSkeletonFromFile("testSkeleton2");
            UpdateFromFileButton.GetComponentInChildren<Text>().text = "Update from File 1";
            readingFile = 1;
        }

        if (firstSetSkeletonPos == false)
        {
            // 0.717f is the default arm length used without calibration from user
            stickSkeleton.transform.localScale = new Vector3(0.717f * 0.009f, 0.717f * 0.009f, 0.717f * 0.009f);
            //stickSkeleton.transform.position = new Vector3(HMDPos.x - 0.8f, HMDPos.y - 0.80f, HMDPos.z);
            firstSetSkeletonPos = true;
        }

        // Set bone length in Game Object
        Neck.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.Neck) * 10.0f, 5.0f);
        Spine.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.Spine) * 10.0f, 5.0f);
        RightHip.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.RightHip) * 10.0f, 5.0f);
        LeftHip.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.LeftHip) * 10.0f, 5.0f);
        LeftUpperLeg.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.LeftUpperLeg) * 10.0f, 5.0f);
        RightUpperLeg.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.RightUpperLeg) * 10.0f, 5.0f);
        LeftLowerLeg.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.LeftLowerLeg) * 10.0f, 5.0f);
        RightLowerLeg.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.RightLowerLeg) * 10.0f, 5.0f);
        LeftFoot.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.LeftFoot) * 10.0f, 5.0f);
        RightFoot.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.RightFoot) * 10.0f, 5.0f);
        LeftShoulder.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.LeftShoulder) * 10.0f, 5.0f);
        RightShoulder.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.RightShoulder) * 10.0f, 5.0f);
        LeftLowerArm.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.LeftLowerArm) * 10.0f, 5.0f);
        LeftUpperArm.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.LeftUpperArm) * 10.0f, 5.0f);
        RightLowerArm.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.RightLowerArm) * 10.0f, 5.0f);
        RightUpperArm.transform.localScale = new Vector3(5.0f, Core.Ins.HumanScaleManager.GetBoneLength(BoneIdx.RightUpperArm) * 10.0f, 5.0f);

        // Update visualization
        if (readingJoints == false)
        {
            showJointPage();
        }
        else
        {
            showBonePage();
        }
    }

    void UpdateUIPos(GameObject UIObject)
    {
        // Vector3 newPosition = new Vector3(UIObject.transform.position.x, UIObject.transform.position.y, UIObject.transform.position.z - armLength * 0.6f);
        float distance = armLength * 0.6f;
        UIObject.transform.position = new Vector3(HMDPos.x, HMDPos.y - 0.2f, HMDPos.z + 0.6f * distance);
    }

    float ComputeGeneric()
    {
        // Compute generic distance between HMD and left controller
        return Vector3.Distance(LeftControllerPos, HMDPos);
        //return ComputeDistance(LeftControllerPos, HMDPos);
    }

    /*
    float ComputeDistance(Vector3 pos1, Vector3 pos2)
    {
        float deltaX = pos1[0] - pos2[0];
        float deltaY = pos1[1] - pos2[1];
        float deltaZ = pos1[2] - pos2[2];
        return (float)Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    }
    */

    void UpdateGenericPos()
    {
        LeftControllerPos = LeftControllerTransform.position;
        RightControllerPos = RightControllerTransform.position;
        HMDPos = HMDTransform.position;
        LeftControllerRot = LeftControllerTransform.rotation.eulerAngles;
        RightControllerRot = RightControllerTransform.rotation.eulerAngles;
        HMDRot = HMDTransform.rotation.eulerAngles;
    }

    //Vector3 normalize(Vector3 vec)
    //{        
    //    float magnitude = vec.x * vec.x + vec.y * vec.y + vec.z * vec.z;
    //    return new Vector3(vec.x / magnitude, vec.y / magnitude, vec.z / magnitude);
    //}

    void updateSkeletonFromFile(string fileName)
    {
        TextAsset text = Resources.Load("JSON/" + fileName) as TextAsset;
        string textString = text.text;
        // StreamReader fi = new StreamReader(Application.dataPath + "/Data/" + fileName + ".txt");
        // string[] axis = fi.ReadToEnd().Split(']');
        string[] axis = textString.Split(']');

        float[] x = axis[0].Replace("[", "").Replace("\r\n", "").Replace("\n", "").Split(' ').Where(s => s != "").Select(f => float.Parse(f)).ToArray();
        float[] y = axis[2].Replace("[", "").Replace("\r\n", "").Replace("\n", "").Split(' ').Where(s => s != "").Select(f => float.Parse(f)).ToArray();
        float[] z = axis[1].Replace("[", "").Replace("\r\n", "").Replace("\n", "").Split(' ').Where(s => s != "").Select(f => float.Parse(f)).ToArray();
        for (int i = 0; i < 17; i++)
        {
            Core.Ins.HumanScaleManager.SetJointPosition(i, new Vector3(x[i], y[i], z[i]));
        }
        Core.Ins.HumanScaleManager.UpdateBoneLength();
    }
}