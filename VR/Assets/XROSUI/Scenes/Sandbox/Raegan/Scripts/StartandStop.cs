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



    public TMP_Text startDisplay, timerDisplay, winDisplay,loseDisplay;
    public GameObject Instructions,DisplayPanel;
 
    public int startTimer,countdownTimer;
    public static bool InitGame = false;
    public GameObject tank;//,wall;
   // public ParticleSystem explosion;
    public GameObject restart, mazeRender;
    public GameObject exit;
    private bool routineFlag;
    public GameObject[] walls;
    public GameObject winTrigger, loseTrigger;
    public int height=10;
    public int width=10;
    
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
        restart.gameObject.SetActive(false);
        timerDisplay.gameObject.SetActive(false);
        startDisplay.gameObject.SetActive(false);
        InitGame = false;
      //  explosion.gameObject.SetActive(false);
        exit.SetActive(false);
        DisplayPanel.SetActive(false);
        loseDisplay.gameObject.SetActive(false);
        winDisplay.gameObject.SetActive(false);
       // hasWon = false;
       // hasLost = false;
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
        StartGame();

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
       
        
    }

    protected virtual void OnHoverEnter(XRBaseInteractor obj)
    {
        StartGame();
    }

    public virtual void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
    }

    
    protected void Update()
    {
       if (Input.GetKey("space"))
        {
            StartGame();

        }

        VA_Update();
        //yes 
        /*if (GetComponent<healthControl>().winner) {
            hasWon = true;
        } 
     */
   
       
       
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

  
    public void StartGame()
    {
        tank.SetActive(true);

        walls = GameObject.FindGameObjectsWithTag("MazeWalls");
        //var maze = MazeGenerator.Generate(width, height);
        //mazeRender.GetComponent<MazeRenderer>().Draw(maze); 

        StartCoroutine(StartCountdown());


    }
    public void StopGame()
    {
        restart.SetActive(false);
        ExitMessage();
    }

    IEnumerator StartCountdown()
    {
        if (routineFlag) yield break;
        routineFlag = true;
        restart.SetActive(false);
        exit.SetActive(false);
        Instructions.SetActive(true);
        yield return new WaitForSeconds(3f);
        Instructions.SetActive(false);
        startDisplay.gameObject.SetActive(true);
        
        while (startTimer > 0)
        {
            startDisplay.text = startTimer.ToString();

            yield return new WaitForSeconds(1f);
            
            startTimer--;

        }
      
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<ScaleUp>().StartLerp();
        }
        yield return new WaitForSeconds(2f);
        startDisplay.text = "GO!";
        yield return new WaitForSeconds(1f);
        startDisplay.gameObject.SetActive(false);
        timerDisplay.gameObject.SetActive(true);
        

        while (countdownTimer > -1)
        {

            if (winTrigger.GetComponent<healthControl>().winner == true)
            {
                Win();
                yield return new WaitForSeconds(3f);
                
                winTrigger.GetComponent<healthControl>().winner = false;
                Restart();
                routineFlag = false;
                StopAllCoroutines();


            }
            else if(loseTrigger.GetComponent<BombController>().loser == true)
             {
                 Lose();
                 yield return new WaitForSeconds(3f);
                loseTrigger.GetComponent<BombController>().loser = false;
              
                 Restart();
                routineFlag = false;
                StopAllCoroutines();
            }
            timerDisplay.text = countdownTimer.ToString();
            yield return new WaitForSeconds(1f);
            countdownTimer--;
            

           
        }
        yield return new WaitForSeconds(1f);



        Lose();
        yield return new WaitForSeconds(3f);
        Restart();
        routineFlag = false;
        StopAllCoroutines();



    }
   public float scaleSpeed = 1f;

    public void Win()
    {
        timerDisplay.gameObject.SetActive(false);
        DisplayPanel.gameObject.SetActive(true);
        loseDisplay.gameObject.SetActive(false);
        winDisplay.gameObject.SetActive(true);



    }
    public void Lose()
        {
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<ScaleUp>().StopLerp();
        }
        timerDisplay.gameObject.SetActive(false);
            DisplayPanel.gameObject.SetActive(true);
            loseDisplay.gameObject.SetActive(true);
       
        winDisplay.gameObject.SetActive(false);
        
        }
       

 
    public void Restart()
    {
        DisplayPanel.gameObject.SetActive(false);
        loseDisplay.gameObject.SetActive(false);
        winDisplay.gameObject.SetActive(false);
        restart.gameObject.SetActive(true);
        countdownTimer = 10;
        startTimer = 5;
       
    }

    public void ExitMessage()
    {
        exit.SetActive(true);
    }
    void Explode(Vector3 pos)
    {
      //  Instantiate(explosion, pos, Quaternion.identity);
        Destruction(tank);

    }
    void Destruction(GameObject destroyed)
    {


        Destroy(destroyed);
    }

}

