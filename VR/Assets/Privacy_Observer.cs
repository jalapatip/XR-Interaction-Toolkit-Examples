using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class Privacy_Observer : MonoBehaviour
{
    public TrackedPoseDriver driver;
    public ENUM_XROS_AnatomyParts AnatomyParts;    
    
    // Start is called before the first frame update
    void Start()
    {
        driver = this.GetComponent<TrackedPoseDriver>();
        Manager_Privacy.EVENT_NewPrivacy += HandleAnatomyChange;
    }

    private void HandleAnatomyChange(ENUM_XROS_AnatomyParts e, bool b)
    {
        if (e == AnatomyParts)
        {
            if (driver)
            {
                driver.enabled = b;
            }
            
               
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
