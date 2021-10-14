using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(XRGrabInteractable))]
public class StartandStop : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;
   

    float lerpDuration = 3;
    float startValue = 0;
    float endValue = 2;
    float valueToLerp = 0;
  
    public TMP_Text startDisplay, timerDisplay;
    
    public GameObject Instructions;
    private int index = 0;
    public int startTimer,countdownTimer;
    public static bool InitGame = false;
    public GameObject tank;
    public ParticleSystem explosion;
    //public GameObject  
    //Powen: It seems XRITK did not intend IsActivated to be a variable. We can add one ourselves but it could cause more confusion
    //It may need to be handled case by case
    //private bool _isActivated = false;
    public ENUM_XROS_AvatarTypes avatarTypes = ENUM_XROS_AvatarTypes.Eyes;
    
    
    void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
       
        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
        _grabInteractable.onSelectExit.AddListener(OnSelectExit);
        _grabInteractable.onActivate.AddListener(OnActivate);
        _grabInteractable.onDeactivate.AddListener(OnDeactivate);

        // assignedTeleportationAnchor.onSelectExit.AddListener(TeleportToAvatar);
        _colliderList = this.GetComponentsInChildren<Collider>();
    }

    public void Start()
    {
        Instructions.SetActive(false);

        timerDisplay.gameObject.SetActive(false);
        startDisplay.gameObject.SetActive(false);
        InitGame = false;
    }



    private Collider[] _colliderList;

    private void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnLastHoverExit);
        _grabInteractable.onSelectEnter.RemoveListener(OnSelectEnter);
        _grabInteractable.onSelectExit.RemoveListener(OnSelectExit);
        _grabInteractable.onActivate.RemoveListener(OnActivate);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivate);

        // assignedTeleportationAnchor.onSelectExit.AddListener(TeleportToAvatar);
    }

    /*public void EnableColliders(bool b)
    {
        foreach (var c in _colliderList)
        {
            c.enabled = b;
        }
    }
    */


    //This only triggers while the object is grabbed (grip button) and the trigger button is initially pushed
    protected virtual void OnActivate(XRBaseInteractor obj)
    {


    }


    //This only triggers while the object is grabbed (grip button) and the trigger button is initially released
    protected virtual void OnDeactivate(XRBaseInteractor obj)
    {


    }

    protected virtual void OnSelectEnter(XRBaseInteractor obj)
    {

    }

    protected virtual void OnSelectExit(XRBaseInteractor obj)
    {
    }

    protected virtual void OnLastHoverExit(XRBaseInteractor obj)
    {
        
    }


    protected virtual void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        StartGame(true);
        
    }

    protected virtual void OnHoverEnter(XRBaseInteractor obj)
    {
       
    }

    public virtual void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
    }

    protected void Update()
    {
        
     
        VA_Update();
    
       
    }


    public float distanceToHideAvatar = 2f;

    private void VA_Update()
    {
        //if (_IsHidden)
        {
            // var distance = Vector3.Distance(Core.Ins.XRManager.GetXrCamera().gameObject.transform.position,
            //     this.gameObject.transform.position);

            var position1 = new Vector2(Core.Ins.XRManager.GetXrCamera().transform.position.x,
                Core.Ins.XRManager.GetXrCamera().transform.position.z);
            var position2 = new Vector2(this.transform.position.x, this.transform.position.z);
            var distance = Vector2.Distance(position1, position2);
            //            print(distance);
            /*  if (distance > 1)
              {
                  ShowAvatar(true);
              }
              else
              {
                  ShowAvatar(false);
              }*/
        }
    }

    public void StartGame(bool b)
    {
        
        StartCoroutine(StartCountdown());
        

    }
    
    IEnumerator StartCountdown()
    {
        Instructions.SetActive(true);
        yield return new WaitForSeconds(3f);
        Instructions.SetActive(false);
        startDisplay.gameObject.SetActive(true);
        InitGame = true;
        while (startTimer > 0)
        {
            startDisplay.text = startTimer.ToString();

            yield return new WaitForSeconds(1f);

            startTimer--;

        }

        startDisplay.text = "GO!";
        yield return new WaitForSeconds(1f);
        startDisplay.gameObject.SetActive(false);
        timerDisplay.gameObject.SetActive(true);
        

        while (countdownTimer > 0)
        {

            timerDisplay.text = countdownTimer.ToString();
            yield return new WaitForSeconds(1f);
            countdownTimer--;


        }

        Explode(tank.transform.position);
        yield return new WaitForSeconds(1f);
        timerDisplay.gameObject.SetActive(false);
        StopCoroutine(StartCountdown());
       

    }
   public float scaleSpeed = 1f;



    void Explode(Vector3 pos)
    {
        Instantiate(explosion, pos, Quaternion.identity);
        Destruction(tank);

    }
    void Destruction(GameObject destroyed)
    {


        Destroy(destroyed);
    }

}

