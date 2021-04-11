using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public enum CalibrationPointTypes
{
    NotSet,
    Head,
    TopOfHead,
    LeftEye,
    RightEye,
    EyeLevel,
    LeftEar,
    RightEar,
    LeftShoulder,
    RightShoulder,
    LeftElbow,
    RightElbow,
    LeftHand,
    RightHand,
    LeftHip,
    RightHip,
    Waist,
    LeftKnee,
    RightKnee,
    LeftFoot,
    RightFoot
}

[RequireComponent(typeof(Transform))]
public class MeasurementsV2 : MonoBehaviour
{
    public GameObject skeleton;
    public GameObject calibrationPoint;
    public Text LeftArmLengthText;
    public Text LeftArmInstructionText;
    public string fileName = "/usercalibration2.csv";


    private int _stepCounter = 0;
    private Transform _leftControllerTransform;
    private Transform _rightControllerTransform;
    private Transform _hmdTransform;

    private Vector2 _centerPoint;
    private float _chestWidthX = 0.0f;
    private float _kneeWidthX = 0.0f;
    private float shoulderLx = 0.0f;
    private float shoulderRx = 0.0f;
    private float kneeLx = 0.0f;
    private float kneeRx = 0.0f;

    //We keep track of GO that represents calibrated points here through the GO's XRCalibrationPoint script
    private Dictionary<CalibrationPointTypes, XrCalibrationPoint>
        _myPoints = new Dictionary<CalibrationPointTypes, XrCalibrationPoint>();

    private void Start()
    {
        // Get VR Headset & Controller from Core's XrManager
        _leftControllerTransform = Core.Ins.XRManager.GetLeftDirectController().transform;
        _rightControllerTransform = Core.Ins.XRManager.GetRightDirectControllerGO().transform;
        _hmdTransform = Core.Ins.XRManager.GetXrCamera().transform;

        InstantiateCalibrationPoint(CalibrationPointTypes.Head);
        InstantiateCalibrationPoint(CalibrationPointTypes.TopOfHead);
        InstantiateCalibrationPoint(CalibrationPointTypes.EyeLevel);
        InstantiateCalibrationPoint(CalibrationPointTypes.LeftShoulder);
        InstantiateCalibrationPoint(CalibrationPointTypes.RightShoulder);
        InstantiateCalibrationPoint(CalibrationPointTypes.LeftElbow);
        InstantiateCalibrationPoint(CalibrationPointTypes.RightElbow);
        InstantiateCalibrationPoint(CalibrationPointTypes.LeftHand);
        InstantiateCalibrationPoint(CalibrationPointTypes.RightHand);
        InstantiateCalibrationPoint(CalibrationPointTypes.Waist);
        InstantiateCalibrationPoint(CalibrationPointTypes.LeftHip);
        InstantiateCalibrationPoint(CalibrationPointTypes.RightHip);
        InstantiateCalibrationPoint(CalibrationPointTypes.LeftKnee);
        InstantiateCalibrationPoint(CalibrationPointTypes.RightKnee);
        InstantiateCalibrationPoint(CalibrationPointTypes.LeftFoot);
        InstantiateCalibrationPoint(CalibrationPointTypes.RightFoot);

        LeftArmInstructionText.text =
            "Grab the orange Calibration Cube with the grip button on the controller. Then use the trigger button on the controller to start";
    }

