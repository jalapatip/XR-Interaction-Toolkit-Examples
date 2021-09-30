using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VES_GestureFeedback : MonoBehaviour
{
    [Tooltip("Use TextMeshPro to display the type of Equipment Gesture being displayed")]
    public TMP_Text Text_Gesture;
    [Tooltip("Use TextMeshPro to display the type of Equipment Action being displayed")]
    public TMP_Text Text_Action;
    [Tooltip("Use TextMeshPro to display debug information")]
    public TMP_Text Text_Debug;

    public bool ShowDebug = false;

    private VE_EquipmentBase _veb;
    
    // Start is called before the first frame update
    void Start()
    {
        Text_Debug.transform.parent.gameObject.SetActive(ShowDebug);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowDebug = !ShowDebug;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeCurrentSelection(ENUM_XROS_EquipmentGesture.Up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeCurrentSelection(ENUM_XROS_EquipmentGesture.Down);
        }
    }

    private void LateUpdate()
    {
        // if (_veb)
        // {
        //     this.transform.position = _veb.GO_MirrorObject.transform.position;    
        // }
    }

    public void ChangeCurrentSelection(ENUM_XROS_EquipmentGesture equipmentGesture)
    {
        Text_Gesture.text = equipmentGesture.ToString();

        if (ShowDebug)
        {
            Text_Debug.text = "Debug";
        }
        switch (equipmentGesture)
        {
            case ENUM_XROS_EquipmentGesture.Up:
                //Text_Status.text = gesture.ToString();
                Text_Action.text = "Volume Up";
                break;
            case ENUM_XROS_EquipmentGesture.Down:
                Text_Action.text = "Volume Down";
                break;
            case ENUM_XROS_EquipmentGesture.Forward:
                break;
            case ENUM_XROS_EquipmentGesture.Backward:
                break;
            case ENUM_XROS_EquipmentGesture.Left:
                break;
            case ENUM_XROS_EquipmentGesture.Right:
                break;
            case ENUM_XROS_EquipmentGesture.RotateForward:
                break;
            case ENUM_XROS_EquipmentGesture.RotateBackward:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(equipmentGesture), equipmentGesture, null);
        }   
    }


    public void UpdateGestureFeedback(ENUM_XROS_EquipmentGesture equipmentGesture, VE_EquipmentBase veb)
    {
//        print(equipmentGesture.ToString());
//        print(veb.GetActionTooltip());
        //this.transform.position = veb.GO_MirrorObject.transform.position;
        this.transform.position = veb.assignedSocket.transform.position + Camera.main.transform.forward * 1f;
        
        Text_Gesture.text = equipmentGesture.ToString();
        Text_Action.text = veb.GetActionTooltip();
    }
}
