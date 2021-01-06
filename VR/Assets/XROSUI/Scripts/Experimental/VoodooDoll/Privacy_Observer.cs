using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class Privacy_Observer : MonoBehaviour
{
    private TrackedPoseDriver _driver;

    [SerializeField]
    private ENUM_XROS_AnatomyParts AnatomyParts;

    private Renderer _renderer;

    private MaterialPropertyBlock _activatedMBP;
    private MaterialPropertyBlock _deactivatedMbp;

    // Start is called before the first frame update
    void Start()
    {
        _driver = this.GetComponent<TrackedPoseDriver>();
        _renderer = this.GetComponent<Renderer>();
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;

        _activatedMBP = new MaterialPropertyBlock();
        _deactivatedMbp = new MaterialPropertyBlock();
        _activatedMBP.SetColor("_BaseColor", Color.gray);
        _deactivatedMbp.SetColor("_BaseColor", Color.black);
    }

    private Vector3 _lastKnownPosition;

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts e, bool b)
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

        if (_renderer)
        {
            if (b)
            {
                _renderer.SetPropertyBlock(_activatedMBP);
            }
            else
            {
                _renderer.SetPropertyBlock(_deactivatedMbp);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}