using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAudioPO : VREquipment
{
    public override void HandleGesture(ENUM_XROS_Gesture gesture, float distance)
    {
        switch (gesture)
        {
            case ENUM_XROS_Gesture.up:
                this.gameObject.GetComponent<AudioSource>().volume += 0.1f;
                break;
            case ENUM_XROS_Gesture.down:
                this.gameObject.GetComponent<AudioSource>().volume -= 0.1f;
                break;
            case ENUM_XROS_Gesture.forward:
                break;
            case ENUM_XROS_Gesture.backward:
                break;
            case ENUM_XROS_Gesture.rotate_clockwise:
                break;
            case ENUM_XROS_Gesture.rotate_counterclockwise:
                break;
            default:
                break;
        }
    }

}
