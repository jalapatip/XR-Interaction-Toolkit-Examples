using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUp : MonoBehaviour
    
{

    float index = 0;

    public bool gameStart;
    public void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x,0, transform.localScale.z);
        gameStart = StartandStop.InitGame;

        

    }
   /* float lerpDuration = 3;
    float startValue = 0;
    float endValue = 2;
    float valueToLerp;

    /*void Start()
        {
            StartCoroutine(Lerp());
        }

        IEnumerator Lerp()
        {
            float timeElapsed = 0;


            while (timeElapsed < lerpDuration)
            {
                valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;

            }

            valueToLerp = endValue;
        }





        */
    public float scaleSpeed = 1f;

    public void Update()
    {
        if (gameStart)
        {
            if (index < 1)
            {

                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.5f * Time.deltaTime, transform.localScale.z);
                index += 0.05f;
            }
            gameStart = false;
        }
    }


}
