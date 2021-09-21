using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;




public class ProjSpawnerGame : MonoBehaviour
{
    [SerializeField] private GameObject projSpawner;

    [SerializeField] private GameObject middle;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    private float timerCheck = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        projSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timerCheck += Time.deltaTime;
        if (timerCheck >= 5)
        {
            StartCoroutine(swapSpawners());
            timerCheck = 0f;
        }
        
    }

    private void projSpawn()
    {
        float headToShoulder = 0.34f;
        Quaternion rotation = Quaternion.Euler(180, 0, 0);
        var headPosition = Core.Ins.XRManager.GetHeadPosition();
        Vector3 spawnPos = new Vector3(headPosition.x, headPosition.y *- headToShoulder, headPosition.z+7f);
        Vector3 spawnPosLeft = new Vector3(headPosition.x - 3f, headPosition.y + headPosition.y *- headToShoulder, headPosition.z+6f);
        Vector3 spawnPosRight = new Vector3(headPosition.x + 3f, headPosition.y + headPosition.y *- headToShoulder, headPosition.z+6f);

        middle = Instantiate(projSpawner, spawnPos, rotation );
        left = Instantiate(projSpawner, spawnPosLeft, rotation* Quaternion.Euler(0,30,0));
        right = Instantiate(projSpawner, spawnPosRight, rotation* Quaternion.Euler(0,-30,0));
        
        middle.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
    }

    private IEnumerator swapSpawners()
    {
        int choice = UnityEngine.Random.Range(1,4);
        switch (choice)
        {
            case 1:
                middle.SetActive(true);
                yield return new WaitForSecondsRealtime(10);
                middle.SetActive(false);
                break;
            case 2:
                left.SetActive(true);
                yield return new WaitForSecondsRealtime(10);
                left.SetActive(false);
                break;
            case 3:
                right.SetActive(true);
                yield return new WaitForSecondsRealtime(10);
                right.SetActive(false);
                break;
        }
    }
}
