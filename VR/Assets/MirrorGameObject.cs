using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MirrorGameObject : MonoBehaviour
{
    public GameObject GameObjectToMirror;
    private Transform _transformToMirror;
    private Vector3 _startingPosition;
    private Vector3 _startingPositionToMirror;

    private bool _isMirroring = false;
    //Vector3 offset = 0.5*GameObject

    public Vector3 mirroredAxis = new Vector3(1, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
 //       StartMirroring();
    }

    // Update is called once per frame
    void Update()
    {
        MirrorMovement();

        DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartMirroring();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopMirroring();
        }
    }
    
    private void MirrorMovement()
    {
        if (!_isMirroring)
        {
            return;
        }
        
        var newPosition = (_transformToMirror.position - _startingPositionToMirror);
        newPosition.x *= mirroredAxis.x;
        newPosition.y *= mirroredAxis.y;
        newPosition.z *= mirroredAxis.z;

        this.transform.position = newPosition + _startingPosition;
    }

    public void SetGameObjectToMirror(GameObject go)
    {
        GameObjectToMirror = go;
    }
    
    public void StartMirroring()
    {
        if (_isMirroring)
        {
            return;
        }
        
        _startingPosition = this.transform.position;
        _startingPositionToMirror = GameObjectToMirror.transform.position;
        _transformToMirror = GameObjectToMirror.transform;
        _isMirroring = true;
    }

    public void StopMirroring()
    {
        this.transform.position = _startingPosition;
        _isMirroring = false;
    }
}
