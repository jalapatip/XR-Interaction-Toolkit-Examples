using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private GameObject swordPrefab2;

    [SerializeField] private float spawnDelay = 1;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn()
    {
        //Create Vec3 of shoulder coordinate for both left and right should
        //Y position would be just shoulder coordinate y and then take the x value and subtract the x for left and add the x for the right
        //Then spawn swords in regards to the arm length from the shoulder, put near max reach, a bit under for ease of grabbing
        float armDistance = .4f;
        float startAngleLeft = (Mathf.PI * 11) / 6;
        float startAngleRight = (Mathf.PI / 6);
        float radius = 1f;
        //Left shoulder 
        for (int i = 0; i < 6; i++)
        {
            float angle = startAngleLeft * (Mathf.PI*1f / 6);
            startAngleLeft -= (Mathf.PI / 6);
            
            Vector3 newPos = transform.position + (new Vector3(Mathf.Cos(angle) * radius - armDistance, UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.Head).y * -.03f, Mathf.Sin(angle) * radius));//Adjust the y and x location for spawning on the shoulder joints and around the head
            Instantiate(swordPrefab, newPos, Quaternion.identity);
        }
        //Right shoulder 
        for (int i = 0; i < 6; i++)
        {
            float angle = startAngleRight * (Mathf.PI*1f / 6);
            startAngleRight += (Mathf.PI / 6);
            Vector3 newPos = transform.position + (new Vector3(Mathf.Cos(angle) * radius + armDistance, UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.Head).y * -.03f, Mathf.Sin(angle) * radius));//Adjust the y and x location for spawning on the shoulder joints and around the head
            Instantiate(swordPrefab2, newPos, Quaternion.identity);
        }
      
    }
}
