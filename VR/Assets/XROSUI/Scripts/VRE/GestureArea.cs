using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureArea : MonoBehaviour
{
    public GameObject GestureCore;
    public GameObject Area;
    public GameObject GO_VE;
    public VREquipment VE;
    public float gestureDistance;
    public float DistanceY;
    public float DistanceZ;
    public float volume;
    public bool Y;
    public bool Z;
    public float coolDown = 0.5f;
    float lastAskTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.RegisterVREquipment(GO_VE.GetComponent<VREquipment>());
    }

    public void RegisterVREquipment(VREquipment vre)
    {
        this.VE = vre;
        this.GO_VE = vre.gameObject;
    }
    public void UnregisterVREquipment()
    {
        this.VE = null;
        this.GO_VE = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (VE.m_Held)
        {
            gestureDistance = Vector3.Distance(GestureCore.transform.position, GO_VE.transform.position);
            if (gestureDistance <= 0.5f * Area.transform.localScale.y && gestureDistance > 0)
            {
                //if(!Text.activeSelf)
                //{
                    //ShowValue
                    //last activated = Time.time;
                    //Text.SetActive(true);
                //}

                if (lastAskTime + coolDown < Time.time)
                {
                    MeasureDirect();
                    lastAskTime = Time.time;
                }
            }
        }



        /*
        if (Input.GetKey(KeyCode.Alpha7))
        {
            Dev.Log("[Debug] Register Equipment");
            GameObject go = GameObject.Find("Headphone2");
            this.RegisterVREquipment(go.GetComponent<VREquipment>());
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            Dev.Log("[Debug] Deregister Equipment");
            this.UnregisterVREquipment();
        }
        if (Input.GetKey(KeyCode.O))
        {
            Dev.Log("[Debug] Move Equipment Up");
            GO_VE.transform.position += Vector3.up;
        }
        if (Input.GetKey(KeyCode.L))
        {
            Dev.Log("[Debug] Move Equipment Down");
            Dev.Log(GO_VE.transform.position);
            GO_VE.transform.position += Vector3.down;
            Dev.Log(GO_VE.transform.position);
        }
        if (GO_VE)
        {
            if (Input.GetKey(KeyCode.Alpha9))
            {
                Dev.Log("test");
                MeasureDirect();
            }
        }
        */
    }

    public void MeasureDirect()
    {
        bool m_Direction;
        //detect the direction of user by the main camera.
        //if (Vector3.Dot(Camera.main.transform.forward, GO_VE.transform.forward) < 0.9)//not work
        if (Camera.main.transform.forward.z < 0f)
        {
            //back
            m_Direction = false;
            //Debug.Log("back");
        }
        else
        {
            //forward
            m_Direction = true;
            //Debug.Log("forward");
        }
        //up/down
        DistanceY = GO_VE.transform.position.y - GestureCore.transform.position.y;
        //forward/backward
        DistanceZ = GO_VE.transform.position.z - GestureCore.transform.position.z;

        if (Mathf.Abs(DistanceY) >= Mathf.Abs(DistanceZ))
        {
            if (DistanceY > 0)
            {
                //Y = true;
                this.VE.HandleGesture(ENUM_XROS_Gesture.Up, DistanceY);
                //Dev.Log("up");
            }
            else if (DistanceY < 0)
            {
                //Y = false;
                this.VE.HandleGesture(ENUM_XROS_Gesture.Down, DistanceY);
                //Dev.Log("down");
            }
            //else Dev.Log("no change");
        }
        else
        {
            if ((DistanceZ > 0 && m_Direction) || (DistanceZ < 0 && !m_Direction))
            //if (DistanceZ > 0)
            {
                //Z = true;
                this.VE.HandleGesture(ENUM_XROS_Gesture.Left,DistanceZ);
                //Dev.Log("left");
            }
            else if ((DistanceZ < 0 && m_Direction) || (DistanceZ > 0 && !m_Direction))
            //else if (DistanceZ < 0)
            {
                //Z = false;
                this.VE.HandleGesture(ENUM_XROS_Gesture.Right, DistanceZ);
                //Dev.Log("right");
            }
            //else Dev.Log("no change");
        }
    }
}
