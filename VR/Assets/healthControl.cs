using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthControl : MonoBehaviour
{
    public GameObject RestartText;
    public GameObject TimerText;
    public GameObject explosion;
    void Start()
    {
        /*float X, Z;
        X = Random.Range(-3.56f, -5.87f);
        Z = Random.Range(-4.27f, -6.4f);
        transform.position = new Vector3(X, transform.position.y, Z);*/


    }
    private void OnTriggerEnter(Collider collide)
    {
        GameObject win = Instantiate(explosion) as GameObject;
        win.transform.position = transform.position;

        this.gameObject.SetActive(false);
        Restarts();
    }
    public void Restarts()
    {

        TimerText.gameObject.SetActive(false);
        RestartText.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("print");
            RestartText.gameObject.SetActive(false);
            //tank.transform.position = initialPos;
            // count.StartCoroutine(StartCountdown());


        }
    }
}


