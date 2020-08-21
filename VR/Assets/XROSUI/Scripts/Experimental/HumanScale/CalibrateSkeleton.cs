using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CalibrateSkeleton : MonoBehaviour
{
    public float Scale = 1.0f;

    Transform cachedLeftControllerTransform;
    Transform cachedHMDTransform;
    //Vector3 LeftControllerPos;
    //Vector3 RightControllerPos;
    //Vector3 HMDPos;
    float LHDistanceGeneric = 1.0f;
    // Skeleton
    public Vector3[] skeletonPos = new Vector3[17];
    Vector3 LeftHandPos;
    Vector3 HeadPos;
    float LHDistanceSkeleton = 1.0f;
    // Skeleton Global Variables
    [SerializeField] string default_skeleton_path;
    //[SerializeField] Animator animator;
    //[SerializeField] GameObject BoneRoot;
    Vector3[] Joints = new Vector3[17];
    Vector3[] DefaultBoneNormalized = new Vector3[12];
    public static Vector3[] BoneNormalized = new Vector3[12];
    Vector3[] LerpedNormalizeBone = new Vector3[12];
    Quaternion[] DefaultBoneRot = new Quaternion[17];
    Quaternion[] DefaultBoneLocalRot = new Quaternion[17];
    Vector3[] DefaultXAxis = new Vector3[17];
    Vector3[] DefaultYAxis = new Vector3[17];
    Vector3[] DefaultZAxis = new Vector3[17];
    public List<Transform> BoneList = new List<Transform>();
    int[,] BoneJointIdx = new int[,]
    { { 0, 2 }, { 2, 3 }, { 0, 5 }, { 5, 6 }, { 0, 7 }, { 7, 8 }, { 8, 9 }, { 9, 10 }, { 9, 12 }, { 12, 13 }, { 9, 15 }, { 15, 16 }
    };

    void Start()
    {
        // Step 1: Load and draw default skeleton
        // TODO
        // GetBones();
        // LoadSkeleton();
        Scale = ComputeScale();
        Debug.Log($"Default world scale: {Scale}");
    }
    
    void Update()
    {
        // Print out Instruction

        // Trigger option 1: Use keypoint Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Scale = ComputeScale();
            Debug.Log($"World scale measured: {Scale}");
            // Update scale of UI -- MAY DEPRECATE TO HAVE EACH FUNCTION UPDATE BY ITSELF

        }
    }

    public void Calibrate()
    {

    }

    float ComputeScale()
    {
        // Compute generic distance between left controller and HMD
        LHDistanceGeneric = ComputeGeneric();
        // Compute skeleton distance between left controller and HMD
        LHDistanceSkeleton = 1.0f; // ComputeSkeleton();                // TO BE CALLED AFTER SKELETON IS IMPORTED
        // Compute scale of skeleton, S
        return LHDistanceGeneric / LHDistanceSkeleton;
    }

    float ComputeGeneric()
    {
        // Load location of headset and left controller (default for right-handers)
        //LeftControllerPos = LeftController.transform.position;
        if(!cachedLeftControllerTransform)
        {
            cachedLeftControllerTransform = Core.Ins.XRManager.GetLeftDirectController().transform;
        }
        if(!cachedHMDTransform)
        {
            cachedHMDTransform = Core.Ins.XRManager.GetXrCamera().transform;
        }
        //LeftControllerPos = LeftControllerTransform.position;
        // RightControllerPos = RightController.transform.position;
        //HMDPos = HMD.transform.position;
        // Compute generic distance between HMD and left controller
        //return ComputeDistance(LeftControllerTransform.position, HMDTransform.position);
        return Vector3.Distance(cachedLeftControllerTransform.position, cachedHMDTransform.position);
    }

    float ComputeSkeleton()
    {
        // Load skeleton position
        LeftHandPos = BoneNormalized[(int)JointsIdx.LeftHand];
        HeadPos = BoneNormalized[(int)JointsIdx.Head];
        // Compute skeleton distance between HMD and left controller
        return Vector3.Distance(LeftHandPos, HeadPos);
    }

    //float ComputeDistance(Vector3 pos1, Vector3 pos2)
    //{
    //    float deltaX = pos1[0] - pos2[0];
    //    float deltaY = pos1[1] - pos2[1];
    //    float deltaZ = pos1[2] - pos2[2];
    //    return (float)Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    //}
    /*
    void GetBones()
    {
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.Hips));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.LeftFoot));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.RightUpperLeg));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.RightLowerLeg));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.RightFoot));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.Spine));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.Chest));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.Neck));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.Head));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.RightUpperArm));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.RightLowerArm));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.RightHand));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.LeftUpperArm));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.LeftLowerArm));
        BoneList.Add(animator.GetBoneTransform(HumanBodyBones.LeftHand));

        for (int i = 0; i < 17; i++)
        {
            var rootT = animator.GetBoneTransform(HumanBodyBones.Hips).root;
            DefaultBoneRot[i] = BoneList[i].rotation;
            DefaultBoneLocalRot[i] = BoneList[i].localRotation;
            DefaultXAxis[i] = new Vector3(
                Vector3.Dot(BoneList[i].right, rootT.right),
                Vector3.Dot(BoneList[i].up, rootT.right),
                Vector3.Dot(BoneList[i].forward, rootT.right)
            );
            DefaultYAxis[i] = new Vector3(
                Vector3.Dot(BoneList[i].right, rootT.up),
                Vector3.Dot(BoneList[i].up, rootT.up),
                Vector3.Dot(BoneList[i].forward, rootT.up)
            );
            DefaultZAxis[i] = new Vector3(
                Vector3.Dot(BoneList[i].right, rootT.forward),
                Vector3.Dot(BoneList[i].up, rootT.forward),
                Vector3.Dot(BoneList[i].forward, rootT.forward)
            );
        }
        for (int i = 0; i < 12; i++)
        {
            DefaultBoneNormalized[i] = (BoneList[BoneJointIdx[i, 1]].position - BoneList[BoneJointIdx[i, 0]].position).normalized;
        }
    }

    void LoadSkeleton()
    {
        StreamReader fi = new StreamReader(Application.dataPath + default_skeleton_path);
        string all = fi.ReadToEnd();
        if (all != "0")
        {
            string[] axis = all.Split(']');
            float[] x = axis[0].Replace("[", "").Replace("\r\n", "").Replace("\n", "").Split(' ').Where(s => s != "").Select(f => float.Parse(f)).ToArray();
            float[] y = axis[2].Replace("[", "").Replace("\r\n", "").Replace("\n", "").Split(' ').Where(s => s != "").Select(f => float.Parse(f)).ToArray();
            float[] z = axis[1].Replace("[", "").Replace("\r\n", "").Replace("\n", "").Split(' ').Where(s => s != "").Select(f => float.Parse(f)).ToArray();
            for (int i = 0; i < 17; i++) Joints[i] = new Vector3(x[i], y[i], -z[i]);
            for (int i = 0; i < 12; i++) BoneNormalized[i] = (Joints[BoneJointIdx[i, 1]] - Joints[BoneJointIdx[i, 0]]).normalized;
        }
        else
        {
            Debug.Log("Warning: All joint 0");
        }
    }
    */
}
