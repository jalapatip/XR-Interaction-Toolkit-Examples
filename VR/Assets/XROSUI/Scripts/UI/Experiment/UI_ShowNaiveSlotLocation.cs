using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ShowNaiveSlotLocation : MonoBehaviour
{
    public DataCollection_Exp2Predict predictModule;

    //public GameObject GameObjectToShow;

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
        //text.text =  //how to access current slot prediction?
        //text.text = predictModule.GetPredictionAsString();
    }

    public void ChangeNaiveSlotLocation(string s)
    {
        text.text = "Naive: " + s;
        
        if (predictModule)
        {
            //predictModule.ReportNaiveSlotLocation();
            predictModule.NaivePredictionString = s;
        }
    }
}