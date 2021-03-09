using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_SaveCalibration : Switch_Base
{
    public MeasurementsV2 measurement;

    protected override void OnActivated(XRBaseInteractor obj)
    {
        measurement.SaveUserData();
    }
}
