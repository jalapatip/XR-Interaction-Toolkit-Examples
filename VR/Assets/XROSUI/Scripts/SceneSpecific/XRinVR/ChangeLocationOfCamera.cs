using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocationOfCamera : MonoBehaviour
{
    public List<GameObject> DestinationList = new List<GameObject>();

    public int currentLocationId = 0;

    // Update is called once per frame
    private void Update()
    {
        DebugInput();

        if (Core.Ins.ScenarioManager.GetFlag("FinishedCalibration") && currentLocationId == 1 &&
            Core.Ins.ScenarioManager.GetCurrentEventId() == 4)
        {
            if (Core.Ins.ScenarioManager.Waiting < 0.2f
            ) //doesn't work, cuz the timer for next event prohibits the teleportation.
            {
                MoveToLocation(1);
            }
        }

        if (Core.Ins.ScenarioManager.GetFlag("TurnOffKeyboard") && currentLocationId == 2)
        {
            if (Core.Ins.ScenarioManager.Waiting < 0.2f)
            {
                MoveToLocation(2);
            }
        }

        if (Core.Ins.ScenarioManager.GetFlag("FileGrabbed") && currentLocationId == 3)
        {
            if (Core.Ins.ScenarioManager.Waiting < 0.2f)
            {
                MoveToLocation(3);
            }
        }
    }

    //Track debug inputs here
    //https://docs.google.com/spreadsheets/d/1NMH43LMlbs5lggdhq4Pa4qQ569U1lr_O7HSHESEantU/edit#gid=0
    void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MoveToLocation(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MoveToLocation(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MoveToLocation(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MoveToLocation(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            MoveToLocation(4);
        }
    }

    public void ChangeLocation(int i)
    {
        currentLocationId++;
        MoveToLocation(this.currentLocationId);
    }

    private void AnimEvent_FadeComplete(int i)
    {
        print("complete " + i );
    }
    
    private void MoveToLocation(int locationId)
    {
        Core.Ins.VisualManager.PlayCrossfadeEffect(1);
        Core.Ins.XRManager.GetXrRig().transform.position = DestinationList[locationId].transform.position;
        Core.Ins.XRManager.GetXrRig().transform.forward = DestinationList[locationId].transform.forward;
    }
}