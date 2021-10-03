using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHD_Microwave : SmartHomeDevice
{
    // Start is called before the first frame update
    void Start()
    {
        SmartHomeManager shm = (SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment();
        shm.AddTarget("Say 'open' while looking at it", this);
        shm.AddTarget("Say 'open' while pointing at it", this);
        shm.AddTarget("Say 'open microwave'", this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
