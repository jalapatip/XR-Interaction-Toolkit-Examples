using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Label_ShowCurrentTypeGoal : MonoBehaviour
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
        DataCollection_ExpTyping exp = (DataCollection_ExpTyping)Core.Ins.DataCollection.currentExperiment;
        text.text = "Enter Key: " + exp.GetTargetKey() + "\nTotal Entries: " + exp.GetTotalEntries();
    }
}