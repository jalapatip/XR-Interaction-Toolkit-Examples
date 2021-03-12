using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Label_ShowCurrentGestureGoal : MonoBehaviour
{
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        DataCollection_ExpGestures exp = (DataCollection_ExpGestures)Core.Ins.DataCollection.currentExperiment;
        text.text = "Current Type: " + exp.gesture.ToString() + "\n" + exp.GetTotalEntries();
    }
}
