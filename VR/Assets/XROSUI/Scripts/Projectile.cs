using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileElement
{
    GrayNormal,
    OrangePyro,
    BlueHydro,
    YellowGeo,
    GreenDendro,
    PurpleElectro,
    WhiteCryo,
    TealAnemo
    
}
public class Projectile : MonoBehaviour
{
    public ProjectileElement elementType;

    public Material[] mats;
    public Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        elementType = (ProjectileElement)UnityEngine.Random.Range(0, mats.Length);
        myRenderer = this.GetComponent<Renderer>();
        
        myRenderer.material = mats[(int)elementType];
        
        // switch (elementType)
        // {
        //     case ProjectileElement.GrayNormal:
        //         myRenderer.material = mats[(int)ProjectileElement.GrayNormal];
        //         break;
        //     case ProjectileElement.OrangePyro:
        //         
        //         break;
        //     case ProjectileElement.BlueHydro:
        //         
        //         break;
        //     case ProjectileElement.YellowGeo:
        //         
        //         break;
        //     case ProjectileElement.GreenDendro:
        //         
        //         break;
        //     case ProjectileElement.PurpleElectro:
        //         
        //         break;
        //     case ProjectileElement.WhiteCryo:
        //         
        //         break;
        //     case ProjectileElement.TealAnemo:
        //         
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 1.0f;
        var move = transform.forward;
        var speedAndTime = speed * Time.deltaTime;
        move = new Vector3(move.x * speedAndTime, move.y * speedAndTime, move.z * speedAndTime);
        this.transform.position = this.transform.position + move;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject && other.gameObject.TryGetComponent(out VE_Weapon weapon))
        {
//            Debug.Log(weapon.name);
            if (this.elementType == ProjectileElement.GrayNormal || weapon.elementType == this.elementType)
            {
                Hit();
            }
        }
    }

    private void Hit()
    {
        Debug.Log("Broken");
        Destroy(this.gameObject);
    }
}
