using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class ScaleUp : MonoBehaviour

{
    float index = 0;

    public bool gameStart;

    public void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
    }

    float lerpDuration = 3;
    float startValue = 0;
    float endValue = 2;
    float valueToLerp;


    IEnumerator Lerp()
    {
        float timeElapsed = 0;
     

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
        

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + valueToLerp * Time.deltaTime, transform.localScale.z);

          
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        valueToLerp = endValue;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + valueToLerp * Time.deltaTime, transform.localScale.z);

        wallComplete = true;
    }


    public float scaleSpeed = 1f;
    private bool wallComplete = false;

    public void Update()
    {
        if (!wallComplete)
        {
         
            StartCoroutine(Lerp());
        }
        else
        {
           
        }
    }
}