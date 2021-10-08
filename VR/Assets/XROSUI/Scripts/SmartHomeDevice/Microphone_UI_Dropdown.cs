using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Microphone_UI_Dropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private void OnEnable()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        Manager_Microphone.Event_NewMicrophoneList += UpdateDropdownList;
        _dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(_dropdown); });
    }

    private void OnEDisable()
    {
        Manager_Microphone.Event_NewMicrophoneList -= UpdateDropdownList;
        _dropdown.onValueChanged.RemoveListener(delegate { DropdownValueChanged(_dropdown); });
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    //Create a List of new Dropdown options
    List<string> _dropOptions = new List<string> { "Option 1", "Option 2" };

    private void UpdateDropdownList()
    {
        _dropOptions = new List<string>(Core.Ins.Microphone.GetDeviceList());
        //Clear the old options of the Dropdown menu
        _dropdown.ClearOptions();
        //Add the options created in the List above
        _dropdown.AddOptions(_dropOptions);
    }

    private void UpdateCurrentDropdownSelection()
    {
        _dropdown.value = Core.Ins.Microphone.GetSelectedDeviceId();
    }

    //When dropdown value changes from user interaction, set the microphone in use, 
    private void DropdownValueChanged(TMP_Dropdown _change)
    {
        Core.Ins.Microphone.SetNewSelectedDeviceById(_change.value);
    }
}