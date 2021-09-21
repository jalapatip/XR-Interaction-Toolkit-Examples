using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script register itself to Core.Ins.XRManager for others to access
/// It should also take the user's physical information to place the center of the head based. Presumably from Core.Ins.HumanFactor or something//TODO
/// </summary>
public class DerivedHeadLocation : MonoBehaviour
{
    private void OnEnable()
    {
        Core.Ins.XRManager.RegisterPredictedHead(this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
