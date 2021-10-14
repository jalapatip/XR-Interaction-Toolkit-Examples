using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//For Mesh Cut
using BLINDED_AM_ME;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class GameStart_PeripersonalSwords : MonoBehaviour
{
    public static bool started = false;
    public bool exists = true;
    public static GameObject newStartCube;
    public GameObject startCube;
    public GameObject score;
    public GameObject deathWall;
    public GameObject life;
    public GameObject projSpawner;
    public Renderer myRenderer;
    public ProjectileElement elementType;
    public AudioClip selectAudio;
    public GameObject successParticle;
    public Material[] mats;
    public float lifeTime = 12.0f;
    public static int ps_played;

    private bool sliced; //the projectile was already sliced
    // Start is called before the first frame update
    void Start()
    {
        ps_played = 0;
        newStartCube = Instantiate(this.gameObject);
        newStartCube.SetActive(false);
        started = false;
        
        score.SetActive(false);
        life.SetActive(false);
        projSpawner.SetActive(false);
        
        if (sliced)
        {
            return;
        }
        else
        {
            

            elementType = (ProjectileElement)UnityEngine.Random.Range(0, mats.Length);
            myRenderer = this.GetComponent<Renderer>();
            
            //myRenderer.material = mats[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
        public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject && other.gameObject.TryGetComponent(out VE_Weapon weapon))
        {
            //limiting to not allowing a hit after one slice, as the amount of splits limitless cause performance issues
           /* if ((this.elementType == ProjectileElement.GrayNormal || weapon.elementType == this.elementType) && !sliced)
            {
                //Dev.Log("Contact Count: " + other.contactCount);
                failureObject.SetActive(false);
                Hit(weapon.transform.position, weapon.transform.right);
            }
            else if (this.elementType != ProjectileElement.GrayNormal && weapon.elementType != this.elementType)
            {
                Core.Ins.AudioManager.PlayAudio(failureAudio, ENUM_Audio_Type.Sfx);
                //Below runs into error with private/public functions- need to fix 3D Audio function first
                //Core.Ins.AudioManager.Play3DAudio(selectAudio.ToString(), gameObject);

                StartCoroutine(activateFailureObject(1.0f));
            }*/
           //limiting to not allowing a hit after one slice, as the amount of splits limitless cause performance issues
           if (!sliced)
           {
               ps_played++;
               //Dev.Log("Contact Count: " + other.contactCount);
               projSpawner.SetActive(true);
               life.SetActive(true);
               score.SetActive(true);

               started = true;
               Hit(weapon.transform.position, weapon.transform.right);
           }
           
            
        }
    }

    private void Hit(Vector3 pos, Vector3 right)
    {
        Core.Ins.AudioManager.PlayAudio(selectAudio, ENUM_Audio_Type.Sfx);
        //Below runs into error with private/public functions- need to fix 3D Audio function first
        //Core.Ins.AudioManager.Play3DAudio(selectAudio.ToString(), gameObject);
        
        //We use MeshCut.Cut to get the resulting cutted gameobjects
        List<GameObject> cuts = MeshCut.Cut(gameObject, pos, right, myRenderer.material)
                .OrderByDescending(c => Volume(c.GetComponent<MeshFilter>().mesh))
                .ToList();
        sliced = true;
 
        //Create the particle effects on slice
        GameObject success = Instantiate(successParticle, transform.position, Quaternion.identity);
        success.GetComponent<Renderer>().material = myRenderer.material;
        foreach (Transform t in success.transform)
        {
            t.gameObject.GetComponent<Renderer>().material = myRenderer.material;
        }
        success.GetComponent<ParticleSystem>().Play();

        foreach (var cut in cuts)
        {
            MeshCollider collider = ManageCut(cut);
        }
    }

    private MeshCollider ManageCut(GameObject cut)
    {
        //Very minor issue: original (left side) has an uneven collider, shouldn't matter much in long run
        //E.g. a small cut will lead the smaller side to stand on its own when it shouldn't be able to

        //Needed for the left side not to fall through the floor
        MeshCollider collider = cut.GetComponent<MeshCollider>();
        if (collider == null)
        {
            collider = cut.AddComponent<MeshCollider>();
        }
        collider.convex = true;

        //Needed to destroy object after X seconds
        SlicedObject slicedObject = cut.GetComponent<SlicedObject>();
        if (slicedObject == null)
        {
            slicedObject = cut.AddComponent<SlicedObject>();
        }

        //Needed to figure out the dimensions of the cut object
        MeshFilter meshFilter = cut.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = cut.AddComponent<MeshFilter>();
        }

        //Needed to enable the actual cut
        Rigidbody rigidbody = cut.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = cut.AddComponent<Rigidbody>();
        }
        rigidbody.mass = Volume(meshFilter.mesh) * 1.0f;
        rigidbody.MovePosition(cut.transform.position + cut.transform.right * 0.01f);

        return collider;
    }

    private float Volume(Mesh mesh)
    {
        float volume = 0.0f;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i+=3)
        {
            Vector3 vec0 = vertices[triangles[i + 0]];
            Vector3 vec1 = vertices[triangles[i + 1]];
            Vector3 vec2 = vertices[triangles[i + 2]];
            float xyz012 = vec0.x * vec1.y * vec2.z;
            float xyz021 = vec0.x * vec2.y * vec1.z;
            float xyz102 = vec1.x * vec0.y * vec2.z;
            float xyz120 = vec1.x * vec2.y * vec0.z;
            float xyz201 = vec2.x * vec0.y * vec1.z;
            float xyz210 = vec2.x * vec1.y * vec0.z;
            float total = xyz012 - xyz021 - xyz102 + xyz120 + xyz201 - xyz210;
            volume += ((1.0f / 6.0f) * total);
        }
        volume = Math.Abs(volume);
        return volume;
    }
    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
