using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Label_ShowCurrentExperimentGoal : MonoBehaviour
{
    public TMP_Text text;
    
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
        text.text = Core.Ins.DataCollection.currentExperiment.GetGoalString();
    }
}
