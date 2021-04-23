using System;
using System.Collections;
using System.Collections.Generic;
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
    ERotRelative,
    QRotRelative
}

public class DataCollection_Exp2Predict : DataCollection_ExpBase
{
    public SlotPrediction slotPrediction;

    public GameObject _head;
    public GameObject _handR;
    public GameObject _handL;
    public bool startPlaying = false;

    public NNModel modelSource;
    public TextAsset scalerSource;
    private IWorker _worker;
    private Dictionary<string, Scaler> _scalers = new Dictionary<string, Scaler>();

    // Start is called before the first frame update
    private void Start()
    {        
        ExpName = "Exp2Predict";
        ReloadXrDevices();

        var model = ModelLoader.Load(modelSource);
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

        Scalers scalers = JsonUtility.FromJson<Scalers>(scalerSource.text);
        foreach (Scaler scaler in scalers.scalers)
        {
            _scalers.Add(scaler.type, scaler);
        }

        Core.Ins.XRManager.GetLeftRayController().GetComponent<XRRayInteractor>().onSelectEnter.AddListener(PredictSlotTest);
        Core.Ins.XRManager.GetRightRayController().onSelectEnter.AddListener(PredictSlotTest2);
        
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
    }

    public void PredictSlotTest2(XRBaseInteractable interactable)
    {
        print("PredictSlotTest Right");
    }
    
    public void PredictSlot(XRBaseInteractable interactable)
    {
        Tensor inputTensor = null;
        switch (slotPrediction)
        {
            case SlotPrediction.QRotRelative:
                inputTensor = CreateTensorUsingQRotForRelative(_head.transform.localPosition, _head.transform.rotation,
                    _head.transform.localPosition - _handR.transform.position, _handR.transform.rotation, _head.transform.localPosition - _handL.transform.position,
                    _handL.transform.rotation);
                break;
            case SlotPrediction.ERotRelative:
                //inputTensor = CreateTensorUsingERotForRelative(_head.transform.localPosition, _head.transform.localEulerAngles, _head.transform.localPosition-_handR.transform.localPosition, _handR.transform.localEulerAngles, _head.transform.localPosition-_handL.transform.localPosition, _handL.transform.localEulerAngles);
                inputTensor = CreateTensorUsingERotForRelative(_head.transform.localPosition,
                    _head.transform.eulerAngles, _head.transform.localPosition - _handR.transform.localPosition,
                    _handR.transform.eulerAngles, _head.transform.localPosition - _handL.transform.localPosition,
                    _handL.transform.eulerAngles);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (inputTensor != null)
        {
            //Powen: Is there a way to check whether the tensor given to the _worker is of the appropriate size?

            _worker.Execute(inputTensor);

            var output = _worker.PeekOutput();
            var outputArray = output.ToReadOnlyArray();
            print(outputArray.ToString());
            //string slotLocation = output.ToReadOnlyArray(); //fix

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

            inputTensor.Dispose();
            output.Dispose();
        }
    }

    private Tensor CreateTensorUsingQRotForRelative(Vector3 headPos, Quaternion headRotQ, Vector3 handRPos, Quaternion handRRotQ,
        Vector3 handLPos, Quaternion handLRotQ)
    {
        return new Tensor(1, 21, new float[21]
            //return new Tensor(16, 1, new float[16]
            {
                _scalers["headPosx"].Transform(headPos.x), _scalers["headPosy"].Transform(headPos.y), _scalers["headPosz"].Transform(headPos.z),
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
                _scalers["handLRotQz"].Transform(handLRotQ.z), _scalers["handLRotQw"].Transform(handLRotQ.w)
                
            });
    }
    private Tensor CreateTensorUsingERotForRelative(Vector3 headPos, Vector3 headRot, Vector3 handRPos,
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
    }
    

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
}