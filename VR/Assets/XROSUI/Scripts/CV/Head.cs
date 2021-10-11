using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mediapipe.BlazePose;

public class Head : MonoBehaviour
{
    [SerializeField] private Transform rootObject, followObject;
    [SerializeField] private Vector3 positionOffset, rotationOffset;

    //[SerializeField] WebCamInput webCamInput;
    //[SerializeField] RawImage inputImageUI;
    //[SerializeField] Shader shader;
    //[SerializeField] BlazePoseResource blazePoseResource;
    //[SerializeField, Range(0, 1)] float humanExistThreshold = 0.5f;
    //[SerializeField] BlazePoseModel poseLandmarkModel;

    //// Lines count of body's topology.
    //const int BODY_LINE_NUM = 35;
    //// Pairs of vertex indices of the lines that make up body's topology.
    //// Defined by the figure in https://google.github.io/mediapipe/solutions/pose.
    //readonly List<Vector4> linePair = new List<Vector4>{
    //    new Vector4(0, 1), new Vector4(1, 2), new Vector4(2, 3), new Vector4(3, 7), new Vector4(0, 4),
    //    new Vector4(4, 5), new Vector4(5, 6), new Vector4(6, 8), new Vector4(9, 10), new Vector4(11, 12),
    //    new Vector4(11, 13), new Vector4(13, 15), new Vector4(15, 17), new Vector4(17, 19), new Vector4(19, 15),
    //    new Vector4(15, 21), new Vector4(12, 14), new Vector4(14, 16), new Vector4(16, 18), new Vector4(18, 20),
    //    new Vector4(20, 16), new Vector4(16, 22), new Vector4(11, 23), new Vector4(12, 24), new Vector4(23, 24),
    //    new Vector4(23, 25), new Vector4(25, 27), new Vector4(27, 29), new Vector4(29, 31), new Vector4(31, 27),
    //    new Vector4(24, 26), new Vector4(26, 28), new Vector4(28, 30), new Vector4(30, 32), new Vector4(32, 28)
    //};

    //BlazePoseDetecter detecter;


    //public GameObject footRight;
    //public GameObject footLeft;

    private Vector3 _headBodyOffset;


    void Start()
    {
        _headBodyOffset = rootObject.position - followObject.position;

        //detecter = new BlazePoseDetecter(blazePoseResource, poseLandmarkModel);

    }


    void LateUpdate()
    {
        //inputImageUI.texture = webCamInput.inputImageTexture;
        //detecter.ProcessImage(webCamInput.inputImageTexture, poseLandmarkModel);

        //var data = new Vector4[detecter.vertexCount];
        //detecter.worldLandmarkBuffer.GetData(data);

        //var offX = data[0].x - data[30].x;
        //var offY = data[0].y - data[30].y;
        //var offZ = data[0].z - data[30].z;

        //Debug.Log("X: " + offX + ", Y: " + offY + ", Z: " + offZ);

        rootObject.position = transform.position + _headBodyOffset;
        rootObject.forward = Vector3.ProjectOnPlane(followObject.up, Vector3.up).normalized;

        transform.position = followObject.TransformPoint(positionOffset);
        transform.rotation = followObject.rotation * Quaternion.Euler(rotationOffset);

        //var fixedFootX_Mocap = rootObject.position.x - footLeft.transform.position.x;
        //var fixedFootY_Mocap = rootObject.position.y - footLeft.transform.position.y;
        //var fixedFootZ_Mocap = rootObject.position.z - footLeft.transform.position.z;


    }

}
