using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//Reference links
//Earlier attempts at brightness adjustment using RenderSettings.ambientLight - if you are in a dark room it wouldn't do anything
//https://youtu.be/MDvPNNgIu7k

//Current attempt at brightness using URP's Post Processing
//https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@7.1/manual/Volumes.html
//https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@7.1/manual/Post-Processing-Lift-Gamma-Gain.html
//https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@7.1/manual/Post-Processing-Color-Adjustments.html
//This can be done using lift, gamma, gain, or through the color adjustment setting. 

//Post-processing in URP for VR
///In VR apps and games, certain post-processing effects can cause nausea and disorientation. 
///To reduce motion sickness in fast-paced or high-speed apps, use the Vignette effect for VR, 
///and avoid the effects Lens Distortion, Chromatic Aberration, and Motion Blur for VR.

///PostProcessingVolume and Volume is different. Volume is for URP. PPV is for other pipelines such as HDRP?

///For Post Processing to work, Camera/Rendering/PostProcessing needs to be checked in the main camera.
 


public delegate void EventHandler_NewBrightness(float newValue);

/// <summary>
/// The goal of the Visual Manager is to keep track of commonly used visual related settings
/// 
/// Current Use:
/// Brightness for Virtual Equipment - Virtual Goggle
///
/// Planned:
/// Vignette Effect
/// Crossfade Effect
/// </summary>
public class Controller_Visual : MonoBehaviour
{
    public Animator CrossFadeAnimator;
    
    public static event EventHandler_NewBrightness EVENT_NewBrightness;
    public Volume volume;
    private VolumeProfile _vp;
    private LiftGammaGain _lgg;
    private ColorAdjustments _colorAdjustments;
    private float _lightIntensity = 0;

    float _minValue = -3;
    float _maxValue = 3;
    float _offsetValue = 0f;
    
    // Start is called before the first frame update
    private void Start()
    {
        if(volume)
        {
            _vp = volume.profile;
            // if (volume.profile.TryGet<LiftGammaGain>(out var tmp))
            // {
            //     _lgg = tmp;
            // }

            if (volume.profile.TryGet<ColorAdjustments>(out var tmp2))
            {
                _colorAdjustments = tmp2;
            }
        }
        else
        {
            Dev.LogError(this.name + " is not assigned Volume in the inspector");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DebugUpdate();

        if (timeToCrossfade != 0)
        {
            timeToCrossfade -= Time.deltaTime;
            CrossFadeAnimator.SetFloat("crossfadeTime", timeToCrossfade);
        }
    }

    private float timeToCrossfade = 0;

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Dev.Log("[Debug] CrossFade Trigger");
            PlayCrossfadeEffect(5);    
        }
        ////For Debugging
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Dev.Log("[DebugUpdate] " + this.name + ":  Alpha8");
            AdjustBrightness(-0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Dev.Log("[DebugUpdate] " + this.name + ":  Alpha9");
            AdjustBrightness(0.1f);
        }
    }

    /// <summary>
    /// Plays a crossfade effect for the duration specified
    /// </summary>
    /// <param name="time">in seconds</param>
    public void PlayCrossfadeEffect(float time)
    {
        timeToCrossfade = time;
        CrossFadeAnimator.SetFloat("crossfadeTime", timeToCrossfade);
        CrossFadeAnimator.SetTrigger("crossfadeTrigger");
    }
    
    private void AnimEvent_FadeComplete(int i)
    {
        //Dev.Log("complete " + i );
    }
    
    public void AdjustBrightness(float f)
    {
        _lightIntensity += f;
        Dev.Log("New Light: " + f);
        SetBrightness(_lightIntensity);
    }

    public float GetBrightness()
    {
        return _lightIntensity;
    }

    public void SetBrightness(float f)
    {
        if (f > _maxValue)
        {
            f = _maxValue;
        }
        else if (f < _minValue)
        {
            f = _minValue;
        }

        EVENT_NewBrightness?.Invoke(f);

        _lightIntensity = f;
        //if(lgg)
        //{
        //    lgg.gamma.SetValue(new Vector4Parameter(new Vector4(1, 1, 1, f - 1), true));
        //}
        if(_colorAdjustments)
        {
            var vpf = new VolumeParameter<float> {value = f - _offsetValue};
            _colorAdjustments.postExposure.SetValue(vpf);
        }
    }
}