using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using KeyboardPosition;
using TMPro;
[System.Serializable]
public class XRKey : MonoBehaviour
{
    private KeyboardController keyboardController;
    //public Text myText;
    public TMP_Text myText;
    private Color transparent = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    private XRGrabInteractable m_GrabInteractable;
    private MeshRenderer m_MeshRenderer;
    private static Color m_UnityMagenta = new Color(0.929f, 0.094f, 0.278f);
    private static Color m_UnityCyan = new Color(0.019f, 0.733f, 0.827f);
    private bool m_Held = false;
    private float hover_timer = 0;
    private bool hover_start = false;
    private Button Button_Timer;
    public  KeyWrapper kw; // to record this XRKey's position
    GameObject mirroredKey;
    MeshRenderer mirroredKeyRenderer;

    private float oldX = 0;
    private float oldY = 0;
    private float oldZ = 0;
    private Vector3 difference = new Vector3(0,0,0);
    public void Setup(string s, KeyboardController kc, KeyWrapper kw)
    {
        this.keyboardController = kc;
        this.name = "Key: " + s;
        myText.text = s;
        this.kw = kw;
    }

    public void Setup(string s, KeyboardController kc, Button button, KeyWrapper kw)
    {
        this.keyboardController = kc;
        this.name = "Key: " + s;
        this.Button_Timer = button;
        myText.text = s;
        this.kw = kw;
    }

    public void AssignMirroredKey(GameObject mirroredKey)
    {
        this.mirroredKey = mirroredKey;
        this.mirroredKeyRenderer = this.mirroredKey.GetComponent<MeshRenderer>();
        difference = mirroredKey.transform.position - transform.position;
        Destroy(this.mirroredKey.GetComponent<XRGrabInteractable>());
    }

    void OnEnable()
    {
        m_GrabInteractable = GetComponent<XRGrabInteractable>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_MeshRenderer.material.color = transparent;
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
        m_MeshRenderer.material.color = m_UnityCyan;
        m_Held = true;
        var position = gameObject.transform.position;
        oldX = position.x;
        oldY = position.y;
        oldZ = position.z;
    }

    void OnReleased(XRBaseInteractor obj)
    {
        m_MeshRenderer.material.color = Color.white;
        m_Held = false;
        var position = gameObject.transform.position;
        this.kw.x += position.x-oldX;
        this.kw.y += position.y-oldY;
        this.kw.z += position.z-oldZ;
        this.mirroredKey.transform.position = position + difference;
    }
    private void Update()
    {
        if (m_Held)
        {
        }
        if (hover_start)
        {
            hover_timer += Time.deltaTime;
        }

        if (hover_timer >= 2f)
        {
            hover_start = false;
            hover_timer = 0;
            Button_Timer.onClick.Invoke();
        }
    }

    void OnHoverExit(XRBaseInteractor obj)
    {
        if (this.gameObject == null)
            return;
        if (!m_Held)
        {
            m_MeshRenderer.material.color = transparent;
            if (this.mirroredKey)
            {
                //this.mirroredKey.GetComponent<MeshRenderer>().material.color = transparent;
                mirroredKeyRenderer.material.color = transparent;
            }
            hover_start = false;
            hover_timer = 0;
        }
        keyboardController.isHovering = false;
    }

    void OnHoverEnter(XRBaseInteractor obj)
    {
        keyboardController.isHovering = true;
        if (this.gameObject == null)
            return;

        if (myText.text == "start" && hover_start == false)
        {
            hover_start = true;
            return;
        }
        if (!m_Held & keyboardController.GetWaiting() == false)
        {
            if (myText.text == "DEL")
            {
                XROSInput.Backspace();
                m_MeshRenderer.material.color = m_UnityMagenta;
                return;
            }
            keyboardController.Wait();
            keyboardController.SetWaiting();
            keyboardController.RegisterInput(myText.text);
            XROSInput.AddInput(myText.text);
            m_MeshRenderer.material.color = m_UnityCyan;
            if (this.mirroredKey)
            {
                //this.mirroredKey.GetComponent<MeshRenderer>().material.color = m_UnityCyan;
                mirroredKeyRenderer.material.color = m_UnityCyan;
            }
        }
    }
}
