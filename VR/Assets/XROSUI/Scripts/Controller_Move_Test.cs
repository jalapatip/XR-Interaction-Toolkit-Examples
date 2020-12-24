using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class Controller_Move_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public Vector3 translationRate = new Vector3(3, 3, 3);
    public Vector3 rotationRate = new Vector3(10, 10, 10);
    // Update is called once per frame
    void Update()
    {
        //Translation
        Vector3 tempVector3 = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            //print(this.transform.forward);
            tempVector3 += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            tempVector3 += -transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            tempVector3 += transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            tempVector3 += -transform.right;
        }
        if (Input.GetKey(KeyCode.R))
        {
            tempVector3 += transform.up;
        }
        if (Input.GetKey(KeyCode.F))
        {
            tempVector3 += -transform.up;
        }
        
        transform.position += Vector3.Scale(translationRate, tempVector3) * Time.deltaTime;
        //Rotation
        Vector3 tempRotationVector = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
        {
            tempRotationVector += -Vector3.up;
        }
        if (Input.GetKey(KeyCode.E))
        {
            tempRotationVector += Vector3.up;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            tempRotationVector += Vector3.right;
        }
        if (Input.GetKey(KeyCode.X))
        {
            tempRotationVector += -Vector3.right;
        }
        if (Input.GetKey(KeyCode.C))
        {
            tempRotationVector += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.V))
        {
            tempRotationVector += -Vector3.forward;
        }
        print(tempRotationVector);
        tempRotationVector += Vector3.Scale(rotationRate, tempRotationVector) * Time.deltaTime;
        transform.Rotate(Vector3.up, tempRotationVector.y);
        transform.Rotate(Vector3.forward, tempRotationVector.z);
        transform.Rotate(Vector3.right, tempRotationVector.x);
        
        //TODO Add Speed Adjustment
    }
}
