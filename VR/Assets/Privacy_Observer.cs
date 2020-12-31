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

    // Start is called before the first frame update
    void Start()
    {
        _driver = this.GetComponent<TrackedPoseDriver>();
        _renderer = this.GetComponent<Renderer>();
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;
    }

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts e, bool b)
    {
        if (e == AnatomyParts)
        {
            if (_driver)
            {
                _driver.enabled = b;
            }

            if (_renderer)
            {
                if (b)
                {
                    
                    _renderer.material.color = Color.green;
                }
                else
                {
                    _renderer.material.color = Color.red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}