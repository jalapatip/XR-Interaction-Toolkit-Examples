using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrCalibrationPoint : MonoBehaviour
{
    public CalibrationPointTypes calibrationType;

    private XRGrabInteractable _grabInteractable;

    //private Transform _cachedTransform;
    // Start is called before the first frame update
    void Start()
    {
        this.name = PointName();
    }

    private void OnEnable()
    {
        if (!_grabInteractable)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }

        _grabInteractable.onActivate.AddListener(OnActivated);
    }

    private void OnDisable()
    {
        _grabInteractable.onActivate.RemoveListener(OnActivated);
    }

    private void OnActivated(XRBaseInteractor obj)
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetPosition(Vector3 pos)
    {
        this.transform.localPosition = pos;
    }

    public void SetPositionX(float newX)
    {
        var newVector = this.transform.localPosition;
        newVector.x = newX;
        this.transform.localPosition = newVector;
    }

    public void SetPositionY(float newY)
    {
        var newVector = this.transform.localPosition;
        newVector.y = newY;
        this.transform.localPosition = newVector;
    }

    public void SetPositionZ(float newZ)
    {
        var newVector = this.transform.localPosition;
        newVector.z = newZ;
        this.transform.localPosition = newVector;
    }

    public string PointName()
    {
        return calibrationType.ToString();
    }

    public float GetY()
    {
        return this.transform.position.y;
    }

    public float GetX()
    {
        return this.transform.position.x;
    }
}