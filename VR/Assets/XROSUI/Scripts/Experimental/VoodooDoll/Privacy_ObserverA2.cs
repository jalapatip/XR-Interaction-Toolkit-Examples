using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class Privacy_ObserverA2 : MonoBehaviour
{
    private TrackedPoseDriver _driver;

    [SerializeField]
    private ENUM_XROS_AnatomyParts AnatomyParts;

    private Renderer _renderer;
    private MaterialPropertyBlock _activatedMBP;
    private MaterialPropertyBlock _deactivatedMBP;

    // Start is called before the first frame update
    void Start()
    {
        _driver = this.GetComponent<TrackedPoseDriver>();
        _renderer = this.GetComponent<Renderer>();
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;

        _activatedMBP = new MaterialPropertyBlock();
        _deactivatedMBP = new MaterialPropertyBlock();
        _activatedMBP.SetColor("_BaseColor", new Color(0, 116, 221, 255));
        _deactivatedMBP.SetColor("_BaseColor", new Color(221, 0, 0, 255));
    }

    private Vector3 _lastKnownPosition;

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts e, bool b, ENUM_XROS_PrivacyObserver o)
    {
        if (e != AnatomyParts) return;

        if (_driver)
        {
            if (!b)
            {
                _lastKnownPosition = this.transform.position;
            }

            _driver.enabled = b;

            if (!b)
            {
                this.transform.position = _lastKnownPosition;
            }
        }

        HandleVisualChange(b);
    }
    
    private void HandleVisualChange(bool changedBool)
    {
        if (_renderer)
        {
            if (changedBool)
            {
                _renderer.material.SetColor("_BaseColor", Color.blue);
//                print("blue");
                //_renderer.SetPropertyBlock(_activatedMBP);
            }
            else
            {
                _renderer.material.SetColor("_BaseColor", Color.red);
//                print("red");
                //_renderer.SetPropertyBlock(_deactivatedMBP);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}