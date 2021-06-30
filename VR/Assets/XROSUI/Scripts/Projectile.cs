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
    TealAnemo
    
}
public class Projectile : MonoBehaviour
{
    public ProjectileElement elementType;

    public Material[] mats;
    public Renderer myRenderer;

    public AudioClip selectAudio;

    public bool hit; //the projectile was already sliced

    // Start is called before the first frame update
    void Start()
    {
        if (hit)
        {
            return;
        }
        else
        {
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

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
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
            //Debug.Log(weapon.name);
            //limiting to not allowing a hit after one slice, as the amount of splits limitless cause performance issues
            if ((this.elementType == ProjectileElement.GrayNormal || weapon.elementType == this.elementType) && !hit)
            {
                Hit(weapon.transform.position);
            }
        }
    }

    private void Hit(Vector3 pos)
    {
        Core.Ins.AudioManager.PlayAudio(selectAudio, ENUM_Audio_Type.Sfx);

        Debug.Log("Broken");
        
        List<GameObject> cuts = MeshCut.Cut(gameObject, pos, transform.right, myRenderer.material)
                .OrderByDescending(c => Volume(c.GetComponent<MeshFilter>().mesh))
                .ToList();
        hit = true;

        //Cut 0
        //Destroy(cuts[0].GetComponent<BoxCollider>());

        BoxCollider collider = cuts[0].GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = cuts[0].AddComponent<BoxCollider>();
        }

        Projectile proj = cuts[0].GetComponent<Projectile>();
        if (proj == null)
        {
            proj = cuts[0].AddComponent<Projectile>();
        }
        proj.hit = true;
        proj.elementType = elementType;
        proj.myRenderer = cuts[0].GetComponent<Renderer>();

        MeshFilter meshFilter = cuts[0].GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = cuts[0].AddComponent<MeshFilter>();
        }

        Rigidbody rigidbody = cuts[0].GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = cuts[0].AddComponent<Rigidbody>();
        }
        rigidbody.mass = Volume(meshFilter.mesh) * 1.0f;
        rigidbody.MovePosition(cuts[0].transform.position + transform.right * 0.01f);

        //Cut 1
        //Destroy(cuts[1].GetComponent<BoxCollider>());
        
        BoxCollider collider1 = cuts[1].GetComponent<BoxCollider>();
        if (collider1 == null)
        {
            collider1 = cuts[1].AddComponent<BoxCollider>();
        }

        Projectile proj1 = cuts[1].GetComponent<Projectile>();
        if (proj1 == null)
        {
            proj1 = cuts[1].AddComponent<Projectile>();
        }
        proj1.hit = true;
        proj1.elementType = elementType;
        proj1.myRenderer = cuts[1].GetComponent<Renderer>();

        MeshFilter meshFilter1 = cuts[1].GetComponent<MeshFilter>();
        if (meshFilter1 == null)
        {
            meshFilter1 = cuts[1].AddComponent<MeshFilter>();
        }

        Rigidbody rigidbody1 = cuts[1].GetComponent<Rigidbody>();
        if (rigidbody1 == null)
        {
            rigidbody1 = cuts[1].AddComponent<Rigidbody>();
        }
        rigidbody1.mass = Volume(meshFilter1.mesh) * 1.0f;
        rigidbody1.MovePosition(cuts[1].transform.position + transform.right * 0.01f);

        //Destroy(this.gameObject);
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
