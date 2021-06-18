using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_MirrorGO : MonoBehaviour
{
    public GameObject GOToMirror;

    public bool FollowPosition = true;
    public Vector3 FollowPositionXyz =  Vector3.one;
    public bool FollowRotation = false;
    public Vector3 FollowRotationXyz = Vector3.one;

    public bool CreateMirrorObject = true;
    public Vector3 positionOffSet;
    public Quaternion rotationOffset;

    public Renderer myRenderer;
    public MeshFilter myMeshFilter;
    // Start is called before the first frame update
    void Start()
    {
        if (CreateMirrorObject)
        {
            //myRenderer = this.GetComponent<Renderer>();
            myRenderer = GOToMirror.GetComponent<Renderer>();

            //myMeshFilter = this.GetComponent<MeshFilter>();
            myMeshFilter = GOToMirror.GetComponent<MeshFilter>();
        }
    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }
    
    private void Update()
    {
        //DebugUpdate();
    }
    private void DebugUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            DoFollowPosition();
        }
        if (Input.GetKey(KeyCode.E))
        {
            DoFollowRotation();
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (FollowPosition)
        {
            DoFollowPosition();
        }
        if (FollowRotation)
        {
            DoFollowRotation();
        }
    }

    void DoFollowPosition()
    {
        this.transform.position = GOToMirror.transform.position + positionOffSet;
    }

    void DoFollowRotation()
    {
        this.transform.rotation = GOToMirror.transform.rotation * rotationOffset;
    }
}
