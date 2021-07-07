using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//For Mesh Cut
using BLINDED_AM_ME;

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
    public GameObject failureParticle;

    public float lifeTime = 12.0f;

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
            Destroy(gameObject, lifeTime);

            elementType = (ProjectileElement)UnityEngine.Random.Range(0, mats.Length);
            myRenderer = this.GetComponent<Renderer>();

            myRenderer.material = mats[(int)elementType];
        }
        
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
            //limiting to not allowing a hit after one slice, as the amount of splits limitless cause performance issues
            if ((this.elementType == ProjectileElement.GrayNormal || weapon.elementType == this.elementType) && !sliced)
            {
                //Dev.Log("Contact Count: " + other.contactCount);
                Hit(weapon.transform.position, weapon.transform.right);
            }
            else if (this.elementType != ProjectileElement.GrayNormal && weapon.elementType != this.elementType)
            {
                Core.Ins.AudioManager.PlayAudio(failureAudio, ENUM_Audio_Type.Sfx);
                //Below runs into error with private/public functions- need to fix 3D Audio function first
                //Core.Ins.AudioManager.Play3DAudio(selectAudio.ToString(), gameObject);

                print("hi");

                GameObject failure = Instantiate(failureParticle, transform.position, Quaternion.identity);
                failure.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private void Hit(Vector3 pos, Vector3 right)
    {
        Core.Ins.AudioManager.PlayAudio(selectAudio, ENUM_Audio_Type.Sfx);
        //Below runs into error with private/public functions- need to fix 3D Audio function first
        //Core.Ins.AudioManager.Play3DAudio(selectAudio.ToString(), gameObject);

        GameObject success = Instantiate(successParticle, transform.position, Quaternion.identity);
        success.GetComponent<ParticleSystem>().Play();

        //We use MeshCut.Cut to get the resulting cutted gameobjects
        List<GameObject> cuts = MeshCut.Cut(gameObject, pos, right, myRenderer.material)
                .OrderByDescending(c => Volume(c.GetComponent<MeshFilter>().mesh))
                .ToList();
        sliced = true;
        
        foreach (var cut in cuts)
        {
            ManageCut(cut);
        }
    }

    private void ManageCut(GameObject cut)
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
