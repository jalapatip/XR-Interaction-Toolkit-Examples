using System;
using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework.Constraints;
using UnityEngine;

//public delegate void Delegate_NewAvatar();

/// <summary>
/// Virtual Equipment System
/// 
/// </summary>
public class Manager_VES : MonoBehaviour
{
    public VES_GestureFeedback VES_GestureFeedback;
    public GameObject PF_DefaultMirrorObject;
    public GameObject PF_MirrorObjectHolder;
    private GameObject MirrorObjectsHolder;
    //public static event Delegate_NewAvatar EVENT_NewAvatar;


    public float displayTime = 0.5f;

    private void OnEnable()
    {
        if (!MirrorObjectsHolder)
        {
            SetupMirrorObjectHolder();    
        }
    }
    
    private float _timeRemaining;
    // Start is called before the first frame update
    private void Start()
    {
        //MirrorObjectsHolder  = GameObject.Instantiate(PF_MirrorObjectHolder);
        //Core.Ins.XRManager.PlaceInXrRigSpawnedObjects(MirrorObjectsHolder);
        //replaces the 2 lines above
//        SetupMirrorObjectHolder();
    }


    // Update is called once per frame
    private void Update()
    {
        if (_timeRemaining < 0)
        {
            _timeRemaining = 0;
            VES_GestureFeedback.gameObject.SetActive(false);
        }
        else
        {
            _timeRemaining -= Time.deltaTime;
        }
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        
    }

    public void UpdateGestureFeedback(ENUM_XROS_EquipmentGesture equipmentGesture, VE_EquipmentBase veb)
    {
        if (!VES_GestureFeedback.gameObject.activeSelf)
        {
            VES_GestureFeedback.gameObject.SetActive(true);
        }
        
        _timeRemaining = displayTime;
        VES_GestureFeedback.UpdateGestureFeedback(equipmentGesture, veb);
    }

    
    public void PlaceMirrorObject(GameObject goMirrorObject)
    {
        if (MirrorObjectsHolder)
        {
            goMirrorObject.transform.SetParent(MirrorObjectsHolder.transform);    
        }
        else
        {
            Dev.Log("MirrorObjectsHolder does not exist, probably due to scene change..");
            SetupMirrorObjectHolder();
        }
    }

    private void SetupMirrorObjectHolder()
    {
        MirrorObjectsHolder  = GameObject.Instantiate(PF_MirrorObjectHolder);
        Core.Ins.XRManager.PlaceInXrRigSpawnedObjects(MirrorObjectsHolder);
    }
}