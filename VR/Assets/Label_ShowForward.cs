using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Label_ShowForward : MonoBehaviour
{
    public TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad) * Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad);
        float yPos = Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad);
        float zPos = Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad) * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);

        
        //https://www.reddit.com/r/gamedev/comments/bje7vd/how_to_calculate_an_objects_normalized_forward/
        var eulerAngles = this.transform.eulerAngles;
        float calculatedForwardX = Mathf.Cos(eulerAngles.x) * Mathf.Cos(eulerAngles.y);
        float calculatedForwardY = Mathf.Cos(eulerAngles.x) * Mathf.Sin(eulerAngles.y);
        float calculatedForwardZ = Mathf.Sin(eulerAngles.x);
        _text.text = "Unity Forward: " + this.transform.forward +
                     "\nCalculated Forward: \n" + xPos + ", " + yPos + "," + zPos;
    }
}