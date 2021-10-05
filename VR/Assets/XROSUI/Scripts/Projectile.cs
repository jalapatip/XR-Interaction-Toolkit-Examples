using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//For Mesh Cut
using BLINDED_AM_ME;
using TMPro;
public enum ProjectileElement
{
    GrayNormal,
    OrangePyro,
    BlueHydro,
    YellowGeo,
    GreenDendro,
    PurpleElectro,
    WhiteCryo,
    TealAnemo,
}

public class Projectile : MonoBehaviour
{
    public ProjectileElement elementType;
 
    public Material[] mats;
    public Renderer myRenderer;

    public AudioClip selectAudio;
    public AudioClip failureAudio;

    public GameObject successParticle;
    public GameObject failureObject;

    
    public static int score = 0;
    public float lifeTime = 10.0f;

   
    //Needed to disable movement of half of the cube and only allow a max of 1 slice 
    private bool sliced; //the projectile was already sliced

    // Start is called before the first frame update
    void Start()
    {
      
        if (sliced)
        {
            return;
        }
        else
        {
           
            Destroy(gameObject,lifeTime);
            elementType = (ProjectileElement)UnityEngine.Random.Range(0, mats.Length);
            myRenderer = this.GetComponent<Renderer>();

            myRenderer.material = mats[(int)elementType];
        }
        
    }

    public bool canMove = true;
    // Update is called once per frame
    void Update()
    {
        if (!sliced)
        {
            if (!canMove)
                return;
            float speed = 1.0f;
            var move = transform.forward;
            var speedAndTime = speed * Time.deltaTime;
            move = new Vector3(move.x * speedAndTime, move.y * speedAndTime, move.z * speedAndTime);
            this.transform.position = this.transform.position + move;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject && other.gameObject.TryGetComponent(out VE_Weapon weapon))
        {
           
           if (!sliced)
           {
               //Dev.Log("Contact Count: " + other.contactCount);
               failureObject.SetActive(false);
               Hit(weapon.transform.position, weapon.transform.right);
               score += 1;
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

    private IEnumerator activateFailureObject(float seconds)
    {
        failureObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        failureObject.SetActive(false);
    }

    public void destroySelf()
    {
        Destroy(this.gameObject);
    }
}
