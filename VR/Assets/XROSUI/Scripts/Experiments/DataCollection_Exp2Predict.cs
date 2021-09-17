using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// https://docs.unity.cn/Packages/com.unity.barracuda@1.0/manual/GettingStarted.html
/// </summary>
public enum SlotPrediction
{
    //ERotRelative,
    QRotRelative
}

public class DataCollection_Exp2Predict : DataCollection_ExpBase
{
    public SlotPrediction slotPrediction;

    public GameObject _head;
    public GameObject _handR;
    public GameObject _handL;
    public GameObject _waist;
    public bool startPlaying = false;

    public NNModel modelSource;
    public TextAsset scalerSource;
    private IWorker _worker;
    private Dictionary<string, Scaler> _scalers = new Dictionary<string, Scaler>();
    private Single[] outputArray = new Single[8];

    // Start is called before the first frame update
    private void Start()
    {
        ExpName = "Exp2Predict";
        ReloadXrDevices();

        if (modelSource)
        {
            var model = ModelLoader.Load(modelSource);
            _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        }
        else
        {
            Dev.LogError(this.name + "No ModelSource " + modelSource);
        }

        if (scalerSource)
        {
            Scalers scalers = JsonUtility.FromJson<Scalers>(scalerSource.text);
            foreach (Scaler scaler in scalers.scalers)
            {
                _scalers.Add(scaler.type, scaler);
            }
        }
        else
        {
            Dev.LogError(this.name + " - No scalerSource ");
        }

        //Core.Ins.XRManager.GetLeftRayController().GetComponent<XRRayInteractor>().onSelectEnter.AddListener(PredictSlotTest);
        //Core.Ins.XRManager.GetRightRayController().onSelectEnter.AddListener(PredictSlotTest2);

        _isRecording = startPlaying;
    }

    private void ReloadXrDevices()
    {
        Dev.Log("Reload Xr Devices");
        if (!_head)
        {
            _head = Core.Ins.XRManager.GetXrCamera().gameObject;
            _handR = Core.Ins.XRManager.GetRightDirectControllerGO();
            _handL = Core.Ins.XRManager.GetLeftDirectController();
            _waist = Core.Ins.XRManager.GetTracker();
        }
    }

    public override void StartRecording()
    {
        base.StartRecording();
        ReloadXrDevices();
    }

    public override void StopRecording()
    {
        base.StopRecording();
    }

    public override void LateUpdate()
    {
        if (!_isRecording)
            return;
    }

    public void PredictSlotTest(XRBaseInteractable interactable)
    {
        print("PredictSlotTest Left");
        print("interactable " + interactable);
        print("interactable name " + interactable.name);
    }

    public void PredictSlotTest2(XRBaseInteractable interactable)
    {
        // print("PredictSlotTest Right");
        // print("interactable " + interactable);
        // print("interactable name " + interactable.name);
        // if (interactable.name.Equals("Switch_ActivatePrediction"))
        // {
        //     print("it's our boy prediction");
        // }
    }

