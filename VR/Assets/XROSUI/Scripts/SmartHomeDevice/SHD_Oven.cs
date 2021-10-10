using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHD_Oven : SmartHomeDevice
{
    public Animator openAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        SmartHomeManager shm = (SmartHomeManager)Core.Ins.DataCollection.GetCurrentExperiment();
        shm.AddTarget("Say 'open' while looking at it", this);
        shm.AddTarget("Say 'open' while pointing at it", this);
        shm.AddTarget("Say 'open oven'", this);
        
        openAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            OpenDevice(true);
        }
        
    }

    public override void OpenDevice(bool b)
    {
        Debug.Log("Open Device " + Time.time);
        print("OpenDevice - Oven");
        openAnimator.SetTrigger("Open");
    }
}
