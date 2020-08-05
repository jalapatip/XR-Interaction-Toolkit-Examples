using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class VREquipment : MonoBehaviour
{
    protected XRGrabInteractable _grabInteractable;
    protected MeshRenderer _meshRenderer;

    protected static Color m_UnityMagenta = new Color(0.929f, 0.094f, 0.278f);
    protected static Color m_UnityCyan = new Color(0.019f, 0.733f, 0.827f);

    public bool m_Held = false;
    private bool bInSocket = false;
    float lastHeldTime;

    public float timeBeforeReturn = 0.5f;
    public GameObject socket;
    public XROSMenuTypes menuTypes = XROSMenuTypes.Menu_General;
    Rigidbody m_Rigidbody;
    void OnEnable()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _meshRenderer = GetComponent<MeshRenderer>();
        m_Rigidbody = GetComponent<Rigidbody>();
        _grabInteractable.onFirstHoverEnter.AddListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.AddListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.AddListener(OnHoverExit);
        _grabInteractable.onSelectEnter.AddListener(OnGrabbed);
        _grabInteractable.onSelectExit.AddListener(OnReleased);
        _grabInteractable.onActivate.AddListener(OnActivated);
        _grabInteractable.onDeactivate.AddListener(OnDeactivated);
    }

    private void OnDisable()
    {
        _grabInteractable.onFirstHoverEnter.RemoveListener(OnFirstHoverEnter);
        _grabInteractable.onHoverEnter.RemoveListener(OnHoverEnter);
        _grabInteractable.onLastHoverExit.RemoveListener(OnHoverExit);
        _grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        _grabInteractable.onSelectExit.RemoveListener(OnReleased);
        _grabInteractable.onActivate.RemoveListener(OnActivated);
        _grabInteractable.onDeactivate.RemoveListener(OnDeactivated);
    }

    public virtual void OnActivated(XRBaseInteractor obj)
    {
        //print("Activated " + this.name);
    }
    public virtual void OnDeactivated(XRBaseInteractor obj)
    {
        //print("Deactivated " + this.name);
    }

    private void OnGrabbed(XRBaseInteractor obj)
    {
        _meshRenderer.material.color = m_UnityCyan;
        //print("Grabbed: " + this.name);
        m_Held = true;
        bInSocket = false;
        this.transform.SetParent(null);
    }

    void OnReleased(XRBaseInteractor obj)
    {
        // print("Released");
        _meshRenderer.material.color = Color.white;
        m_Held = false;
        m_Rigidbody.ResetCenterOfMass();
        m_Rigidbody.ResetInertiaTensor();
        m_Rigidbody.angularDrag = 0;
        m_Rigidbody.angularVelocity = Vector3.zero;
        //m_Rigidbody.sle1 = 0;
    }

    void OnHoverExit(XRBaseInteractor obj)
    {
        if (!m_Held)
        {
            _meshRenderer.material.color = Color.white;
        }
    }

    private void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        if (!m_Held)
        {
            //print("Hover: " + this.name);
            _meshRenderer.material.color = m_UnityMagenta;
        }
    }

    private void OnHoverEnter(XRBaseInteractor obj)
    {
        if (!m_Held)
        {
            //Vibrate
            //foreach(XRBaseInteractor hi in this.m_GrabInteractable.hoveringInteractors)
            //{

            //}
        }
    }

    public virtual void AlternateFunction()
    {
        Dev.Log("Alternate Function: " + this.name);
    }

    // need to be fixed
    //public virtual void TriggerFunction()
    //{

    //}

    public virtual void HandleGesture(ENUM_XROS_Gesture gesture, float distance)
    {

    }

    private void Update()
    {
        if (m_Held)
        {
            lastHeldTime = Time.time;
        }
        else if (!m_Held && Time.time > lastHeldTime + timeBeforeReturn)
        {
            if (!bInSocket)
            {
                this.transform.localRotation = Quaternion.identity;
                this.transform.position = socket.transform.position;

                this.transform.SetParent(socket.transform);
                m_Rigidbody.ResetCenterOfMass();
                m_Rigidbody.ResetInertiaTensor();
                m_Rigidbody.angularDrag = 0;
                m_Rigidbody.angularVelocity = Vector3.zero;
                m_Rigidbody.velocity = Vector3.zero;
                this.transform.localRotation = Quaternion.identity;
                this.transform.position = socket.transform.position;

                bInSocket = true;
            }
        }
    }
}
