using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAvatarController : MonoBehaviour
{
    public ENUM_XROS_PrivacyObserver observerType; 
    // Start is called before the first frame update
    void Start()
    {
        var observers = this.GetComponentsInChildren<Privacy_Observer>();
        foreach(var o in observers)
        {
            o.observerType = this.observerType;
        }
        
        var observers2 = this.GetComponentsInChildren<Privacy_Observer_Background>();
        foreach(var o in observers2)
        {
            o.observerType = this.observerType;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
