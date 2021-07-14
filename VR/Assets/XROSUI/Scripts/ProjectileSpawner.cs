using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject[] projectiles;

    public Transform[] points;

    public float beat = 60/105f;

    private float timer = 0f;
    private float cooldown = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > cooldown)
        {
//            print("New Projectile");
            var projectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], points[Random.Range(0, points.Length)]);
            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.Rotate(transform.forward, 0.0f);
            timer -= cooldown;
        }

        timer += Time.deltaTime;
    }
}
