using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ShowPosRot : MonoBehaviour
{
    public GameObject GameObjectToShow;

    public TMP_Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Pos: " + GameObjectToShow.transform.position.ToString() + 
                    "\nRot: " + GameObjectToShow.transform.localEulerAngles.ToString();
    }
}
