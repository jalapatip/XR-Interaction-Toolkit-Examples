using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] private Transform rootObject, followObject;
    [SerializeField] private Vector3 positionOffset, rotationOffset;

    private Vector3 _headBodyOffset;


    void Start()
    {
        _headBodyOffset = rootObject.position - followObject.position;

    }


    void LateUpdate()
    {

        rootObject.position = transform.position + _headBodyOffset;
        rootObject.forward = Vector3.ProjectOnPlane(followObject.up, Vector3.up).normalized;

        transform.position = followObject.TransformPoint(positionOffset);
        transform.rotation = followObject.rotation * Quaternion.Euler(rotationOffset);

    }

}
