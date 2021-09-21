using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetLocation : MonoBehaviour
{
    private void OnEnable()
    {
        Core.Ins.XRManager.RegisterPredictedHead(this.gameObject);
    }
}
