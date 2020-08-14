using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//This scripts shows what the xrRayInteractor is hovering over and what it has selected by displaying thee info on a
//TextMeshPro text component
//We use RequireComponent to ensure TMP_Txt exists
//We use Dev.CheckAssignment to ensure XRRayInteractor is assigned from Hierarchy
[RequireComponent(typeof(TMP_Text))]
public class UI_RayInteractorSelection : MonoBehaviour
{
    [TooltipAttribute("Assign using inspector from Hierarchy")]
    public XRRayInteractor xrRayInteractor;

    private TMP_Text _tmp_Text;

    private void OnEnable()
    {
        _tmp_Text = GetComponent<TMP_Text>();

        Dev.CheckAssignment(xrRayInteractor, transform);
    }

    // Update is called once per frame
    private void Update()
    {
        if (xrRayInteractor.isHoverActive)
        {
            var list = new List<XRBaseInteractable>();
            xrRayInteractor.GetValidTargets(list);

            if (list.Count > 0)
            {
                _tmp_Text.text = "Hovering over: ";
                foreach (var t in list)
                {
                    _tmp_Text.text += "\n" + t.name;
                }
            }
        }
        else if (xrRayInteractor.isSelectActive)
        {
            _tmp_Text.text = "Selected: " + xrRayInteractor.selectTarget.name;
        }
        else
        {
            _tmp_Text.text = "";
        }
    }
}