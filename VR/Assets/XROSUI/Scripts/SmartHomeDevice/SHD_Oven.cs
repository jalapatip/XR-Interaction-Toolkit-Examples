using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHD_Oven : SmartHomeDevice
{

    // Start is called before the first frame update
    void Start()
    {
        SmartHomeManager shm = (SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment();
        shm.AddTarget("Say 'open' while looking at it", this);
        shm.AddTarget("Say 'open' while pointing at it", this);
        shm.AddTarget("Say 'open oven'", this);
    }

    // Update is called once per frame
    void Update()
    {
        DebugUpdate();
    }

    void DebugUpdate()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            this.OpenDevice(true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            this.OpenDevice(false);
        }
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            this.TurnOnLight(true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            this.TurnOnLight(false);
        }
    }

    // public override void OpenDevice(bool b)
    // {
    //
    // }
}
