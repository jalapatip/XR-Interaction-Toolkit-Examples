using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider_Visual : MonoBehaviour
{
    public float light_value = 0.0001f;

    public void SetLight(float f)
    {
        //m_light.SetLight(f);
        Core.Ins.VisualManager.SetBrightness(f);
        light_value = f;
    }
}
