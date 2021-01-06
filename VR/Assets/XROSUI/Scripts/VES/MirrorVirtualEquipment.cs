using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 
/// </summary>
public class MirrorVirtualEquipment : MonoBehaviour
{
    public GameObject GameObjectToMirror;
    public GameObject Model;
    private Transform _transformToMirror;
    private Vector3 _startingPosition;
    private Vector3 _startingPositionToMirror;

    public bool isMirroringAtStart = true;
    public float distanceInFrontOfUser = 1f;
    private bool _isMirroring = false;

    private Transform _cameraTransform;
    //Vector3 offset = 0.5*GameObject

    public Vector3 mirroredAxis = new Vector3(1, 1, 1);

    private void OnEnable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Model)
        {
            Model = this.transform.GetChild(0).gameObject;
        }

        _cameraTransform = Camera.main.transform;
        SetGameObjectToMirror(GameObjectToMirror);
        if (isMirroringAtStart)
        {
            StartMirroring(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MirrorMovement();

        DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartMirroring(true);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartMirroring(false);
        }
    }


    private void MirrorMovement()
    {
        // if (!_isMirroring)
        // {
        //     return;
        // }


        //Track Position
        // var newPosition = (_transformToMirror.position - _startingPositionToMirror);
        // newPosition.x *= mirroredAxis.x;
        // newPosition.y *= mirroredAxis.y;
        // newPosition.z *= mirroredAxis.z;

        //this.transform.position = newPosition + _startingPosition;
        //    this.transform.position = newPosition + Camera.main.transform.position + Camera.main.transform.forward * 0.5f;

        // if (!_cameraTransform)
        // {
        //     _cameraTransform = Camera.main.transform;
        // }
        //
        var cameraPosition = _cameraTransform.position;
        //where we are, but ahead by 1 meter
        var worldPosition = cameraPosition + _cameraTransform.forward * distanceInFrontOfUser;
        var position = _transformToMirror.position;
        worldPosition += position - cameraPosition;

//        print(_transformToMirror.position.y);
        this.transform.position = new Vector3(worldPosition.x, position.y, worldPosition.z);


        //Track Rotation
        this.transform.LookAt(_transformToMirror);
    }

    public void SetGameObjectToMirror(GameObject go)
    {
        GameObjectToMirror = go;
        _transformToMirror = go.transform;
    }

    public void StartMirroring(bool bStartMirroring)
    {
        if (bStartMirroring)
        {
            if (_isMirroring)
            {
                return;
            }

            // _startingPosition = this.transform.position;
            // _startingPositionToMirror = GameObjectToMirror.transform.position;
            // _transformToMirror = GameObjectToMirror.transform;
            //

            _isMirroring = true;
            Model.SetActive(_isMirroring);
        }
        else
        {
            _isMirroring = false;
            Model.SetActive(_isMirroring);
            //this.gameObject.SetActive(_isMirroring);
        }
    }

    public void Reset()
    {
        //this.transform.position = _startingPosition;
    }
}