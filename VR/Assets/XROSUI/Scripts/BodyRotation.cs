using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BodyRotation : MonoBehaviour
{
    public GameObject HeadLocation;

    public Vector3 Offset = new Vector3(0, -0.5f, 0);
    // Start is called before the first frame update
    void Start()
    {
        if (!HeadLocation)
        {
            HeadLocation = Core.Ins.XRManager.GetXrCamera().gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
//        print("Head Rotation: " + HeadLocation2.GetComponent<Transform>().eulerAngles);
//        print("Head Rotation2: " + HeadLocation2.GetComponent<Transform>().localEulerAngles);

        var rot = HeadLocation.transform.localEulerAngles;
        var pot = HeadLocation.transform.localPosition;
//        print(rot);
        //this.transform.localEulerAngles = new Vector3(rot.x, 0, rot.z);
        
        this.transform.localEulerAngles = new Vector3(0, rot.y, 0);
        this.transform.localPosition = Offset + pot;
    }
}
