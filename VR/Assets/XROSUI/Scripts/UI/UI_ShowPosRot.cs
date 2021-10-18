using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This simple script is used to show the position and rotation of a assigned GameObject
///
/// This can be used to provide feedback while in Virtual Reality
/// </summary>
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
        text.text = "GO: " + GameObjectToShow.name +
                    "\nPos: " + GameObjectToShow.transform.position.ToString() + 
                    "\nRot: " + GameObjectToShow.transform.localEulerAngles.ToString();
    }
}
