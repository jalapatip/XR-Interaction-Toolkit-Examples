using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataReplayHelper : MonoBehaviour
{
    public GameObject ReplayHeadset;
    public GameObject ReplayHandR;
    public GameObject ReplayHandL;
    public GameObject ReplayTracker;

    private int currentIndex = 0;
    public bool startPlayback = false;

    // Start is called before the first frame update
    void Start()
    {
        RandomPosition();
    }

    void ModifyPosition()
    {
        
        if (currentIndex < DataReplayManager.Ins.GetMaxIndex())
        {
            //print("currentIndex: " + currentIndex + " at " + Time.time);
            ReplayHeadset.transform.localPosition = DataReplayManager.Ins.GetPosition(currentIndex, ReplayDataType.head);
            ReplayHeadset.transform.localRotation = DataReplayManager.Ins.GetRotation(currentIndex, ReplayDataType.head);
            ReplayHandR.transform.localPosition = DataReplayManager.Ins.GetPosition(currentIndex, ReplayDataType.handR);
            ReplayHandR.transform.localRotation = DataReplayManager.Ins.GetRotation(currentIndex, ReplayDataType.handR);
            ReplayHandL.transform.localPosition = DataReplayManager.Ins.GetPosition(currentIndex, ReplayDataType.handL);
            ReplayHandL.transform.localRotation = DataReplayManager.Ins.GetRotation(currentIndex, ReplayDataType.handL);
            ReplayTracker.transform.localPosition = DataReplayManager.Ins.GetPosition(currentIndex, ReplayDataType.tracker1);;
            ReplayTracker.transform.localRotation = DataReplayManager.Ins.GetRotation(currentIndex, ReplayDataType.tracker1);

            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
    }

    public void RandomPosition()
    {
        currentIndex = (int) UnityEngine.Random.Range(0, DataReplayManager.Ins.GetMaxIndex());
//        Debug.Log(currentIndex);
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
