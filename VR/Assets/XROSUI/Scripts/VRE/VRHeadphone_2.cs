using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class VRHeadphone_2 : VREquipment
{
    public GameObject GestureCore;
    private float coolDown = 0.2f;
    private float lastAskTime = 0;

    public void Start()
    {

    }

    public override void OnActivated(XRBaseInteractor obj)
    {
        Core.Ins.AudioManager.PlayPauseMusic();
    }

    //public override void OnDeactivated(XRBaseInteractor obj)
    //{
    //}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Core.Ins.AudioManager.AdjustVolume(1, ENUM_Audio_Type.master);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Core.Ins.AudioManager.AdjustVolume(-1, ENUM_Audio_Type.master);
        }

        if (!this.m_Held)
        {
            ResetPosition();
        }
    }
    public override void HandleGesture(ENUM_XROS_Gesture gesture, float distance)
    {
        int scale = 10;
        if (lastAskTime + coolDown < Time.time)
        {
            lastAskTime = Time.time;
            switch (gesture)
            {
                case ENUM_XROS_Gesture.Up:
                case ENUM_XROS_Gesture.Down:
                    int increaseRate = (int)(distance * scale);
                    Core.Ins.AudioManager.AdjustVolume(increaseRate, ENUM_Audio_Type.master);
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
                    int freq = Math.Abs((int)(distance * 400));
                    Core.Ins.XRManager.SendHapticBuffer(freq);
                    break;
                case ENUM_XROS_Gesture.Forward:
                    break;
                case ENUM_XROS_Gesture.Backward:
                    break;
                case ENUM_XROS_Gesture.RotateClockwise:
                    break;
                case ENUM_XROS_Gesture.RotateCounterclockwise:
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetPosition()
    {
        this.transform.position = GestureCore.transform.position;
    }
}
