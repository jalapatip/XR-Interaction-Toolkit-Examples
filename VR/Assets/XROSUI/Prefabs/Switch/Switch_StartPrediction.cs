using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_StartPrediction : Switch_Base
{
    public DataCollection_Exp2Predict predictModule;
    protected override void OnActivated(XRBaseInteractor obj)
    {
        Dev.Log("StartPrediction: Activate");
        predictModule.PredictSlot();
    }
}
