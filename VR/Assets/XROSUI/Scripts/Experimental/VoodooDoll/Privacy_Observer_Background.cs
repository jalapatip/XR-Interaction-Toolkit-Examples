using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class Privacy_Observer_Background : MonoBehaviour
{
    [SerializeField]
    private ENUM_XROS_AnatomyParts AnatomyParts;

    private Renderer _renderer;

    private MaterialPropertyBlock _activatedMBP;
    private MaterialPropertyBlock _deactivatedMBP;

    public Texture texture1;
    public Texture texture2;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = this.GetComponent<Renderer>();
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;

        _activatedMBP = new MaterialPropertyBlock();
        _deactivatedMBP = new MaterialPropertyBlock();
        //_activatedMBP.SetColor("_BaseColor", Color.gray);
        //_deactivatedMBP.SetColor("_BaseColor", Color.black);
        
        _activatedMBP.SetTexture("_EmissionMap", texture1);
        _deactivatedMBP.SetTexture("_EmissionMap", texture2);
        
    }

    private Vector3 _lastKnownPosition;

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts e, bool b)
    {
        if (e != AnatomyParts) return;
        
        if (_renderer)
        {
            if (b)
            {
                _renderer.SetPropertyBlock(_activatedMBP);
            }
            else
            {
                _renderer.SetPropertyBlock(_deactivatedMBP);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}