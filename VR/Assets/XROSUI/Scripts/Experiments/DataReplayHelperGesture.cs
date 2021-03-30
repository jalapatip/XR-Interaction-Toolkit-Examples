using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class DataReplayHelperGesture : MonoBehaviour
{
    public GameObject ReplayHeadset;
    public GameObject ReplayHandR;
    public GameObject ReplayHandL;
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

    void ModifyPosition()
    {
        if (currentIndex < DataReplayManagerGesture.Ins.GetMaxIndex() - 1)
        {
            currentIndex++; 
        }
        else
        {
            currentIndex = 0;
        }
        print("fileName: " + DataReplayManagerGesture.Ins.GetFileName());
        print("currentIndex: " + currentIndex);
        print("size " + DataReplayManagerGesture.Ins.GetMaxIndex());
        ReplayHeadset.transform.localPosition = DataReplayManagerGesture.Ins.GetPosition(currentIndex, ReplayDataType.head);
        ReplayHeadset.transform.localRotation = DataReplayManagerGesture.Ins.GetRotation(currentIndex, ReplayDataType.head);
        ReplayHandR.transform.localPosition = DataReplayManagerGesture.Ins.GetPosition(currentIndex, ReplayDataType.handR);
        ReplayHandR.transform.localRotation = DataReplayManagerGesture.Ins.GetRotation(currentIndex, ReplayDataType.handR);
        ReplayHandL.transform.localPosition = DataReplayManagerGesture.Ins.GetPosition(currentIndex, ReplayDataType.handL);
        ReplayHandL.transform.localRotation = DataReplayManagerGesture.Ins.GetRotation(currentIndex, ReplayDataType.handL);
        //ReplayTracker.transform.localPosition = DataReplayManagerGesture.Ins.GetPosition(currentIndex, ReplayDataType.tracker1);;
        //ReplayTracker.transform.localRotation = DataReplayManagerGesture.Ins.GetRotation(currentIndex, ReplayDataType.tracker1);
        gesture = DataReplayManagerGesture.Ins.GetGesture(currentIndex);
       
    }

    public void RandomPosition()
    {
        currentIndex = (int) UnityEngine.Random.Range(0, DataReplayManagerGesture.Ins.GetMaxIndex());
        /*while (currentIndex < DataReplayManagerGesture.Ins.GetMaxIndex() - 1 && DataReplayManagerGesture.Ins.GetGesture(currentIndex) == "None")
        {
            currentIndex++;
            if (currentIndex == DataReplayManagerGesture.Ins.GetMaxIndex() - 1)
            {
                currentIndex = 0;
            }
        }

        if (currentIndex > 10)
        {
            currentIndex = currentIndex - 10;
        }
        else
        {
            currentIndex = 0;
        }   */ 
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
