using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class VE_Mask : VE_EquipmentBase
{
    public float distanceBeforeShowingDoll = 0.3f;
    public GameObject GO_VoodooScene;
    public Renderer myRenderer;
    
    //private bool _IsDeployed = false;    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (this.IsSelected() && !_isInSocket && _isEquipped )
        {
//            print(Vector3.Distance(this.transform.position, this.socket.transform.position));
            if (Vector3.Distance(this.transform.position, this.socket.transform.position) > distanceBeforeShowingDoll)
            {
                GO_VoodooScene.SetActive(true);
                this.myRenderer.enabled = false;
                
                //GO_VoodooScene.transform.SetParent(this.transform);
                GO_VoodooScene.transform.position = this.transform.position;
                GO_VoodooScene.transform.rotation = this.transform.rotation;
                
            }
            else
            {
                HideVoodooDollIfNotDeployed();
            }
        }
    }

    private void HideVoodooDollIfNotDeployed()
    {
        this.myRenderer.enabled = true;
        
        if (!Core.Ins.Privacy.IsVoodooDollDeployed())
        {
            GO_VoodooScene.SetActive(false);
            //GO_VoodooScene.transform.SetParent(null);
        }
        
    }
    protected override void OnSelectExit(XRBaseInteractor obj)
    {
        base.OnSelectExit(obj);
        
        HideVoodooDollIfNotDeployed();
    }

    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
        
        Core.Ins.Privacy.DeployVoodooDoll(true);
        GO_VoodooScene.SetActive(true);
        //GO_VoodooScene.transform.SetParent(null);
        //_IsDeployed = true;
        //GameObject.Instantiate()
        // GO_VoodooScene.transform.position = this.transform.position;
        // GO_VoodooScene.transform.rotation = this.transform.rotation;
        // GO_VoodooScene.SetActive(true);
        //this.gameObject.SetActive(false);
    }
}
