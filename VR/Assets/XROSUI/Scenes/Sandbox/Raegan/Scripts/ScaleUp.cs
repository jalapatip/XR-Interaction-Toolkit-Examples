using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class ScaleUp : MonoBehaviour

{


    private bool routineFlag1, routineFlag2;
    
    float lerpDuration = 1.5f;
    float startValue = 0;
    float endValue = 3f;
    float valueToLerp;
    public void Start()
    {
        routineFlag1 = false;
        routineFlag2 = false;
    }
    public void StartLerp()
    {
        StartCoroutine(Lerp());
       
    }
    public void StopLerp()
    {
        StartCoroutine(UnLerp());
    }
   IEnumerator Lerp()
    {

        if (routineFlag1) yield break;
        routineFlag1 = true;
        float timeElapsed = 0;

        Debug.Log("Go2");
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
        

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + valueToLerp * Time.deltaTime, transform.localScale.z);

          
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        valueToLerp = endValue;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + valueToLerp * Time.deltaTime, transform.localScale.z);
        routineFlag1 = false;
        StopAllCoroutines();

    }

    IEnumerator UnLerp()
    {
        if (routineFlag2) yield break;
        routineFlag2 = true;
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);


            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - valueToLerp * Time.deltaTime, transform.localScale.z);


            timeElapsed += Time.deltaTime;
            yield return null;
        }

        valueToLerp = endValue;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + valueToLerp * Time.deltaTime, transform.localScale.z);

        routineFlag2 = false;
        StopAllCoroutines();
    }
}
 