    public ENUM_XROS_PeripersonalEquipmentLocations PredictSlot()
    {
//        Dev.Log("Predict Slot!");

        Tensor inputTensor = null;
        switch (slotPrediction)
        {
            case SlotPrediction.QRotRelative:
                inputTensor = CreateTensorUsingQRotForRelative(_head.transform.localPosition, _head.transform.rotation,
                    _head.transform.localPosition - _handR.transform.localPosition, _handR.transform.rotation,
                    _head.transform.localPosition - _handL.transform.localPosition,
                    _handL.transform.rotation, _head.transform.localPosition - _waist.transform.localPosition,
                    _waist.transform.rotation);
                break;
            /*case SlotPrediction.ERotRelative:
                //inputTensor = CreateTensorUsingERotForRelative(_head.transform.localPosition, _head.transform.localEulerAngles, _head.transform.localPosition-_handR.transform.localPosition, _handR.transform.localEulerAngles, _head.transform.localPosition-_handL.transform.localPosition, _handL.transform.localEulerAngles);
                inputTensor = CreateTensorUsingERotForRelative(_head.transform.localPosition,
                    _head.transform.eulerAngles, _head.transform.localPosition - _handR.transform.localPosition,
                    _handR.transform.eulerAngles, _head.transform.localPosition - _handL.transform.localPosition,
                    _handL.transform.eulerAngles);
                break;*/
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (inputTensor != null)
        {
            _worker.Execute(inputTensor);

            var output = _worker.PeekOutput();
            //var outputArray = output.ToReadOnlyArray();
            outputArray = output.ToReadOnlyArray();
//            print("new set");
//            foreach (var i in outputArray)
            {
                //print(i.GetType());
//                print("output value" + i.ToString());
                //8 9 10 11 12 1 2 3, missing 4
            }
            //print(outputArray.ToString());
            //string slotLocation = output.ToReadOnlyArray(); //fix

            //TODO from Powen to Mark: make sure the method GetPredictionAsString returns the proper thing 
            // switch (slotPrediction)
            // {
            //     case SlotPrediction.QRotRelative:
            //         UseQRotPrediction(slotLocation);
            //         break;
            //     case SlotPrediction.ERotRelative:
            //         UseERotPrediction(slotLocation);
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }

            int maxInd = Array.IndexOf(outputArray, outputArray.Max());
            //print("Max Ind:" + maxInd);
            
            //This order is based on the output of Machine Learning 
            switch (maxInd)
            {
                case 0:
                    ModelPredictionString = "_0100";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._0100;
                    break;
                case 1:
                    ModelPredictionString = "_0200";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._0200;
                    break;
                case 2:
                    ModelPredictionString = "_0300";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._0300;
                    break;
                case 3:
                    ModelPredictionString = "_0400";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._0800;
                    break;
                case 4:
                    ModelPredictionString = "_0800";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._0900;
                    break;
                case 5:
                    ModelPredictionString = "_0900";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._1000;
                    break;
                case 6:
                    ModelPredictionString = "_1000";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._1100;
                    break;
                case 7:
                    ModelPredictionString = "_1100";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._1200;
                    break;
                case 8:
                    ModelPredictionString = "_1200";
                    ModelPredictionEnum = ENUM_XROS_PeripersonalEquipmentLocations._1200;
                    break;
            }

//            ModelPredictionString = maxInd.ToString();

            inputTensor.Dispose();
            output.Dispose();
        }

        return ModelPredictionEnum;
    }

    private string ModelPredictionString = "";
    private ENUM_XROS_PeripersonalEquipmentLocations ModelPredictionEnum;

    public string GetPredictionAsString()
    {
        return ModelPredictionString;
    }

    public string NaivePredictionString { get; set; } = "";


    private Tensor CreateTensorUsingQRotForRelative(Vector3 headPos, Quaternion headRotQ, Vector3 handRPos,
        Quaternion handRRotQ,
        Vector3 handLPos, Quaternion handLRotQ, Vector3 waistPos, Quaternion waistRotQ)
    {
        return new Tensor(1, 28, new float[28]
            //return new Tensor(16, 1, new float[16]
            {
                _scalers["headPosx"].Transform(headPos.x), _scalers["headPosy"].Transform(headPos.y),
                _scalers["headPosz"].Transform(headPos.z),
                _scalers["headRotQx"].Transform(headRotQ.x), _scalers["headRotQy"].Transform(headRotQ.y),
                _scalers["headRotQz"].Transform(headRotQ.z), _scalers["headRotQw"].Transform(headRotQ.w),
                _scalers["relativeHandRPosx"].Transform(handRPos.x),
                _scalers["relativeHandRPosy"].Transform(handRPos.y),
                _scalers["relativeHandRPosz"].Transform(handRPos.z),
                _scalers["handRRotQx"].Transform(handRRotQ.x), _scalers["handRRotQy"].Transform(handRRotQ.y),
                _scalers["handRRotQz"].Transform(handRRotQ.z), _scalers["handRRotQw"].Transform(handRRotQ.w),
                _scalers["relativeHandLPosx"].Transform(handLPos.x),
                _scalers["relativeHandLPosy"].Transform(handLPos.y),
                _scalers["relativeHandLPosz"].Transform(handLPos.z),
                _scalers["handLRotQx"].Transform(handLRotQ.x), _scalers["handLRotQy"].Transform(handLRotQ.y),
                _scalers["handLRotQz"].Transform(handLRotQ.z), _scalers["handLRotQw"].Transform(handLRotQ.w),

                _scalers["relativeTracker1Posx"].Transform(waistPos.x),
                _scalers["relativeTracker1Posy"].Transform(waistPos.y),
                _scalers["relativeTracker1Posz"].Transform(waistPos.z),
                _scalers["tracker1RotQx"].Transform(waistRotQ.x), _scalers["tracker1RotQy"].Transform(waistRotQ.y),
                _scalers["tracker1RotQz"].Transform(waistRotQ.z), _scalers["tracker1RotQw"].Transform(waistRotQ.w)
            });
    }
    /*private Tensor CreateTensorUsingERotForRelative(Vector3 headPos, Vector3 headRot, Vector3 handRPos,
        Vector3 handRRot, Vector3 handLPos, Vector3 handLRot)
    {
        return new Tensor(1, 16, new float[18]
            //return new Tensor(16, 1, new float[16]
            {
                _scalers["headPosx"].Transform(headPos.x), _scalers["headPosy"].Transform(headPos.y), _scalers["headPosz"].Transform(headPos.z),
                _scalers["headRotx"].Transform(headRot.x), _scalers["headRoty"].Transform(headRot.y),
                _scalers["headRotz"].Transform(headRot.z),
                _scalers["relativeHandRPosx"].Transform(handRPos.x),
                _scalers["relativeHandRPosy"].Transform(handRPos.y),
                _scalers["relativeHandRPosz"].Transform(handRPos.z),
                _scalers["handRRotx"].Transform(handRRot.x), _scalers["handRRoty"].Transform(handRRot.y),
                _scalers["handRRotz"].Transform(handRRot.z),
                _scalers["relativeHandLPosx"].Transform(handLPos.x),
                _scalers["relativeHandLPosy"].Transform(handLPos.y),
                _scalers["relativeHandLPosz"].Transform(handLPos.z),
                _scalers["handLRotx"].Transform(handLRot.x), _scalers["handLRoty"].Transform(handLRot.y),
                _scalers["handLRotz"].Transform(handLRot.z)
            });
    }*/


