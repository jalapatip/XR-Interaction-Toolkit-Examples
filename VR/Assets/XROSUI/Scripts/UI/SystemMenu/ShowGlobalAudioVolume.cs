using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowGlobalAudioVolume : ShowValue
{
    // Start is called before the first frame update
    private void Start()
    {
        //As this is 
        Controller_Audio.EVENT_NewVolumeMaster += HandleValueChange;
    }

    protected override string FormatValue(float f)
    {
        return "Volume: " + ((int) (f * 100f)).ToString() + "%";
    }
}