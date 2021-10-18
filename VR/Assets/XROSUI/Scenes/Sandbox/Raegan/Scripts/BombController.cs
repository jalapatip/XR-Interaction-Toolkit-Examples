using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject explosion;
  
 
    public GameObject tank;
    public Vector3 initialPos;
    public GameObject TimerText;
    CountdownController count;
    // Start is called before the first frame update
    void Start()
    {
        float X, Z;
        X = Random.Range(-3.56f, -5.87f);
        Z = Random.Range(-4.27f, -6.4f);
        transform.position = new Vector3(X,transform.position.y, Z);
        initialPos = tank.transform.position;
        explosion.SetActive(false);
        
    }
    public void OnTriggerEnter(Collider collide)
    {
        
        
        GameObject bomb = Instantiate(explosion) as GameObject;
        bomb.transform.position = transform.position;
        Destroy(collide.gameObject);
        this.gameObject.SetActive(false);
      

    }
   
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
