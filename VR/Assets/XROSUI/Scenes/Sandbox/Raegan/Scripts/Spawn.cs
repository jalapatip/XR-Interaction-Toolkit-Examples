using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject spawnedObject;
    public Transform[] positions;
    private float time = 0;
    private float seconds = 10f;

    private GameObject eyes;
    // Start is called before the first frame update
    void Start()
    {
      
    }


    // Update is called once per frame
    void Update()
    {
        
        if (time > seconds)
        {
            eyes = Instantiate(spawnedObject, positions[Random.Range(0, positions.Length)]);
            eyes.transform.localPosition = Vector3.zero;
            eyes.transform.Rotate(transform.forward, 0.0f);
            time -= seconds;
        }
        if(time > seconds * 0.5)
        {
            Destruction(eyes);
        }
        time += Time.deltaTime;
    }

    void Destruction(GameObject destroyed)
    {
        Destroy(destroyed);
    }
}
   
   

