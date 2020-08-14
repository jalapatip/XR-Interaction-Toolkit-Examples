using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VeFeedback : MonoBehaviour
{
    public TMP_Text Text_Status;

    public TMP_Text Text_Action;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     ChangeCurrentSelection(ENUM_XROS_Gesture.Up);
        // }
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     ChangeCurrentSelection(ENUM_XROS_Gesture.Down);
        // }
    }

    public void ChangeCurrentSelection(ENUM_XROS_Gesture gesture)
    {
        Text_Status.text = gesture.ToString();
        switch (gesture)
        {
            case ENUM_XROS_Gesture.Up:
                //Text_Status.text = gesture.ToString();
                Text_Action.text = "Volume Up";
                break;
            case ENUM_XROS_Gesture.Down:
                Text_Action.text = "Volume Down";
                break;
            case ENUM_XROS_Gesture.Forward:
                break;
            case ENUM_XROS_Gesture.Backward:
                break;
            case ENUM_XROS_Gesture.Left:
                break;
            case ENUM_XROS_Gesture.Right:
                break;
            case ENUM_XROS_Gesture.RotateClockwise:
                break;
            case ENUM_XROS_Gesture.RotateCounterclockwise:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gesture), gesture, null);
        }   
    }
}
