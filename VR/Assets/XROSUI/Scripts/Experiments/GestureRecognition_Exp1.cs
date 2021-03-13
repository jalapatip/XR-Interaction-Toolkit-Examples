using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GestureRecognition_Exp1 : DataCollection_ExpBase
{
    // To be set in Unity to determine which gesture we are currently collecting data for
    public XRGrabInteractable grabInteractable;
    
    // How often to sample the position for a gesture/movement
    private static double _timestepSec = 0.1;
    // How many position samples to include in a gesture/movementen
    public static int samplesPerGesture = 10;

    // Running queue of last _samples_per_gesture positions
    private Queue<DataContainer_ExpGesturesPosition> _lastPositions = new Queue<DataContainer_ExpGesturesPosition>();
    // Lock to make sure the lastPositions queue is safe to edit or read
    private readonly object _lastPositionsLock = new object();
    
    private GameObject _head;
    private GameObject _handR;
    private GameObject _handL;

    private float _lastUpdateTime;

    private bool startedGesture = false;

    public NNModel modelSource;
    public TextAsset scalerSource;
    public TextAsset labelSource;
    private IWorker _worker;
    private Dictionary<string, Scaler> _scalers = new Dictionary<string, Scaler>();
    private Dictionary<int, Gesture>_labelDictionary = new Dictionary<int, Gesture>();
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        ExpName = "GestureRecognition";
        grabInteractable.onSelectEnter.AddListener(StartGesture);
        grabInteractable.onSelectExit.AddListener(EndGesture);
        ReloadXrDevices();
        
        var model = ModelLoader.Load(modelSource);
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

        Scalers scalers = JsonUtility.FromJson<Scalers>(scalerSource.text);
        foreach (Scaler scaler in scalers.scalers)
        {
            _scalers.Add(scaler.type, scaler);
        }
        
        GestureKeys gestureKeys = JsonUtility.FromJson<GestureKeys>(labelSource.text);
        foreach (GestureKey gestureKey in gestureKeys.gestureKeys)
        {
            _labelDictionary.Add(int.Parse(gestureKey.key), gestureKey.GetGesture());
        }
    }

    private void OnDisable()
    {
        grabInteractable.onSelectEnter.RemoveListener(StartGesture);
        grabInteractable.onSelectExit.RemoveListener(EndGesture);
    }

    private void StartGesture(XRBaseInteractor arg0)
    {
        startedGesture = true;
    }

    private void ReloadXrDevices()
    {
        Dev.Log("Reload Xr Devices");
        _head = Core.Ins.XRManager.GetXrCamera().gameObject;
        _handR = Core.Ins.XRManager.GetRightDirectControllerGO();
        _handL = Core.Ins.XRManager.GetLeftDirectController();
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
        lock (_lastPositionsLock)
        {
            if (_lastPositions.Count == 0 || Time.time - _lastUpdateTime >= _timestepSec)
            {
                var data = new DataContainer_ExpGesturesPosition
                {
                    headPos = _head.transform.localPosition,
                    headRot = _head.transform.eulerAngles,
                    headRotQ = _head.transform.rotation,
                    handRPos = _handR.transform.localPosition,
                    handRRot = _handR.transform.eulerAngles,
                    handRRotQ = _handR.transform.rotation,
                    handLPos = _handL.transform.localPosition,
                    handLRot = _handL.transform.eulerAngles,
                    handLRotQ = _handL.transform.rotation
                };
                _lastPositions.Enqueue(data);
                _lastUpdateTime = Time.time;
                if (_lastPositions.Count > samplesPerGesture)
                {
                    _lastPositions.Dequeue();
                }
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    public void EndGesture(XRBaseInteractor xrBaseInteractor)
    {
        if (!startedGesture)
            return;

        // Make sure we have control of the lastPositions queue
        lock (_lastPositionsLock)
        {
            // Make sure there are the proper number of position samples to make a gesture
            if (_lastPositions.Count != samplesPerGesture)
                return;

            Tensor inputTensor = CreateTensor();
            _worker.Execute(inputTensor);
            var output = _worker.PeekOutput();
            var labelScoreArray = output.ToReadOnlyArray();
            
            // need to take the argmax of the array
            print(labelScoreArray);
            var labelKey = labelScoreArray.ToList().IndexOf(labelScoreArray.Max());
            print("Your gesture key was " + labelKey);
            var gesture = _labelDictionary[labelKey];
            print("Your gesture was " + gesture);
            
            // Clean up
            inputTensor.Dispose();
            output.Dispose();
        }

        startedGesture = false;
    }

    private Tensor CreateTensor()
    {
        // relativeHandRPos + relativeHandLPos + headRot + handRRot + handLRot
        float[] relativeHandRPos = new float[samplesPerGesture * 3];
        float[] relativeHandLPos = new float[samplesPerGesture * 3];
        float[] headRot = new float[samplesPerGesture * 3];
        float[] handRRot = new float[samplesPerGesture * 3];
        float[] handLRot = new float[samplesPerGesture * 3];
        var i = 0;
        foreach (var position in _lastPositions)
        {
            relativeHandRPos[i * 3] = _scalers["relativeHandRPosx" + i.ToString()].Transform(position.headPos.x - position.handRPos.x);
            relativeHandRPos[i*3 + 1] = _scalers["relativeHandRPosy" + i.ToString()].Transform(position.headPos.y - position.handRPos.y);
            relativeHandRPos[i*3 + 2] = _scalers["relativeHandRPosz" + i.ToString()].Transform(position.headPos.z - position.handRPos.z);
            relativeHandLPos[i * 3] = _scalers["relativeHandLPosx" + i.ToString()].Transform(position.headPos.x - position.handLPos.x);
            relativeHandLPos[i*3 + 1] = _scalers["relativeHandLPosy" + i.ToString()].Transform(position.headPos.y - position.handLPos.y);
            relativeHandLPos[i*3 + 2] = _scalers["relativeHandLPosz" + i.ToString()].Transform(position.headPos.z - position.handLPos.z);
            headRot[i * 3] = _scalers["headRotx" + i.ToString()].Transform(position.headRot.x);
            headRot[i*3 + 1] = _scalers["headRoty" + i.ToString()].Transform(position.headRot.y);
            headRot[i*3 + 2] = _scalers["headRotz" + i.ToString()].Transform(position.headRot.z);
            handRRot[i * 3] = _scalers["handRRotx" + i.ToString()].Transform(position.handRRot.x);
            handRRot[i*3 + 1] = _scalers["handRRoty" + i.ToString()].Transform(position.handRRot.y);
            handRRot[i*3 + 2] = _scalers["handRRotz" + i.ToString()].Transform(position.handRRot.z);
            handLRot[i * 3] = _scalers["handLRotx" + i.ToString()].Transform(position.handLRot.x);
            handLRot[i*3 + 1] = _scalers["handLRoty" + i.ToString()].Transform(position.handLRot.y);
            handLRot[i*3 + 2] = _scalers["handLRotz" + i.ToString()].Transform(position.handLRot.z);
            i++;
        }

        float[] finalData = new float[samplesPerGesture * 3 * 5];
        relativeHandRPos.CopyTo(finalData, 0);
        relativeHandLPos.CopyTo(finalData, samplesPerGesture * 3);
        headRot.CopyTo(finalData, samplesPerGesture * 3 * 2);
        handRRot.CopyTo(finalData, samplesPerGesture * 3 * 3);
        handLRot.CopyTo(finalData, samplesPerGesture * 3 * 4);
        
        return new Tensor(1, samplesPerGesture * 3 * 5, finalData);
    }
}