    private void UseQRotPrediction(string slotLocation) //Use enum values instead?
    {
        //make poster game object display predicted slot location
        //  this.gameObject.transform.position = newPosition;  

        // var newQuaternion = new Quaternion(_scalers["tracker1RotQx"].InverseTransform(tensorArray[3]),
        //     _scalers["tracker1RotQy"].InverseTransform(tensorArray[4]),
        //     _scalers["tracker1RotQz"].InverseTransform(tensorArray[5]),
        //     _scalers["tracker1RotQw"].InverseTransform(tensorArray[6]));
        // this.gameObject.transform.rotation = newQuaternion;
    }

    private void UseERotPrediction(string slotLocation) //Use enum values instead?
    {
        //make poster game object display predicted slot location
        //this.gameObject.transform.position = newPosition;

        // var newQuaternion = new Quaternion(_scalers["tracker1RotQx"].InverseTransform(tensorArray[3]),
        //     _scalers["tracker1RotQy"].InverseTransform(tensorArray[4]),
        //     _scalers["tracker1RotQz"].InverseTransform(tensorArray[5]),
        //     _scalers["tracker1RotQw"].InverseTransform(tensorArray[6]));
        // this.gameObject.transform.rotation = newQuaternion;
    }

    private LinkedList<Vector3> headPosArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> headRotArray = new LinkedList<Vector3>();
    private LinkedList<Quaternion> headRotArrayQ = new LinkedList<Quaternion>();
    private LinkedList<Vector3> handRPosArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> handRRotArray = new LinkedList<Vector3>();
    private LinkedList<Quaternion> handRRotArrayQ = new LinkedList<Quaternion>();
    private LinkedList<Vector3> handLPosArray = new LinkedList<Vector3>();
    private LinkedList<Vector3> handLRotArray = new LinkedList<Vector3>();
    private LinkedList<Quaternion> handLRotArrayQ = new LinkedList<Quaternion>();

// Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            this._isRecording = true;
        }

        TrackNewData();
    }

    private int stepValue = 1;
    private int historyDataNeeded = 10;

    private int GetTotalEntriesToTrack()
    {
        /*
         * we want 10 entries
        step value of 1
        that means we need to have 20 data points saved
        if we want step value of 2
        we need 10*(2+1);
         */
        return (stepValue + 1) * historyDataNeeded;
    }

