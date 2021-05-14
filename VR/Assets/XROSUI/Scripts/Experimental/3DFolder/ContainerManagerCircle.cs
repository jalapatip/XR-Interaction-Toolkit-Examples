using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class ContainerManagerCircle : MonoBehaviour
{
    [Tooltip("Assign from Project")]
    public GameObject PF_Socket;

    [Tooltip("Assign from Project")]
    public GameObject PF_containerObject;

    [Tooltip("Assign from Project")]
    public GameObject PF_layerCircleObject;

    [Tooltip("Assign from Prefab")]
    public GameObject StartPoint;

    [Tooltip("Customize in Inspector")]
    public float socketOffsetDistance = 2;

    [Tooltip("Customize in Inspector")]
    public float layerOffsetDistance = 0.15f;

    private List<ContainerLayerCircle> _containerLayerCircleList = new List<ContainerLayerCircle>();
    private List<ContainerObject> _containerObjectList = new List<ContainerObject>();
    private List<ContainerSocket> _ContainerSocketList = new List<ContainerSocket>();
    private GameObject _rightDirectController;
    private GameObject _leftDirectController;

    private float _leftHandValue;
    private float _rightHandValue;
    private float _depthValue = 0.1f;


    //Debug only
    private GameObject _co;
    private GameObject _cs;

    public Transform planeTarget;
    public Transform target;

    private void Start()
    {
        _leftDirectController = Core.Ins.XRManager.GetRightDirectControllerGO();
        _rightDirectController = Core.Ins.XRManager.GetLeftDirectController();

        GenerateLayersAndObjects();
    }

    private void GenerateLayersAndObjects()
    {
        //Generate 3 layers.
        for (var i = 0; i < 3; i++)
        {
            var go = Instantiate(PF_layerCircleObject, this.transform.position + Vector3.back * layerOffsetDistance * i,
                Quaternion.identity);
            go.name = "Layer " + i;

            _depthValue += 0.2f;
            //print("The " + i + " layer value is " + value);
            go.transform.SetParent(this.transform);
            var co = go.GetComponent<ContainerLayerCircle>();
            co.layervalue = _depthValue;
            _containerLayerCircleList.Add(co);
        }

        AddAllContainerSocket();

        //Generate containers of different color for testing purposes
        for (var i = 0; i < 5; i++)
        {
            AddContainerObject();
        }

        for (var i = 0; i < 4; i++)
        {
            AddContainerObject();
            InitializeColorHelper(Color.red);
        }

        for (var i = 0; i < 6; i++)
        {
            AddContainerObject();
            InitializeColorHelper(Color.blue);
        }

        for (var i = 0; i < 6; i++)
        {
            AddContainerObject();
            InitializeColorHelper(Color.green);
        }

        for (var i = 0; i < 6; i++)
        {
            AddContainerObject();
            InitializeColorHelper(Color.yellow);
        }
    }

    private void InitializeColorHelper(Color c)
    {
        c.a = 0.3f;
        _co.GetComponent<MeshRenderer>().material.SetColor("_Color", c);
    }

    // Update is called once per frame
    private void Update()
    {
        CheckDistanceForEach();
    }


    public void AddContainerObject()
    {
        for (var i = 0; i < _containerLayerCircleList.Count; i++)
        {
            if (!_containerLayerCircleList[i].IsFull())
            {
                _co = Instantiate(PF_containerObject, transform.position + transform.forward * 2, Quaternion.identity);
                _containerLayerCircleList[i].AddObject(_co);
                return;
            }
        }
    }

    private void AddAllContainerSocket()
    {
        foreach (var layer in _containerLayerCircleList)
        {
            for (var j = 0; j < layer.GetMaxSocket(); j++)
            {
                //generate Container Socket
                _cs = Instantiate(PF_Socket, transform.position + transform.forward * socketOffsetDistance,
                    Quaternion.identity);
                layer.AddObjectSocket(_cs);
            }
        }
    }

    #region CheckDistanceForEach

    //slidervalue from 0 to 0.8
    //layervalue 0.2 0.4 0.6
    private void CheckDistanceForEach()
    {
        _leftHandValue = Vector3.Distance(_rightDirectController.transform.position, StartPoint.transform.position) -
                         0.1f;
        _rightHandValue = Vector3.Distance(_leftDirectController.transform.position, StartPoint.transform.position) -
                          0.1f;

        foreach (var layer in _containerLayerCircleList)
        {
            if (_leftHandValue <= layer.layervalue || _rightHandValue <= layer.layervalue)
            {
                layer.HideContainerObject();
                layer.gameObject.SetActive(false);
            }
            else if (_leftHandValue > layer.layervalue || _rightHandValue > layer.layervalue)
            {
                layer.ShowContainerObject();
                layer.gameObject.SetActive(true);
            }
        }
    }

    //public float GetDistanceToPoint()
    //{
    //    Plane plane= new Plane(planeTarget.up, planeTarget.position);
    //    float distance = plane.GetDistanceToPoint(target.position);
    //    print("distance:" + distance);
    //    //return distance;
    //}

    #endregion

    #region check distance between the controller and layer, if<1 disappear

    //public void CheckDistance()
    //{
    //    GameObject controllerhand = GameObject.Find("righthand");
    //    float ContainerDepth;
    //    Container_Cube = GameObject.Find("Container_Cube");
    //    ContainerDepth = Container_Cube.GetComponent<Collider>().bounds.size.x;
    //    print("ContainerDepth is" + ContainerDepth);
    //    float dis2;
    //    //dis2 = Vector3.Distance(controllerhand.transform.position, );

    //    //for (float i=0;i<ContainerDepth;i++) 
    //    //{ 
    //    //}
    //    GameObject disappearlayer = GameObject.Find("Container_Cube");
    //    float dis;
    //    dis = Vector3.Distance(controllerhand.transform.position, disappearlayer.transform.position);
    //    // print("dis=" + dis);
    //    if (dis <= 0.2f)
    //    {
    //        containerlayerlist[0].gameObject.SetActive(false);
    //    }
    //    else if (dis > 1.0)
    //    {
    //        containerlayerlist[0].gameObject.SetActive(true);
    //    }
    //}

    #endregion

    #region check value

    public void CheckValue()
    {
        GameObject controllerhand = GameObject.Find("righthand");
        GameObject endpoint = GameObject.Find("EndPoint");
        float dis;
        dis = Vector3.Distance(controllerhand.transform.position, endpoint.transform.position);
        print("dis is" + dis);
        if (dis >= 0.4f && dis <= 0.45f)
        {
            _containerLayerCircleList[0].gameObject.SetActive(false);
        }
        else if (dis > 0.5)
        {
            _containerLayerCircleList[0].gameObject.SetActive(true);
        }

        if (dis >= 0.2f && dis <= 0.25f)
        {
            _containerLayerCircleList[1].gameObject.SetActive(false);
        }
        else if (dis > 0.3)
        {
            _containerLayerCircleList[1].gameObject.SetActive(true);
        }

        if (dis >= 0 && dis <= 0.12)
        {
            _containerLayerCircleList[2].gameObject.SetActive(false);
        }
        else if (dis > 0.1)
        {
            _containerLayerCircleList[2].gameObject.SetActive(true);
        }
    }

    #endregion

    #region check angel between the cube and controller

    //public void CheckAngle() 
    //{ 
    //    GameObject controllerhand = GameObject.Find("righthand");
    //    //print("Containerforward" + this.transform.forward);
    //    print("test: " +Vector3.Dot(this.transform.forward, controllerhand.transform.forward));
    //}

    #endregion
}