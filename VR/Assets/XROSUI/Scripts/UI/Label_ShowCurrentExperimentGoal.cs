using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Label_ShowCurrentExperimentGoal : MonoBehaviour
{
    private TMP_Text text;
    private DataCollection_ExpBase exp;
    
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TMP_Text>();
        exp = Core.Ins.DataCollection.currentExperiment;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = exp.GetGoalString();
    }
}
