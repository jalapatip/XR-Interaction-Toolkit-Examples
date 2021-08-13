using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ShowSlotLocation : MonoBehaviour
{
    public DataCollection_Exp2Predict predictModule;
    
    public GameObject GameObjectToShow;

    public TMP_Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //text.text =  //how to access current slot prediction?
        text.text = "Prediction: " + predictModule.GetPredictionAsString() + "\n" + predictModule.GetPredictionTableString();
    }
}