//We go from oldest entry to the latest
//so ideally our linkedlist's head is the oldest and the newest is at the end

    private void TrackNewData()
    {
        headPosArray.AddLast(_head.transform.localPosition);
        headRotArray.AddLast(_head.transform.eulerAngles);
        handRPosArray.AddLast(_handR.transform.localPosition);
        handRRotArray.AddLast(_handR.transform.eulerAngles);
        handLPosArray.AddLast(_handL.transform.localPosition);
        handLRotArray.AddLast(_handL.transform.eulerAngles);
        headRotArrayQ.AddLast(_handR.transform.localRotation);
        handRRotArrayQ.AddLast(_head.transform.localRotation);
        handLRotArrayQ.AddLast(_handL.transform.localRotation);


        if (headPosArray.Count > this.GetTotalEntriesToTrack())
        {
            // var node = headPostArray.First;
            // var toPrint = "";
            // while (node != null)
            // {
            //     toPrint += node.Value + " - ";
            //     node = node.Next;
            // }
            //
            // print(toPrint);
            headPosArray.RemoveFirst();
            headRotArray.RemoveFirst();
            headRotArrayQ.RemoveFirst();
            handRPosArray.RemoveFirst();
            handRRotArray.RemoveFirst();
            handRRotArrayQ.RemoveFirst();
            handLPosArray.RemoveFirst();
            handLRotArray.RemoveFirst();
            handLRotArrayQ.RemoveFirst();
        }

        // if (headRotArray.Count > this.GetTotalEntriesToTrack())
        // {
        //     headRotArray.RemoveFirst();
        //     headRotArrayQ.RemoveFirst();
        // }
        // if (handRPostArray.Count > this.GetTotalEntriesToTrack())
        //     handRPostArray.RemoveFirst();
        // if (handRRotArray.Count > this.GetTotalEntriesToTrack())
        // {
        //     handRRotArray.RemoveFirst();
        //     handRRotArrayQ.RemoveFirst();
        // }
        // if (handLPostArray.Count > this.GetTotalEntriesToTrack())
        //     handLPostArray.RemoveFirst();
        // if (handLRotArray.Count > this.GetTotalEntriesToTrack())
        // {
        //     handLRotArray.RemoveFirst();
        //     handLRotArrayQ.RemoveFirst();
        // }
    }

    private void DebugUpdate()
    {
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     StartRecording();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     StopRecording();
        // }
        //
        // //EVENT_NewUser?.Invoke(s);
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     Debug.Log("[Debug] DataCollection: WriteAsCsv");
        // }
        //
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     Debug.Log("[Debug] DataCollection: WriteAsJson");
        // }
    }

    public string GetPredictionTableString()
    {
        return
            "_0100: " + outputArray[0] + "\n" +
            "_0200: " + outputArray[1] + "\n" +
            "_0300: " + outputArray[2] + "\n" +
            "_0800: " + outputArray[3] + "\n" +
            "_0900: " + outputArray[4] + "\n" +
            "_1000: " + outputArray[5] + "\n" +
            "_1100: " + outputArray[6] + "\n" +
            "_1200: " + outputArray[7];
    }

    private float accuracy = 0;
    private int NoOfGestures = 0;
    private int correctGesture = 0;
    public void PredictSlotComparison(string gesture, Vector3 position, Quaternion rotation)
    {
        NoOfGestures++;
        var a = PredictSlot();
        var predictEnum = PredictSlot();
        CreateVisualization(position, rotation, predictEnum);
        var prediction = predictEnum.ToString();
        if (gesture.Equals(prediction))
        {
            correctGesture++;
            accuracy = (float)correctGesture / NoOfGestures;
        }
        print("Actual: " + gesture + " vs Prediction " + prediction + " accuracy " + accuracy);
    }
    
    public GameObject PF_SlotVisualization;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    ///Create a new object and provide a color based on slot location
    public void CreateVisualization(Vector3 position, Quaternion rotation, ENUM_XROS_PeripersonalEquipmentLocations slot)
    {
        var go = GameObject.Instantiate(PF_SlotVisualization, position, rotation);
        //var go = GameObject.Instantiate(PF_SlotVisualization, Core.Ins.XRManager.GetXrCamera().gameObject.transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
        var r = go.GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();
        var c = Color.white;
        //c = new Color(255f, 165f, 0f);
        c = Experiment2_PeripersonalSlotHelper.GetSlotColor(slot);
        block.SetColor(BaseColor, c);
        block.SetColor(EmissionColor, c);
        r.SetPropertyBlock(block);
    }
}