using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class KeyboardPositionSetter : MonoBehaviour
{
    public SeparateKeyboardCharacterCreator kcc;
    public GameObject hemisphere;
    XRGrabInteractable m_InteractableBase;

    const string k_AnimTriggerDown = "TriggerDown";
    const string k_AnimTriggerUp = "TriggerUp";
    public GameObject leftDirectController;
    public GameObject rightDirectController;
    public GameObject leftRayController;
    public GameObject rightRayController;
    public GameObject controllerPF;
    public float ScaleNumber;
    Transform keyboardPosition;
    XRBaseInteractor controller;
    
    void Start()
    {
        keyboardPosition = gameObject.GetComponent<Transform>();
        m_InteractableBase = GetComponent<XRGrabInteractable>();
        m_InteractableBase.onDeactivate.AddListener(onDeactivate);
        m_InteractableBase.onSelectExit.AddListener(onSelectExit);
        m_InteractableBase.onSelectEnter.AddListener(onSelectEnter);

        rightDirectController = Core.Ins.XRManager.GetRightDirectController();
        rightRayController = Core.Ins.XRManager.GetRightRayController();
        leftDirectController = Core.Ins.XRManager.GetLeftDirectController();
        leftRayController = Core.Ins.XRManager.GetLeftRayController();
    }
    private void onSelectEnter(XRBaseInteractor obj)
    {
        Core.Ins.ScenarioManager.SetFlag("GrabingKeyboard", true);//tell the Core user start keyboard successfully.
    }
    private void onSelectExit(XRBaseInteractor obj)
    {

    }

    private void onDeactivate(XRBaseInteractor obj)
    {
        if (kcc.isHovering)
        {
            return;
        }
        
        if (kcc.active)
        {
            TurnOffKeyboard(obj);
        }
        else
        {
            TurnOnKeyboard(obj);
        }
    }
    
    public void SetDefaultKeyboard()
    {
        if (kcc.active)
        {
            Core.Ins.ScenarioManager.SetFlag("TurnOffKeyboard", true);//tell the Core user start keyboard successfully.
            
            kcc.DestroyPoints();
            kcc.active = false;
            leftRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 10;
            rightRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 10;
            this.Transform(leftDirectController, true);
            this.Transform(rightDirectController, true);

            leftDirectController.transform.localScale = new Vector3(1, 1, 1);
            rightDirectController.transform.localScale = new Vector3(1, 1, 1);
            leftRayController.transform.localScale = new Vector3(1, 1, 1);
            rightRayController.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void TurnOffKeyboard(XRBaseInteractor obj)
    {
        Core.Ins.ScenarioManager.SetFlag("TurnOffKeyboard", true);//tell the Core user start keyboard successfully.
        
        kcc.SaveKeyPositions();
        kcc.DestroyPoints();
        kcc.active = false;
        leftRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 10;
        rightRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 10;
        this.Transform(leftDirectController, true);
        this.Transform(rightDirectController, true);

        leftDirectController.transform.localScale = new Vector3(1, 1, 1);
        rightDirectController.transform.localScale = new Vector3(1, 1, 1);
        leftRayController.transform.localScale = new Vector3(1, 1, 1);
        rightRayController.transform.localScale = new Vector3(1, 1, 1);
    }

    private void TurnOnKeyboard(XRBaseInteractor obj)
    {
        Core.Ins.ScenarioManager.SetFlag("TurnOnKeyboard", true);//tell the Core user end keyboard successfully.
        kcc.CreateMirrorKeyboard(keyboardPosition.position.x, keyboardPosition.position.y, keyboardPosition.position.z);
        kcc.active = true;

        leftRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 0;
        rightRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 0;
        leftDirectController.transform.localScale = new Vector3(ScaleNumber, ScaleNumber, ScaleNumber);
        
        Core.Ins.XRManager.GetRightDirectController().transform.localScale = new Vector3(ScaleNumber, ScaleNumber, ScaleNumber);
        leftRayController.transform.localScale = new Vector3(ScaleNumber, ScaleNumber, ScaleNumber);
        rightRayController.transform.localScale = new Vector3(ScaleNumber, ScaleNumber, ScaleNumber);
        LookAtCamera(obj);
    }

    void Transform(GameObject controller, bool large)
    {
        int count = controller.transform.childCount;
        Vector3 size;
        if (large)
            size = new Vector3(1f, 1f, 1f);
        else
            size = new Vector3(0.5f, 0.5f, 0.5f);
        for (int i = 0; i < count; i++)
        {
            if (controller.transform.GetChild(i).name.Contains(" Model"))
            {
                controller.transform.GetChild(i).transform.localScale = size;
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TurnOnKeyboard(leftDirectController.GetComponent<XRBaseInteractor>());
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            TurnOffKeyboard(leftDirectController.GetComponent<XRBaseInteractor>());
        }
    }
    private void LookAtCamera(XRBaseInteractor obj)
    {
        hemisphere.transform.RotateAround(obj.transform.position, transform.up, Camera.main.gameObject.transform.rotation.eulerAngles.y);

        GameObject charactorCreator = hemisphere.transform.GetChild(0).gameObject;
        int childCount = charactorCreator.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject key = charactorCreator.transform.GetChild(i).gameObject;
            var rotationVector = key.transform.rotation.eulerAngles;
            rotationVector.x = Camera.main.gameObject.transform.rotation.eulerAngles.x;
            key.transform.rotation = Quaternion.Euler(rotationVector);
        }
    }
}
