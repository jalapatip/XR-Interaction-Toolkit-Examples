using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeXRControllerModelPrefabColor : MonoBehaviour
{
    public Material myMaterial;

    private XRController _myController;
    private Transform _myControllerModelTransform;
    private Renderer[] _myRenderers;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(0.5f));
    }
    
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
 
        _myController = this.GetComponent<XRController>();
        _myControllerModelTransform = _myController.modelTransform;

        _myRenderers = _myControllerModelTransform.GetComponentsInChildren<Renderer>();
        foreach (var r in _myRenderers)
        {
            //r.sharedMaterial.color = myColor;
            r.material = myMaterial;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
