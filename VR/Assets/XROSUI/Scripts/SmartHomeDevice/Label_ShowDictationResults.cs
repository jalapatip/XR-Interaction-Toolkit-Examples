using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Label_ShowDictationResults : MonoBehaviour
{
    public TMP_Text text;
    private string prev_uttr;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!text)
        {
            text = this.GetComponent<TMP_Text>();    
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Todo change this to event driven
        text.text =
            "Status: " + Core.Ins.Microphone.DictationStatus() +
            "\nHypothesis: " + Core.Ins.Microphone.GetCurrentUtteranceHypothesis() +
            "\nUtterance: " + Core.Ins.Microphone.GetCurrentUtterance();

        // var curr_uttr = Core.Ins.Microphone.GetCurrentUtterance();
        // if (curr_uttr != null)
        // {
        //     if (prev_uttr == null)
        //     {
        //         prev_uttr = curr_uttr;
        //         print(Core.Ins.Microphone.GetCurrentUtterance());
        //     }
        //     else if (prev_uttr != curr_uttr)
        //     {
        //         prev_uttr = curr_uttr;
        //         print(Core.Ins.Microphone.GetCurrentUtterance());
        //     }
        // }
    }
}
