using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class DataReplayHelperGesture : MonoBehaviour
{
    public GameObject ReplayHeadset;
    public GameObject ReplayHandR;
    public GameObject ReplayHandL;
    public int userID;
    public float height;
    public float LarmLength;
    public float RarmLength;
    public float Lshoulderx;
    public float Rshoulderx;
    public float Lshouldery;
    public float Rshouldery;
    public float chestWidth;
    public float Lelbowy;
    public float Relbowy;
    public float Lkneey;
    public float Rkneey;
    public GestureAgent agent;
    //public GameObject ReplayTracker;
    public string gesture;

    private int currentIndex = 0;
    public bool startPlayback = false;

    // Start is called before the first frame update
    void Start()
    {
        //yield return new WaitUntil(() => DataReplayManagerGesture.Ins.GetMaxIndex() > 0 == true);
        RandomPosition();
    }

    public int count = 0;
    void ModifyPosition()
    {
        //Debug.Log(" ManagerId " + DataReplayManagerGesture.Ins.GetInstanceID());
        if (currentIndex < DataReplayManagerGesture.Ins.GetMaxIndex() - 1)
        {
            currentIndex++; 
        }
        else
        {
            currentIndex = 0;
        }
        /*print("fileName: " + DataReplayManagerGesture.Ins.GetFileName());
        print("currentIndex: " + currentIndex);
        print("size " + DataReplayManagerGesture.Ins.GetMaxIndex());*/
        ReplayHeadset.transform.localPosition = DataReplayManagerGesture.Ins.GetPosition(currentIndex, ReplayDataType.head);
        ReplayHeadset.transform.localRotation = DataReplayManagerGesture.Ins.GetRotation(currentIndex, ReplayDataType.head);
        ReplayHandR.transform.localPosition = DataReplayManagerGesture.Ins.GetPosition(currentIndex, ReplayDataType.handR);
        ReplayHandR.transform.localRotation = DataReplayManagerGesture.Ins.GetRotation(currentIndex, ReplayDataType.handR);
        ReplayHandL.transform.localPosition = DataReplayManagerGesture.Ins.GetPosition(currentIndex, ReplayDataType.handL);
        ReplayHandL.transform.localRotation = DataReplayManagerGesture.Ins.GetRotation(currentIndex, ReplayDataType.handL);
        userID = DataReplayManagerGesture.Ins.GetUserID(currentIndex);
        height = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.height);
        LarmLength = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.LarmLength);
        RarmLength = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.RarmLenghth);
        Lshoulderx = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.Lshoulderx);
        Rshoulderx = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.Rshoulderx);
        Lshouldery = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.Lshouldery);
        Rshouldery = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.Rshouldery);
        chestWidth = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.chestWidth);
        Lelbowy = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.Lelbowy);
        Relbowy = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.Relbowy);
        Lkneey = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.Lkneey);
        Rkneey = DataReplayManagerGesture.Ins.GetUserdata(userID, UserDataType.Rkneey);
        //ReplayTracker.transform.localPosition = DataReplayManagerGesture.Ins.GetPosition(currentIndex, ReplayDataType.tracker1);;
        //ReplayTracker.transform.localRotation = DataReplayManagerGesture.Ins.GetRotation(currentIndex, ReplayDataType.tracker1);
        gesture = DataReplayManagerGesture.Ins.GetGesture(currentIndex);
        if (!gesture.Equals("None"))
        {
            agent.SetCorrectGesture(gesture);
            agent.RequestDecision();
            //count++;
            //print("actionCount: " + count);
        }
        /*print("currentIndex: " + currentIndex);
        print("headpos: " + ReplayHeadset.transform.localPosition);
        print("headrot: " + ReplayHeadset.transform.localRotation);
        print("handrpos: " + ReplayHandR.transform.localPosition);
        print("handrrot: " + ReplayHandR.transform.localRotation);
        print("handlpos: " + ReplayHandL.transform.localPosition);
        print("handlrot: " + ReplayHandL.transform.localRotation);
        print("gesture: " + gesture);*/
    }

    public void RandomPosition()
    {
        currentIndex = (int) UnityEngine.Random.Range(0, DataReplayManagerGesture.Ins.GetMaxIndex());
        while (currentIndex < DataReplayManagerGesture.Ins.GetMaxIndex() - 1 && DataReplayManagerGesture.Ins.GetGesture(currentIndex) == "None")
        {
            currentIndex++;
            if (currentIndex == DataReplayManagerGesture.Ins.GetMaxIndex() - 1)
            {
                currentIndex = 0;
            }
        }

        if (currentIndex > 90)
        {
            currentIndex = currentIndex - 90;
        }
        else
        {
            currentIndex = 0;
        }   
        ModifyPosition();
//        Debug.Log(currentIndex);
    }

    public string getGesture()
    {
        return gesture;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (startPlayback)
        {
            ModifyPosition();
        }
        //ModifyPosition();
        if (Input.GetKeyUp(KeyCode.L))
        {
            ModifyPosition();
        }

        if (Input.GetKeyUp(KeyCode.N))
        {
            RandomPosition();
        }
    }
}
