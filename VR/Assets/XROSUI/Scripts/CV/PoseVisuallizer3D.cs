using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.BlazePose;

public class PoseVisuallizer3D : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    //This script fetches image from a webcam
    //If it has a static input (a video), it will prioritize that
    [SerializeField] WebCamInput webCamInput;
    //This shows the webcaminput in the game, whether its live or from a video
    [SerializeField] RawImage inputImageUI;
    //this is the special shader they have written to help visualize the skeletons
    [SerializeField] Shader shader;
    [SerializeField] BlazePoseResource blazePoseResource;
    [SerializeField, Range(0, 1)] float humanExistThreshold = 0.5f;
    [SerializeField] BlazePoseModel poseLandmarkModel;

    Material material;
    BlazePoseDetecter detecter;

    public GameObject HeadRef;
    //public GameObject Head;
    //public GameObject HandL;
    //public GameObject HandR;
    public GameObject FootL;
    public GameObject FootR;
    public GameObject KneeL;
    public GameObject KneeR;
    //public GameObject ShoulderL;
    //public GameObject ShoulderR;

    // Lines count of body's topology.
    const int BODY_LINE_NUM = 35;
    // Pairs of vertex indices of the lines that make up body's topology.
    // Defined by the figure in https://google.github.io/mediapipe/solutions/pose.
    readonly List<Vector4> linePair = new List<Vector4>{
        new Vector4(0, 1), new Vector4(1, 2), new Vector4(2, 3), new Vector4(3, 7), new Vector4(0, 4), 
        new Vector4(4, 5), new Vector4(5, 6), new Vector4(6, 8), new Vector4(9, 10), new Vector4(11, 12), 
        new Vector4(11, 13), new Vector4(13, 15), new Vector4(15, 17), new Vector4(17, 19), new Vector4(19, 15), 
        new Vector4(15, 21), new Vector4(12, 14), new Vector4(14, 16), new Vector4(16, 18), new Vector4(18, 20), 
        new Vector4(20, 16), new Vector4(16, 22), new Vector4(11, 23), new Vector4(12, 24), new Vector4(23, 24), 
        new Vector4(23, 25), new Vector4(25, 27), new Vector4(27, 29), new Vector4(29, 31), new Vector4(31, 27), 
        new Vector4(24, 26), new Vector4(26, 28), new Vector4(28, 30), new Vector4(30, 32), new Vector4(32, 28)
    };


    void Start(){
        material = new Material(shader);
        detecter = new BlazePoseDetecter(blazePoseResource, poseLandmarkModel);
    }

    void Update(){
        //Powen: We don't need to constantly rotate the camera
        //mainCamera.transform.RotateAround(Vector3.zero, Vector3.up, 0.1f);

        //Waist.transform.position = Head.transform.position - new Vector3(0, 0.5f, 0);
    }

    void LateUpdate(){
        inputImageUI.texture = webCamInput.inputImageTexture;

        // Predict pose by neural network model.
        // Switchable anytime models with 2nd argment.
        detecter.ProcessImage(webCamInput.inputImageTexture, poseLandmarkModel);
    } 

    void OnRenderObject(){
        var data = new Vector4[detecter.vertexCount];
        detecter.worldLandmarkBuffer.GetData(data);


        var offX = data[0].x - HeadRef.transform.position[0];
        var offY = data[0].y - HeadRef.transform.position[1];
        var offZ = data[0].z - HeadRef.transform.position[2];

        for (var i = 0; i < detecter.vertexCount; i++)
        {
            data[i].x -= offX;          
            data[i].y -= offY;
            data[i].z -= offZ;

        }

        detecter.worldLandmarkBuffer.SetData(data);

        HeadRef.transform.position = data[0];
        //HandL.transform.position = data[16];
        //HandR.transform.position = data[15];
        FootL.transform.position = data[28] - new Vector4(0, 0.2f, 0, 0);
        FootR.transform.position = data[27] - new Vector4(0, 0.2f, 0, 0);
        KneeL.transform.position = data[26] - new Vector4(0, 0.2f, 0, 0);
        KneeR.transform.position = data[25] - new Vector4(0, 0.2f, 0, 0);
        //HipL.transform.position = data[24];
        //HipR.transform.position = data[23];


        // Set predicted pose world landmark results.
        material.SetBuffer("_worldVertices", detecter.worldLandmarkBuffer);
        // Set pose landmark counts.
        material.SetInt("_keypointCount", detecter.vertexCount);
        material.SetFloat("_humanExistThreshold", humanExistThreshold);
        material.SetVectorArray("_linePair", linePair);
        material.SetMatrix("_invViewMatrix", mainCamera.worldToCameraMatrix.inverse);

        // Draw 35 world body topology lines.
        material.SetPass(2);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, BODY_LINE_NUM);

        // Draw 33 world landmark points.
        material.SetPass(3);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, detecter.vertexCount);
    }

    void OnApplicationQuit(){
        // Must call Dispose method when no longer in use.
        detecter.Dispose();
    }
}
