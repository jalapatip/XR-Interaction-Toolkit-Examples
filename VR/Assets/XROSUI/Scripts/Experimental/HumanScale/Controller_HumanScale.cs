using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoneIdx
{
    Neck,
    Spine,
    RightHip,
    LeftHip,
    LeftUpperLeg,
    RightUpperLeg,
    LeftLowerLeg,
    RightLowerLeg,
    LeftFoot,
    RightFoot,
    LeftShoulder,
    RightShoulder,
    LeftLowerArm,
    LeftUpperArm,
    RightLowerArm,
    RightUpperArm
}

public class Controller_HumanScale : MonoBehaviour
{
    float leftArmLength = 0.635f;
    float eyeHeight = 1.6f;
    IDictionary<int, float> boneLengthDict = new Dictionary<int, float>();
    IDictionary<int, Vector3> jointPositionDict = new Dictionary<int, Vector3>();

    IDictionary<int, int[]> boneJointPairDict = new Dictionary<int, int[]>()
    {
        {(int) BoneIdx.Neck, new int[] {0, 1}},
        {(int) BoneIdx.Spine, new int[] {1, 8}},
        {(int) BoneIdx.RightHip, new int[] {8, 12}},
        {(int) BoneIdx.LeftHip, new int[] {8, 9}},
        {(int) BoneIdx.LeftUpperLeg, new int[] {9, 10}},
        {(int) BoneIdx.RightUpperLeg, new int[] {12, 13}},
        {(int) BoneIdx.LeftLowerLeg, new int[] {10, 11}},
        {(int) BoneIdx.RightLowerLeg, new int[] {13, 14}},
        {(int) BoneIdx.LeftFoot, new int[] {11, 15}},
        {(int) BoneIdx.RightFoot, new int[] {14, 16}},
        {(int) BoneIdx.LeftShoulder, new int[] {1, 2}},
        {(int) BoneIdx.RightShoulder, new int[] {1, 5}},
        {(int) BoneIdx.LeftLowerArm, new int[] {3, 4}},
        {(int) BoneIdx.LeftUpperArm, new int[] {2, 3}},
        {(int) BoneIdx.RightLowerArm, new int[] {6, 7}},
        {(int) BoneIdx.RightUpperArm, new int[] {5, 6}}
    };

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(BoneIdx)).Length; i++)
        {
            try
            {
                boneLengthDict.Add(i, 0.0f);
            }
            catch (Exception e)
            {
                Dev.LogWarning("" + e.Message);
            }
        }

        for (int i = 0; i < 17; i++)
        {
            try
            {
                jointPositionDict.Add(i, new Vector3(0.0f, 0.0f, 0.0f));
            }
            catch (Exception e)
            {
                Dev.LogWarning("" + e.Message);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //updateBoneLength();
    }

    public void SetBoneLength(int boneIdx, float newValue)
    {
        boneLengthDict[boneIdx] = newValue;
    }

    public float GetBoneLength(int boneIdx)
    {
        return boneLengthDict[boneIdx];
    }

    public float GetBoneLength(BoneIdx idx)
    {
        return GetBoneLength((int) idx);
    }

    public void SetJointPosition(int jointIdx, Vector3 newValue)
    {
        jointPositionDict[jointIdx] = newValue;
    }

    public Vector3 GetJointPosition(int jointIdx)
    {
        return jointPositionDict[jointIdx];
    }

    public void UpdateBoneLength()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(BoneIdx)).Length; i++)
        {
            SetBoneLength(i,
                computeBoneLength(jointPositionDict[boneJointPairDict[i][0]],
                    jointPositionDict[boneJointPairDict[i][1]]));
        }
    }

    public void DrawSkeleton()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(BoneIdx)).Length; i++)
        {
            Debug.DrawLine(jointPositionDict[boneJointPairDict[i][0]], jointPositionDict[boneJointPairDict[i][1]],
                Color.blue);
        }
    }

    float computeBoneLength(Vector3 joint1, Vector3 joint2)
    {
        float deltaX = joint1[0] - joint2[0];
        float deltaY = joint1[1] - joint2[1];
        float deltaZ = joint1[2] - joint2[2];
        return (float) Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    }

    public float GetEyeHeight()
    {
        return eyeHeight;
    }

    public void SetArmLength(float newLength)
    {
        leftArmLength = newLength;
    }

    public float GetArmLength()
    {
        return leftArmLength;
    }
}