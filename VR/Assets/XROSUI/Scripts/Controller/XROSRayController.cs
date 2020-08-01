using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;


public class XROSRayController : MonoBehaviour
{
    LineRenderer m_lineRenderer;
    XRInteractorLineVisual m_XRInteractorLineVisual;
    XRRayInteractor m_RayInteractor;
    public LaserLengthChange grabbedTarget;
    public LaserTracking laserTracker;
    private bool _isGrabbing;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        this.m_lineRenderer = this.GetComponent<LineRenderer>();
        this.m_RayInteractor = GetComponent<XRRayInteractor>(); ;
        this.m_XRInteractorLineVisual = this.GetComponent<XRInteractorLineVisual>();
        this._isGrabbing = false;

        //m_RayInteractor.onHoverEnter
        //m_RayInteractor.onHoverExit
        //m_RayInteractor.onSelectEnter
        //m_RayInteractor.onSelectExit        
    }

    public void OnGrab(/*XRBaseInteractor obj*/)
    {
        if (!this._isGrabbing)
        {
            this._isGrabbing = true;
        }
    }

    public void OnRelease(/*XRBaseInteractor obj*/)
    {
        if (this._isGrabbing)
        {
            this._isGrabbing = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (this._isGrabbing && !grabbedTarget.grabbed && !laserTracker.m_Held)
        {
            // print("onGrab grabbed="+(grabbedTarget.grabbed? "true":"false"));
            // this.lineRenderer.enabled=false;
            if (this.m_XRInteractorLineVisual.enabled)
            {
                this.m_XRInteractorLineVisual.enabled = false;
            }
        }
        else
        {
            if (!this.m_XRInteractorLineVisual.enabled)
            {
                this.m_XRInteractorLineVisual.enabled = true;
            }
        }
    }
}
