using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SpatialTracking;

public class Privacy_Toggler : MonoBehaviour
{
    [FormerlySerializedAs("AnatomyParts")]
    [SerializeField]
    private ENUM_XROS_AnatomyParts anatomyParts;

    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;
        toggle.onValueChanged.AddListener(SubmitAnatomyChange);
    }

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts e, bool b)
    {
        if (e != anatomyParts) return;

        if (toggle)
        {
            toggle.isOn = b;
        }
    }

    private void SubmitAnatomyChange(bool b)
    {
        Core.Ins.Privacy.ToggleAnatomyPart(anatomyParts, b);
    }
    // Update is called once per frame
    void Update()
    {
    }
}