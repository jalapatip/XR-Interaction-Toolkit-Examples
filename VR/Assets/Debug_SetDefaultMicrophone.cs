using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_SetDefaultMicrophone : MonoBehaviour
{
    public int currentDeviceId;

    // Start is called before the first frame update
    void Start()
    {
        Core.Ins.Microphone.SetDeviceById(currentDeviceId);
    }

    // Update is called once per frame
    void Update()
    {
    }
}