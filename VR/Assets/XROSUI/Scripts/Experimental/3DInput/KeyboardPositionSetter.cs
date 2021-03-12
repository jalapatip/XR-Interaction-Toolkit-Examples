using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This is a virtual keyboard that is used to set the position of the subsequent spheres
/// 
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class KeyboardPositionSetter : MonoBehaviour
{
    [Tooltip("Assign from Project PF_CharacterCreator_Hemisphere")]
    public GameObject hemisphere;

    //Can probably get this from hemisphere and don't need to drag this 
    [FormerlySerializedAs("kcc")]
    [Tooltip("Assign from Hierarchy PF_CharacterCreator_Hemisphere/Character Creator")]
    public SeparateKeyboardCharacterCreator keyboardCharacterCreator;

    [Tooltip("Assign from Project KeyboardController")]
    public GameObject controllerPF;

    [Tooltip("Customize in inspector")]
    public float ScaleNumber;

    private XRGrabInteractable _grabInteractableBase;
    private Transform _keyboardPosition;

    private GameObject _leftDirectController;
    private GameObject _rightDirectController;
    private GameObject _leftRayController;
    private GameObject _rightRayController;

    private void Start()
    {
        //TODO isn't this just transform?
        _keyboardPosition = gameObject.GetComponent<Transform>();
        _grabInteractableBase = GetComponent<XRGrabInteractable>();
        _grabInteractableBase.onDeactivate.AddListener(OnDeactivate);
        _grabInteractableBase.onSelectEnter.AddListener(OnSelectEnter);

        _rightDirectController = Core.Ins.XRManager.GetRightDirectControllerGO();
        _rightRayController = Core.Ins.XRManager.GetRightRayControllerGO();
        _leftDirectController = Core.Ins.XRManager.GetLeftDirectController();
        _leftRayController = Core.Ins.XRManager.GetLeftRayController();
    }

    private void OnSelectEnter(XRBaseInteractor obj)
    {
        //tell the Core user start keyboard successfully.
        Core.Ins.ScenarioManager.SetFlag("GrabingKeyboard", true);
    }

    private void OnDeactivate(XRBaseInteractor obj)
    {
        if (keyboardCharacterCreator.isHovering)
        {
            return;
        }

        if (keyboardCharacterCreator.active)
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
        if (keyboardCharacterCreator.active)
        {
            Core.Ins.ScenarioManager.SetFlag("TurnOffKeyboard", true); //tell the Core user start keyboard successfully.

            keyboardCharacterCreator.DestroyPoints();
            keyboardCharacterCreator.active = false;

            ReduceControllerSizeForTyping(false);
        }
    }

    private void ReduceControllerSizeForTyping(bool toReduce)
    {
        if (toReduce)
        {
            _leftRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 0;
            _rightRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 0;
            _leftDirectController.transform.localScale = new Vector3(ScaleNumber, ScaleNumber, ScaleNumber);

            Core.Ins.XRManager.GetRightDirectControllerGO().transform.localScale =
                new Vector3(ScaleNumber, ScaleNumber, ScaleNumber);
            _leftRayController.transform.localScale = new Vector3(ScaleNumber, ScaleNumber, ScaleNumber);
            _rightRayController.transform.localScale = new Vector3(ScaleNumber, ScaleNumber, ScaleNumber);
        }
        else
        {
            //Return Controller to original size
            _leftRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 10;
            _rightRayController.GetComponent<XRRayInteractor>().maxRaycastDistance = 10;
            this.TransformController(_leftDirectController, true);
            this.TransformController(_rightDirectController, true);

            _leftDirectController.transform.localScale = new Vector3(1, 1, 1);
            _rightDirectController.transform.localScale = new Vector3(1, 1, 1);
            _leftRayController.transform.localScale = new Vector3(1, 1, 1);
            _rightRayController.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void TurnOffKeyboard(XRBaseInteractor obj)
    {
        //tell the Core user start keyboard successfully.
        Core.Ins.ScenarioManager.SetFlag("TurnOffKeyboard", true);

        keyboardCharacterCreator.SaveKeyPositions();
        keyboardCharacterCreator.DestroyPoints();
        keyboardCharacterCreator.active = false;
        ReduceControllerSizeForTyping(false);
    }

    private void TurnOnKeyboard(XRBaseInteractor obj)
    {
        //tell the Core user end keyboard successfully.
        Core.Ins.ScenarioManager.SetFlag("TurnOnKeyboard", true);

        var position = _keyboardPosition.position;
        keyboardCharacterCreator.CreateMirrorKeyboard(position.x, position.y, position.z);
        keyboardCharacterCreator.active = true;

        ReduceControllerSizeForTyping(true);
        LookAtCamera(obj);
    }

    void TransformController(GameObject controller, bool large)
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
        DebugUpdate();
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TurnOnKeyboard(_leftDirectController.GetComponent<XRBaseInteractor>());
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            TurnOffKeyboard(_leftDirectController.GetComponent<XRBaseInteractor>());
        }
    }

    private void LookAtCamera(XRBaseInteractor obj)
    {
        hemisphere.transform.RotateAround(obj.transform.position, transform.up,
            Camera.main.gameObject.transform.rotation.eulerAngles.y);

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