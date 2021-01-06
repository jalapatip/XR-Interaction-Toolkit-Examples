using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

//
//Powen: Alternate headphone with vibation.
//Vibration may only work with Oculus
//Originally written by Peggy
//
[RequireComponent(typeof(XRGrabInteractable))]
public class VE_HeadphoneV1A2 : VE_EquipmentBase
{
    [FormerlySerializedAs("coolDown")]
    public float gestureCooldown = 0.3f;
    public float adjustFactor = 10f;
    
    private float _lastAskTime = 0;


    public void Start()
    {

    }

    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
        Core.Ins.AudioManager.PlayPauseMusic();
    }

    public new void Update()
    {
        base.Update();
        
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Core.Ins.AudioManager.AdjustVolume(1, ENUM_Audio_Type.Master);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Core.Ins.AudioManager.AdjustVolume(-1, ENUM_Audio_Type.Master);
        }
    }
    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        base.HandleGesture(equipmentGesture, distance);
        if (_lastAskTime + gestureCooldown < Time.time)
        {
            _lastAskTime = Time.time;
            switch (equipmentGesture)
            {
                case ENUM_XROS_EquipmentGesture.Up:
                case ENUM_XROS_EquipmentGesture.Down:
                    var increaseRate = (int)(distance * adjustFactor);
                    Core.Ins.AudioManager.AdjustVolume(increaseRate, ENUM_Audio_Type.Master);
                    /*
                    ENUM_XROS_VibrationLevel level;
                    if (Math.Abs(increaseRate) <= 1)
                        level = ENUM_XROS_VibrationLevel.light;
                    else if (Math.Abs(increaseRate) == 2)
                        level = ENUM_XROS_VibrationLevel.middle;
                    else
                        level = ENUM_XROS_VibrationLevel.heavy;
                    Core.Ins.XRManager.SendHapticImpulse(level, 0.2f);
                    */
                    var freq = Math.Abs((int)(distance * 400));
                    Core.Ins.XRManager.SendHapticBuffer(freq);
                    break;
                case ENUM_XROS_EquipmentGesture.Forward:
                    break;
                case ENUM_XROS_EquipmentGesture.Backward:
                    break;
                case ENUM_XROS_EquipmentGesture.RotateClockwise:
                    break;
                case ENUM_XROS_EquipmentGesture.RotateCounterclockwise:
                    break;
                case ENUM_XROS_EquipmentGesture.Left:
                    break;
                case ENUM_XROS_EquipmentGesture.Right:
                    break;
                default:
                    break;
            }
        }
    }
}
