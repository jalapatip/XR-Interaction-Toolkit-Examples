using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//This scripts shows what the xrRayInteractor is hovering over and what it has selected by displaying the info on a
//TextMeshPro text component
//We use Dev.CheckAssignment to ensure XRRayInteractor is assigned from Hierarchy
public class UI_RayInteractorSelection : MonoBehaviour
{
    [TooltipAttribute("Assign using inspector from Hierarchy")]
    public XRRayInteractor xrRayInteractor;

    [TooltipAttribute("Assign in the Prefab")]
    public TMP_Text _tmp_Text;

    [TooltipAttribute("Assign in the Prefab")]
    public UnityEngine.UI.Image UIPanel;

    private void OnEnable()
    {
        Dev.CheckAssignment(xrRayInteractor, transform);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSelectionText();
    }

    private void UpdateSelectionText()
    {
        if (xrRayInteractor.isHoverActive)
        {
            var list = new List<XRBaseInteractable>();
            xrRayInteractor.GetValidTargets(list);

            if (list.Count > 0)
            {
                UIPanel.enabled = true;
                _tmp_Text.text = "Hovering over: ";
                foreach (var t in list)
                {
                    _tmp_Text.text += "\n" + t.name;
                }
            }
            else
            {
                _tmp_Text.text = string.Empty;
                UIPanel.enabled = false;
            }
        }
        else if (xrRayInteractor.isSelectActive)
        {
            _tmp_Text.text = "Selected: " + xrRayInteractor.selectTarget.name;
            UIPanel.enabled = true;
        }
        else
        {
            //_tmp_Text.text = string.Empty;
            UIPanel.enabled = false;
        }
    }
}