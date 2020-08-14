using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// This Manager interfaces between XRITK's Ray Controller and our own LaserEmitterPositionRetainer, LaserLengthAdjuster, LaserTracker
/// </summary>
[RequireComponent(typeof(XRInteractorLineVisual))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(XRRayInteractor))]
public class XrosRayControllerManager : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private XRInteractorLineVisual _xrInteractorLineVisual;
    private XRRayInteractor _rayInteractor;

    public LaserLengthAdjuster grabbedTarget;
    private LaserTracking _laserTracker;

    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rayInteractor = GetComponent<XRRayInteractor>();
        _xrInteractorLineVisual = GetComponent<XRInteractorLineVisual>();

        _rayInteractor.onSelectEnter.AddListener(OnSelectEnter);
        _rayInteractor.onSelectExit.AddListener(OnSelectExit);
    }

    private void OnDisable()
    {
        _rayInteractor.onSelectEnter.RemoveListener(OnSelectEnter);
        _rayInteractor.onSelectExit.RemoveListener(OnSelectExit);
    }

    private void OnSelectEnter(XRBaseInteractable obj)
    {
        var newTracking = obj.GetComponent<LaserTracking>();
        if (newTracking)
        {
            _laserTracker = newTracking;
            //this._xrInteractorLineVisual.enabled = false;
            //_lineRenderer.enabled = true;
        }

        var newLaserLengthChange = obj.GetComponent<LaserLengthAdjuster>();
        if (newLaserLengthChange)
        {
            grabbedTarget = newLaserLengthChange;
        }
    }

    private void OnSelectExit(XRBaseInteractable obj)
    {
        _laserTracker = null;
        grabbedTarget = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_rayInteractor.isSelectActive && !grabbedTarget && !_laserTracker)
        {
            if (_xrInteractorLineVisual.enabled)
            {
                _xrInteractorLineVisual.enabled = false;
            }
        }
        else
        {
            _xrInteractorLineVisual.enabled = true;
        }
    }

    public void AdjustRayLength(XRRayInteractor interactor, float f)
    {
        interactor.maxRaycastDistance = f;
    }

    public void AdjustEmitterForwardDirection(XRRayInteractor interactor, Vector3 direction)
    {
        interactor.attachTransform.forward = direction;
    }
}