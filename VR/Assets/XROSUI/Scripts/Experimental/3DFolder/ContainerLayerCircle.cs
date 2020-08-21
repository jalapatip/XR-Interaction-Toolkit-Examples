using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerLayerCircle : MonoBehaviour
{
    int maxSocket;

    // int maxObject;
    private float containerObjectRadius = 0.05f;
    private int Rmax;
    private int Cmax;
    private float Rlast = 0;
    private float Clast = 0;
    private float RlastforSocket = 0;
    private float ClastforSocket = 0;
    private float buffery = 0.2f;
    private float bufferz = 0.2f;
    public float layervalue = 0f;
    private List<ContainerObject> _containerObjectList = new List<ContainerObject>();
    private List<ContainerSocket> _containerSocketList = new List<ContainerSocket>();
    float r = 0.25f;

    public void AddObject(GameObject go)
    {
        go.name = "CO " + _containerObjectList.Count;
        var co = go.GetComponent<ContainerObject>();
        _containerObjectList.Add(co);

        //int currentIndex = containerobjectlist.Count;
        //containersocketlist[currentIndex].
        TraditionalAddObject(co);
    }

    private void TraditionalAddObject(ContainerObject co)
    {
        // co.transform.position = this.transform.position + Vector3.up * 0.5f * containerobjectlist.Count;
        ////Place object
        float Ri;
        float Ci;
        ////print("set position:");
        if (_containerObjectList.Count == 1)
        {
            Ci = 0;
            Ri = 0;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }

        //2
        if (_containerObjectList.Count == 2)
        {
            Ci = 0;
            Ri = r;
            Clast = Ci;
            Rlast = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }

        //3
        if (_containerObjectList.Count == 3)
        {
            Ci = (float) (-r / 1.414);
            Ri = (float) (r / 1.414);
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }

        if (_containerObjectList.Count == 4)
        {
            Ci = -r;
            Ri = 0;
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }

        if (_containerObjectList.Count == 5)
        {
            Ci = (float) (-r / 1.414);
            Ri = (float) (-r / 1.414);
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }

        if (_containerObjectList.Count == 6)
        {
            Ci = 0;
            Ri = -r;
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }

        if (_containerObjectList.Count == 7)
        {
            Ci = (float) (r / 1.414);
            Ri = (float) (-r / 1.414);
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }

        if (_containerObjectList.Count == 8)
        {
            Ci = r;
            Ri = 0;
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }

        if (_containerObjectList.Count == 9)
        {
            Ci = (float) (r / 1.414);
            Ri = (float) (r / 1.414);
            
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            co.transform.position = new Vector3(x, y, z);
            co.transform.SetParent(this.transform);
        }
    }

    public void AddObjectSocket(GameObject go)
    {
        ContainerSocket cs = go.GetComponent<ContainerSocket>();
        cs.name = "CS " + _containerSocketList.Count;
        _containerSocketList.Add(cs);
        //co.transform.position = this.transform.position + Vector3.up * 0.5f * containerobjectlist.Count;
        //Place object
        //1
        float Ri;
        float Ci;
        ////print("set position:");
        //Ri = RlastforSocket;
        //print("Ri: " + Ri);
        //Ci = ClastforSocket;
        // print("Ci: " + Ci); 
        //1
        if (_containerSocketList.Count == 1)
        {
            Ci = 0;
            Ri = 0;
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }

        //2
        if (_containerSocketList.Count == 2)
        {
            Ci = 0;
            Ri = r;
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }

        //3
        if (_containerSocketList.Count == 3)
        {
            Ci = (float) (-r / 1.414);
            Ri = (float) (r / 1.414);
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }

        if (_containerSocketList.Count == 4)
        {
            Ci = -r;
            Ri = 0;
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }

        if (_containerSocketList.Count == 5)
        {
            Ci = (float) (-r / 1.414);
            Ri = (float) (-r / 1.414);
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }

        if (_containerSocketList.Count == 6)
        {
            Ci = 0;
            Ri = -r;
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }

        if (_containerSocketList.Count == 7)
        {
            Ci = (float) (r / 1.414);
            Ri = (float) (-r / 1.414);
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }

        if (_containerSocketList.Count == 8)
        {
            Ci = r;
            Ri = 0;
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }

        if (_containerSocketList.Count == 9)
        {
            Ci = (float) (r / 1.414);
            Ri = (float) (r / 1.414);
            ClastforSocket = Ci;
            RlastforSocket = Ri;
            float x = this.transform.position.x + (Ci);
            float y = this.transform.position.y + (Ri);
            float z = this.transform.position.z;
            cs.transform.position = new Vector3(x, y, z);
            cs.transform.SetParent(this.transform);
        }
    }

    public int GetMaxSocket()
    {
        var localScale = transform.localScale;
        var a = localScale.x;
        var b = localScale.y;
        var c = localScale.z;
        Cmax = (int) (c / (buffery + (containerObjectRadius * 2)));
        // print("Cmax is "+ Cmax);
        Rmax = (int) (b / (bufferz + (containerObjectRadius * 2)));
        maxSocket = Cmax * Rmax;
        return maxSocket;
    }

    public bool IsFull()
    {
        maxSocket = Cmax * Rmax;
        return _containerObjectList.Count >= maxSocket;
    }

    // Start is called before the first frame update
    void Awake()
    {
        GetMaxSocket();
    }

    public void HideContainerObject()
    {
        foreach (var containerObject in _containerObjectList)
        {
            containerObject.gameObject.SetActive(false);
        }
    }

    public void ShowContainerObject()
    {
        foreach (var containerObject in _containerObjectList)
        {
            containerObject.gameObject.SetActive(true);
        }
    }
}