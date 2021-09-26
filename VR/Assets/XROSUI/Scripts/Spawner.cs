using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;

public enum SwordOClock
{_0100, _0200, _0300, _0400
}

public class EquipmentSlot
{
    public SwordOClock OClock;
     
    public string StoredPrefab; //or a name
};


public class Spawner : MonoBehaviour
{
    public List<GameObject> swordList;
    
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private GameObject swordPrefab2;

    //[SerializeField] private float spawnDelay = 1;

    private List<EquipmentSlot> myLIst = new List<EquipmentSlot>();
    
    // Start is called before the first frame update
    void Start()
    {
        Spawn();

        SpawnDefaultSwords();
    }

    private void SpawnDefaultSwords()
    {
        GameObject instance = Instantiate(Resources.Load("Prop_CrystalSword_02", typeof(GameObject))) as GameObject;
        instance.transform.position = new Vector3(100, 0, 100);
        
        //myLIst.Add(new Vector3(), );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn()
    {
        float startAngleLeft = (Mathf.PI * 11) / 6;//11 o clock
        float startAngleRight = (Mathf.PI / 6);// 1 o clock

        // foreach (var v in myLIst)
        // {
        //     SpawnSwordsBasedOnVariable(v.OClock, v.StoredPrefab);
        // }
        //
        print("Start Angle Left " + startAngleLeft);
        print("Start Angle Right " + startAngleRight);
        //Left shoulder
        SpawnSwordsBasedOnVariable(6, startAngleLeft, swordPrefab, true);
        //Right shoulder
        SpawnSwordsBasedOnVariable(6, startAngleRight, swordPrefab2, false);
        // //Left shoulder 
        // for (int i = 0; i < 6; i++)
        // {
        //     float angle = startAngleLeft * (Mathf.PI*1f / 6);
        //     startAngleLeft -= (Mathf.PI / 6);
        //     
        //     //TODO UnityEngine.XR.InputTracking.GetLocatPosition to be replaced with call to Core.Ins.XrController or something. 
        //     
        //     
        //     Vector3 newPos = headPosition + (new Vector3(Mathf.Cos(angle) * radius - armDistance, -.03f, Mathf.Sin(angle) * radius));//Adjust the y and x location for spawning on the shoulder joints and around the head
        //                                                                                                                                                                                                   //Vector3 newPos = transform.position + (new Vector3(Mathf.Cos(angle) * radius - armDistance, UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.Head).y * -.03f, Mathf.Sin(angle) * radius));//Adjust the y and x location for spawning on the shoulder joints and around the head
        //     Instantiate(swordPrefab, newPos, Quaternion.identity);
        // }
        // //Right shoulder 
        // for (int i = 0; i < 6; i++)
        // {
        //     float angle = startAngleRight * (Mathf.PI*1f / 6);
        //     startAngleRight += (Mathf.PI / 6);
        //     Vector3 newPos = headPosition + (new Vector3(Mathf.Cos(angle) * radius + armDistance, -.03f, Mathf.Sin(angle) * radius));//Adjust the y and x location for spawning on the shoulder joints and around the head
        //     Instantiate(swordPrefab2, newPos, Quaternion.identity);
        // }
      
    }

    //
    private float degreeBetweenSwords = (float)(Math.PI / 6);
    private void SpawnSwordsBasedOnVariable(int no, float startAngle, GameObject weaponType, bool Isleft)
    {
        //Create Vec3 of shoulder coordinate for both left and right should
        //Y position would be just shoulder coordinate y and then take the x value and subtract the x for left and add the x for the right
        //Then spawn swords in regards to the arm length from the shoulder, put near max reach, a bit under for ease of grabbing
        
        //TODO Core.Ins.HumanScaleManager ???
        float armDistance = .7f;

       // float radius = 1f;

        var headPosition = Core.Ins.XRManager.GetHeadPosition();
        float headToShoulderDistance = 0.34f; //get from humanscale
        float shoulderWidth = .4f; //also get from some kind of humanscalemanager
        float offsetBasedOnLeftRight = shoulderWidth / 2;
        if (Isleft)//Set offset before the loop
        {
            offsetBasedOnLeftRight *= -1;
        }
        else
        {
        }
        
        for (int i = 0; i < no; i++)//loop through the angles
        {
            float angle = startAngle * (Mathf.PI*1f / no);
            
            if (Isleft)
            {
                startAngle -= degreeBetweenSwords;//Decrease by 30 degrees. I.E from 11 o clock to 10 clock
            }
            else
            {
                startAngle += degreeBetweenSwords;//increase by 30 degrees. I.E. from 1 o clock to 2 o clock
            }  

            Vector3 newPos = headPosition + new Vector3(offsetBasedOnLeftRight, 0, 0) + (new Vector3(Mathf.Cos(angle)*armDistance,(headPosition.y * -headToShoulderDistance), Mathf.Sin(angle) * armDistance));//Adjust the y and x location for spawning on the shoulder joints and around the head
            //Vector3 newPos = headPosition + new Vector3(offsetBasedOnLeftRight, 0, 0);
            //Vector3 newPos = transform.position + (new Vector3(Mathf.Cos(angle) * radius - armDistance, UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.Head).y * -.03f, Mathf.Sin(angle) * radius));//Adjust the y and x location for spawning on the shoulder joints and around the head
            Instantiate(weaponType, newPos, Quaternion.identity);
        }
    }
}
