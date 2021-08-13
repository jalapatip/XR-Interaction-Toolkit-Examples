using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_ChangeTargetGesture : Switch_Base
{
    [FormerlySerializedAs("GesturesToRecord")]
    public ENUM_XROS_EquipmentGesture TargetGestureToRecord = ENUM_XROS_EquipmentGesture.Up;
    private DataCollection_Exp1Gestures _exp1;
    
    void Start()
    {
        _exp1 = (DataCollection_Exp1Gestures) Core.Ins.DataCollection.currentExperiment;
    }

    protected override void OnActivated(XRBaseInteractor obj)
    {
        _exp1.ChangeExperimentType(TargetGestureToRecord);
    }
}
