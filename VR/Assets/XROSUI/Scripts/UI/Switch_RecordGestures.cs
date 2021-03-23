using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_RecordGestures : Switch_Base
{
    public ENUM_XROS_EquipmentGesture GesturesToRecord = ENUM_XROS_EquipmentGesture.Up;
    
    protected override void OnActivated(XRBaseInteractor obj)
    {
        DataCollection_ExpGestures exp = (DataCollection_ExpGestures) Core.Ins.DataCollection.currentExperiment;
        exp.ChangeExperimentType(GesturesToRecord);
    }
}
