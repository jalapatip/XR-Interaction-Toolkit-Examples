using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_FollowXRRigV2 : MonoBehaviour
{
    public GameObject GO_XRRigCamera;

    public float UpdateFrequencyInSeconds = 0.05f;
    public float DistanceToStopMoving = 0.3f;
    //public float distanceInFrontOfPlayer = 0.5f;
    public Vector3 OffsetDistance;
    public float speed = 1.0f;
    private Vector3 startPositon;
    private Vector3 goalPosition;
    private float startTime;
    private float journeyLength;

    // Start is called before the first frame update
    void Start()
    {
        //GO_XRRigCamera = Core.Ins.XRManager.GetCamera().gameObject;
        GO_XRRigCamera = Camera.main.gameObject;
        this.transform.position = GetGoalPosition()+Vector3.up*0.01f;
        this.transform.LookAt(GO_XRRigCamera.transform);

        //goalPosition = this.transform.position;
        SetNewGoal();
    }

    private void OnEnable()
    {
        //GO_XRRigCamera = Camera.main.gameObject;
        Start();
    }
    private Vector3 GetGoalPosition()
    {
        if(GO_XRRigCamera)
        {

        return GO_XRRigCamera.transform.position
            + GO_XRRigCamera.transform.forward * OffsetDistance.z + GO_XRRigCamera.transform.right * OffsetDistance.x
            + GO_XRRigCamera.transform.up * OffsetDistance.y;
        }

        return this.transform.position;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!_IsFollowing)
        {
            return;
        }
        //this.transform.LookAt(GO_XRRigCamera.transform);
        Vector3 newGoalPosition = GetGoalPosition();

        //print(Vector3.Distance(newGoalPosition, this.transform.position));
        if (Vector3.Distance(newGoalPosition, this.transform.position) > DistanceToStopMoving)
        {
            if (startTime + UpdateFrequencyInSeconds < Time.time)
            {
                SetNewGoal();
            }
            FollowCamera();
        }
    }

    void SetNewGoal()
    {
        //print("Set new goal");
        //GoalPosition = GO_XRRigCamera.transform.position;
        startPositon = this.transform.position;
        goalPosition = GetGoalPosition();
        journeyLength = Vector3.Distance(startPositon, goalPosition);
        startTime = Time.time;
    }

    void FollowCamera()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        //print("fraction: " + fractionOfJourney);

        this.transform.position = Vector3.Lerp(startPositon, goalPosition, fractionOfJourney);
        //this.transform.position = GO_XRRigCamera.transform.position + GO_XRRigCamera.transform.forward * distanceInFrontOfPlayer;
        this.transform.LookAt(GO_XRRigCamera.transform);
    }

    private bool _IsFollowing = true;
    public void ToggleFollowState()
    {
        _IsFollowing = !_IsFollowing;
    }
}
