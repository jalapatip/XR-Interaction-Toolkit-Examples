using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class Privacy_Camera_Observer : MonoBehaviour
{
    private TrackedPoseDriver _driver;

    public TMPro.TMP_Text text;
    private ENUM_XROS_PrivacyObserver observerType;

    void OnEnable()
    {
        Manager_Privacy.EVENT_NewPrivacyObserver += HandleObserverChange;
    }
    void OnDisable()
    {
        Manager_Privacy.EVENT_NewPrivacyObserver -= HandleObserverChange;
    }
    
    private void HandleObserverChange(ENUM_XROS_PrivacyObserver o, bool b)
    {
        text.text = o.ToString();
    }
    
}