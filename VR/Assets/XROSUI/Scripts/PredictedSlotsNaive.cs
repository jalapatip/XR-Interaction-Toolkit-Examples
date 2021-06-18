using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictedSlotsNaive : MonoBehaviour
{
    public GameObject PF_Slots;

    public float armLength = 0.53f; //Mark has 53cm long arms not including hand
    public UI_ShowNaiveSlotLocation display;
    private List<GameObject> peripersonalSlots = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {   
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._0800, -120f);
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._0900, -90f);
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._1000, -60f);
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._1100, -30f);
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._1200, 0f);
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._0100, 30f);
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._0200, 60f);
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._0300, 90f);
        CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations._0400, 120f);
        
        
        
        //ENUM_XROS_PeripersonalEquipmentLocations._1200 = 
    }

    private void CreateSlot(ENUM_XROS_PeripersonalEquipmentLocations enumXrosPeripersonalEquipmentLocations,
        float degree)
    {
        //degree = degree + 90;
        var go = GameObject.Instantiate(PF_Slots);
        go.transform.SetParent(this.transform);
        //without the extra *1.3 factor for the x-coord. below, the slots that lie closer to the extremes of the x-axis would not be slightly too close to the user. So technically the slots are not located on a perfect circle. 

        go.transform.localPosition = new Vector3((Mathf.Sin(degree*Mathf.PI/180))*1.3f*armLength, 0.20f, (Mathf.Cos(degree*Mathf.PI/180))*armLength);

        go.name = enumXrosPeripersonalEquipmentLocations.ToString();
        ReportNaiveSocket rns = go.GetComponent<ReportNaiveSocket>();
        rns.location = enumXrosPeripersonalEquipmentLocations;
        rns.display = display;
 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