    private void InstantiateCalibrationPoint(CalibrationPointTypes type)
    {
        var point = Instantiate(calibrationPoint, Vector3.zero, Quaternion.identity);
        point.transform.SetParent(skeleton.transform, false);

        var p = point.GetComponent<XrCalibrationPoint>();
        p.calibrationType = type;

        this._myPoints.Add(type, p);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Calibrate();
        }
    }

    public void Calibrate()
    {
        var rightControllerPos = _rightControllerTransform.position;
        var leftControllerPos = _leftControllerTransform.position;
        var hmdPos = _hmdTransform.position;
        var x = hmdPos.x;
        var y = hmdPos.y;
        var z = hmdPos.z;

        if (_stepCounter == 0)
        {
            LeftArmInstructionText.text =
                $"Step 1. While standing with both arms straight down, activate the calibration cube with the trigger button to continue";
            _stepCounter++;
        }
        else if (_stepCounter == 1) // Measure both arms Vertically
        {
            var pointA = new Vector2(hmdPos.x, hmdPos.z); //Head XZ position
            _centerPoint = pointA;
            var pointB = new Vector2(rightControllerPos.x, rightControllerPos.z); //Right hand XZ position
            shoulderRx = Vector2.Distance(pointA, pointB);
            pointB = new Vector2(leftControllerPos.x, leftControllerPos.z);
            shoulderLx = Vector2.Distance(pointA, pointB);
            _chestWidthX = Math.Abs(shoulderLx + shoulderRx);

            _myPoints[CalibrationPointTypes.LeftHand].SetPositionY(leftControllerPos.y);
            _myPoints[CalibrationPointTypes.RightHand].SetPositionY(rightControllerPos.y);
            //If we want to use the average, replace below with chestWidthX/2 and apply - for left and + for right
            _myPoints[CalibrationPointTypes.LeftHand].SetPositionX(-shoulderLx);
            _myPoints[CalibrationPointTypes.RightHand].SetPositionX(shoulderRx);
            _myPoints[CalibrationPointTypes.LeftShoulder].SetPositionX(-shoulderLx);
            _myPoints[CalibrationPointTypes.RightShoulder].SetPositionX(shoulderRx);
            _myPoints[CalibrationPointTypes.LeftElbow].SetPositionX(-shoulderLx);
            _myPoints[CalibrationPointTypes.RightElbow].SetPositionX(shoulderRx);

            LeftArmInstructionText.text =
                $"Step 2. While standing with arms straight down by your side. Bend both elbows 90 degrees so that the forearms are parallel with the floor. \nActivate the calibration cube with the trigger button to continue.";
            _stepCounter++;
        }
        else if (_stepCounter == 2) // Measure both elbows at 90 degrees
        {
            _myPoints[CalibrationPointTypes.LeftElbow].SetPositionY(leftControllerPos.y);
            _myPoints[CalibrationPointTypes.RightElbow].SetPositionY(rightControllerPos.y);

            LeftArmInstructionText.text =
                $"Step 3. While standing with both arms straight down by your side. Raise both arms 90 degrees so that they are parallel to the floor. \nActivate the calibration cube with the trigger button to continue.";
            _stepCounter++;
        }
        else if (_stepCounter == 3) // Measure both arms parallel to the floor
        {
            _myPoints[CalibrationPointTypes.LeftShoulder].SetPositionY(leftControllerPos.y);
            _myPoints[CalibrationPointTypes.RightShoulder].SetPositionY(rightControllerPos.y);

            LeftArmInstructionText.text =
                $"Step 4. While standing. Position your right controller to top of your head. \nActivate the calibration cube with the trigger button to continue.";
            _stepCounter++;
        }
        else if (_stepCounter == 4) //Measure use's height
        {
            _myPoints[CalibrationPointTypes.TopOfHead].SetPositionY(rightControllerPos.y);

            LeftArmInstructionText.text =
                $"Step 4. While standing. Position your right controller to your mid waist. \nActivate the calibration cube with the trigger button to continue.";
            _stepCounter++;
        }
        else if (_stepCounter == 5) // Measure waist
        {
            _myPoints[CalibrationPointTypes.Waist].SetPositionY(rightControllerPos.y);

            LeftArmInstructionText.text =
                $"Step 5. While standing, position your left controller to your left hip and right controller to your right hip. \nActivate the calibration cube with the trigger button to continue.";
            _stepCounter++;
        }
        else if (_stepCounter == 6) //measure hip
        {
            _myPoints[CalibrationPointTypes.LeftHip].SetPositionY(leftControllerPos.y);
            _myPoints[CalibrationPointTypes.RightHip].SetPositionY(rightControllerPos.y);

            LeftArmInstructionText.text =
                $"Step 5. While standing, position your left controller to your left knee and right controller to your right knee. Do not bend your knees. \nActivate the calibration cube with the trigger button to continue.";
            _stepCounter++;
        }
        else if (_stepCounter == 7) //measure knee
        {
            _myPoints[CalibrationPointTypes.LeftKnee].SetPositionY(leftControllerPos.y);
            _myPoints[CalibrationPointTypes.RightKnee].SetPositionY(rightControllerPos.y);

            //var pointA = new Vector2(hmdPos.x, hmdPos.z);
            var pointB = new Vector2(rightControllerPos.x, rightControllerPos.z);
            kneeRx = Vector2.Distance(_centerPoint, pointB);
            pointB = new Vector2(leftControllerPos.x, leftControllerPos.z);
            kneeLx = Vector2.Distance(_centerPoint, pointB);
            _kneeWidthX = Math.Abs(kneeRx + kneeLx);

            _myPoints[CalibrationPointTypes.LeftKnee].SetPositionX(-kneeLx);
            _myPoints[CalibrationPointTypes.RightKnee].SetPositionX(kneeRx);
            _myPoints[CalibrationPointTypes.LeftHip].SetPositionX(-kneeLx);
            _myPoints[CalibrationPointTypes.RightHip].SetPositionX(kneeRx);

            LeftArmInstructionText.text =
                $"Step 5. While standing, position your left controller to your left foot and right controller to your right foot. \nActivate the calibration cube with the trigger button to continue.";
            _stepCounter++;
        }
        else if (_stepCounter == 8) //measure feet
        {
            _myPoints[CalibrationPointTypes.LeftFoot].SetPositionY(leftControllerPos.y);
            _myPoints[CalibrationPointTypes.RightFoot].SetPositionY(rightControllerPos.y);
            _myPoints[CalibrationPointTypes.LeftFoot].SetPositionX(-kneeLx);
            _myPoints[CalibrationPointTypes.RightFoot].SetPositionX(kneeRx);

            SaveUserData();
            
            LeftArmInstructionText.text = $"";
            _stepCounter = 0;
        }
    }

    public void SaveUserData()
    {
        DataContainer_User user = new DataContainer_User
        {
            height = _myPoints[CalibrationPointTypes.TopOfHead].GetY(),
            LarmLength = _myPoints[CalibrationPointTypes.LeftShoulder].GetY() -
                         _myPoints[CalibrationPointTypes.LeftHand].GetY(), //Derived
            RarmLength = _myPoints[CalibrationPointTypes.RightShoulder].GetY() -
                         _myPoints[CalibrationPointTypes.RightHand].GetY(), //Derived
            Lshoulderx = _myPoints[CalibrationPointTypes.LeftShoulder].GetX(),
            Lshouldery = _myPoints[CalibrationPointTypes.LeftShoulder].GetY(),
            Rshoulderx = _myPoints[CalibrationPointTypes.RightShoulder].GetX(),
            Rshouldery = _myPoints[CalibrationPointTypes.RightShoulder].GetY(),
            chestWidth = _myPoints[CalibrationPointTypes.RightShoulder].GetX() -
                         _myPoints[CalibrationPointTypes.LeftShoulder].GetX(), //Derived
            Lelbowy = _myPoints[CalibrationPointTypes.LeftElbow].GetY(),
            Relbowy = _myPoints[CalibrationPointTypes.RightElbow].GetY(),
            Lkneey = _myPoints[CalibrationPointTypes.LeftKnee].GetY(),
            Rkneey = _myPoints[CalibrationPointTypes.RightKnee].GetY()
        };
        print(user.JSONdata);
        print("height: " + user.height);
       // user.ConvertToJSON();
       // File.WriteAllText(Application.persistentDataPath + fileName, user.JSONdata);
        File.WriteAllText(Application.persistentDataPath + fileName, DataContainer_User.HeaderToString() + user.ToString());
            
#if UNITY_EDITOR
        EditorUtility.RevealInFinder(Application.persistentDataPath + fileName);
#endif
        LeftArmLengthText.text =
            $"Left Arm length: {user.LarmLength} Right Arm length: {user.RarmLength} Height: {user.height} LShoulder: {user.Lshouldery} RShoulder: {user.Rshouldery} LElbow: {user.Lelbowy} Relbow: {user.Relbowy} Lknee: {user.Lkneey} Rknee: {user.Rkneey} chest: {_chestWidthX} kneew: {_kneeWidthX}";
    }

}