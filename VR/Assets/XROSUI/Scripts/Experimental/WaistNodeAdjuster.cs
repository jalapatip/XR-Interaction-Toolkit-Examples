using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WaistNodeAdjuster : XrInteractable
{
    public GameObject HeadLocation;
    public Vector3 Offset = new Vector3(0, -0.5f, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        //HeadLocation = Core.Ins.XRManager.GetXrCamera().gameObject;
        if (!HeadLocation)
        {
            HeadLocation = Core.Ins.XRManager.GetXrCamera().gameObject;    
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_grabInteractable.isSelected)
        {
            
        }
        else
        {
            UpdateLocation();    
        }
        
    }

    private void UpdateLocation()
    {
//        print("Head Rotation: " + HeadLocation2.GetComponent<Transform>().eulerAngles);
//        print("Head Rotation2: " + HeadLocation2.GetComponent<Transform>().localEulerAngles);

        var rot = HeadLocation.transform.localEulerAngles;
        var pot = HeadLocation.transform.localPosition;
//        print(rot);
        //this.transform.localEulerAngles = new Vector3(rot.x, 0, rot.z);
        

        //this.transform.localPosition = pot + Offset;
        var transform1 = transform;
        transform1.localEulerAngles = new Vector3(0, rot.y, 0);
        transform1.localPosition = pot + transform1.up*Offset.y + transform1.forward *Offset.z + transform1.right *Offset.x;
    }

    protected override void OnSelectEnter(XRBaseInteractor obj)
    {
        
    }

    protected override void OnSelectExit(XRBaseInteractor obj)
    {
        Offset = transform.position - HeadLocation.transform.position;
        //print(Offset);
    }
}
