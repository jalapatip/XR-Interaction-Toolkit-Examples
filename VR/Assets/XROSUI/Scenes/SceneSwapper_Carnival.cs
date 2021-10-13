using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
//For Mesh Cut
using BLINDED_AM_ME;
public class SceneSwapper_Carnival : MonoBehaviour
{
    public ParticleSystem confetti;
    public static bool reset;
    public static int timePassed = 1400;
    public bool sliced;
    public string loadLevel;
    public string currentlevel;
    public float timerCheck = 0.0f;
    public Renderer myRenderer;
    public ProjectileElement elementType;
    public AudioClip selectAudio;
    public GameObject successParticle;
    public Material[] mats;
    void Start()
    {
        if (sliced)
        {
            SceneManager.LoadScene(loadLevel);
            return;
        }
        else
        {
            

            elementType = (ProjectileElement)UnityEngine.Random.Range(0, mats.Length);
            myRenderer = this.GetComponent<Renderer>();
            
           // myRenderer.material = mats[0];
        }
    }
    void Update()
    {
    
    }
    public void OnTriggerEnter(Collider other)
    {
        
        reset = true;
        SceneManager.LoadScene(loadLevel);

    }
    public void OnSelectEnter(XRBaseInteractor obj)
    {
       
        reset = true;
        SceneManager.LoadScene(loadLevel);

    }
    
      public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject && other.gameObject.TryGetComponent(out VE_Weapon weapon))
        {
            if (!sliced)
            {
                reset = true;
                confetti.Stop();
                Hit(weapon.transform.position, weapon.transform.right);
               SceneManager.LoadScene(loadLevel);
            }
           
            
        }
    }

    private void Hit(Vector3 pos, Vector3 right)
    {
        Core.Ins.AudioManager.PlayAudio(selectAudio, ENUM_Audio_Type.Sfx);
       
        List<GameObject> cuts = MeshCut.Cut(gameObject, pos, right, myRenderer.material)
                .OrderByDescending(c => Volume(c.GetComponent<MeshFilter>().mesh))
                .ToList();
        sliced = true;
 
     
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
       
        MeshCollider collider = cut.GetComponent<MeshCollider>();
        if (collider == null)
        {
            collider = cut.AddComponent<MeshCollider>();
        }
        collider.convex = true;

    
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


}
