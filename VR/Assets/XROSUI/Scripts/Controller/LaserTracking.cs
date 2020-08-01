using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class LaserTracking : MonoBehaviour
{
    XRGrabInteractable m_GrabInteractable;
    MeshRenderer m_MeshRenderer;

    private GameObject LaserEmitter;

    public bool m_Held = false;

    void OnEnable()
    {
        m_GrabInteractable = GetComponent<XRGrabInteractable>();
        m_MeshRenderer = GetComponent<MeshRenderer>();

        m_GrabInteractable.onFirstHoverEnter.AddListener(OnHoverEnter);
        m_GrabInteractable.onLastHoverExit.AddListener(OnHoverExit);
        m_GrabInteractable.onSelectEnter.AddListener(OnGrabbed);
        m_GrabInteractable.onSelectExit.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        m_GrabInteractable.onFirstHoverEnter.RemoveListener(OnHoverEnter);
        m_GrabInteractable.onLastHoverExit.RemoveListener(OnHoverExit);
        m_GrabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        m_GrabInteractable.onSelectExit.RemoveListener(OnReleased);
    }

    private void OnGrabbed(XRBaseInteractor obj)
    {
        this.LaserEmitter = obj.attachTransform.gameObject;
        m_MeshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.OnGrab);
        m_Held = true;
    }

    void OnReleased(XRBaseInteractor obj)
    {
        m_MeshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.Default);
        m_Held = false;
    }

    void OnHoverExit(XRBaseInteractor obj)
    {
        if (!m_Held)
        {
            m_MeshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.Default);
        }
    }

    void OnHoverEnter(XRBaseInteractor obj)
    {
        if (!m_Held)
        {
            m_MeshRenderer.material.color = Core.Ins.UIEffectsManager.GetColor(Enum_XROSUI_Color.OnHover);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_Held)
        {
            this.Lockon();
        }
    }

    void Lockon()
    {
        Vector3 direction = this.transform.position - LaserEmitter.transform.position;
        LaserEmitter.transform.forward = direction;
    }
}
