using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowObjects : MonoBehaviour
{
    // Start is called before the first frame update
    public bool EnablePositionRetaining;
    public bool relative;//position relative to parent object
    public float RelativePosition_x;
    public float RelativePosition_y;
    public float RelativePosition_z;
    public GameObject HidenObject;
    public GameObject ParentObject;
    MeshRenderer m_Renderer;
    XRGrabInteractable m_XRGrabInteractable;
    XRRayInteractor m_XRRayInteractor;

    void Start()
    {
        m_Renderer = HidenObject.GetComponent<MeshRenderer>();
        m_XRGrabInteractable = HidenObject.GetComponent<XRGrabInteractable>();
        m_XRRayInteractor = ParentObject.GetComponent<XRRayInteractor>();
        // m_XRGrabInteractable.onSelectEnter.AddListener(OnGrabbed);
    }

    // Update is called once per frame
    void Update()
    {
        if (EnablePositionRetaining)
        {
            if (!relative)
            {
                HidenObject.transform.position =
            ParentObject.transform.position +
            new Vector3(RelativePosition_x, RelativePosition_y, RelativePosition_z);
            }
            else
            {
                HidenObject.transform.position = ParentObject.transform.position +
                ParentObject.transform.forward * RelativePosition_z +
                ParentObject.transform.up * RelativePosition_y +
                ParentObject.transform.right * RelativePosition_x;
            }
        }
    }

    public void Show()
    {
        m_Renderer.enabled = true;
        m_XRGrabInteractable.enabled = true;
    }

    public void Hide()
    {
        m_Renderer.enabled = true;
        m_XRGrabInteractable.enabled = true;
    }
}
