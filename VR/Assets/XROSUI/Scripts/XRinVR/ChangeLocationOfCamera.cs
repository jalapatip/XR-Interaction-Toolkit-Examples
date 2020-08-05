using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocationOfCamera : MonoBehaviour
{
    public List<GameObject> DestinationList = new List<GameObject>();

    public int currentLocationId = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

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
                //anim.SetBool("fadeOut", true);
                MoveToLocation(1);
            }
        }

        if (Core.Ins.ScenarioManager.GetFlag("TurnOffKeyboard") && currentLocationId == 2)
        {
            if (Core.Ins.ScenarioManager.Waiting < 0.2f)
            {
                //anim.SetBool("fadeOut", true);
                MoveToLocation(2);
            }
        }

        if (Core.Ins.ScenarioManager.GetFlag("FileGrabbed") && currentLocationId == 3)
        {
            if (Core.Ins.ScenarioManager.Waiting < 0.2f)
            {
                //anim.SetBool("fadeOut", true);
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
        // Debug.Log("change location!!");

        currentLocationId++;
        MoveToLocation(this.currentLocationId);
        // Core.Ins.ScenarioManager.SetFlag("position"+currentLocationId,true);
    }

    private void AnimEvent_FadeComplete(int i)
    {
        print("complete " + i );
    }
    
    private void MoveToLocation(int locationId)
    {
        Core.Ins.VisualManager.PlayCrossfadeEffect(1);
        Core.Ins.XRManager.GetXRRig().transform.position = DestinationList[locationId].transform.position;
        Core.Ins.XRManager.GetXRRig().transform.forward = DestinationList[locationId].transform.forward;
    }
}