using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using System.IO;



public class DataReplay_ModelInferenceExp01 : DataCollection_ExpBase
{

    public GameObject _interactionThingy;

    public GameObject _predictedTracker;
    public GameObject _groundTruthTracker;

    public bool _isInferencing = false;
    public string csvSource;
    public string csvDest;
    private int _currentIndex = 0;
    public float mse = 0.0f;
    public NNModel modelSource;
    public TextAsset scalerSource;
    private IWorker _worker;
    private Dictionary<string, Scaler> _scalers = new Dictionary<string, Scaler>();
    private List<DataContainer_Exp0> _dataList = new List<DataContainer_Exp0>();

    // Start is called before the first frame update
    void Start()
    {
        //ReloadXrDevices();

        var model = ModelLoader.Load(modelSource);
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

        Scalers scalers = JsonUtility.FromJson<Scalers>(scalerSource.text);
        foreach (Scaler scaler in scalers.scalers)
        {
            _scalers.Add(scaler.type, scaler);
        }

        var stringList = ReadTextFile(csvSource);
        var parsedList = ParseStringListToDataList(stringList);
        _dataList = CsvListToDataList<DataContainer_Exp0>(parsedList);

    }
    private List<string> ReadTextFile(string path)
    {
        var inp_stm = new StreamReader(path);
        var stringList = new List<string>();
        while (!inp_stm.EndOfStream)
        {
            var inp_ln = inp_stm.ReadLine();

            stringList.Add(inp_ln);
        }

        inp_stm.Close();
        return stringList;
    }

    private List<string[]> ParseStringListToDataList(List<string> stringList)
    {
        var parsedList = new List<string[]>();
        for (var i = 1; i < stringList.Count; i++)
        {
            var temp = stringList[i].Split(',');
            for (var j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim(); //removed the blank spaces
            }

            parsedList.Add(temp);
        }

        return parsedList;
    }

    private List<T> CsvListToDataList<T>(List<string[]> csvList) where T : DataContainer_Base, new()
    {
        //you should now have a list of arrays, ewach array can ba appied to the script that's on the Sprite
        //you'll have to figure out a way to push the data the sprite
        var dataList = new List<T>();
        print("Count: " + csvList.Count);
        for (var i = 0; i < csvList.Count; i++)
        {
            var dataContainer = new T();
            dataContainer.StringToData(csvList[i]);
            dataList.Add(dataContainer);
        }

        return dataList;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            print("Starting model inference");
            this._isInferencing = true;
        }
    }

    public override void LateUpdate()
    {
        if (!_isInferencing)
            return;

        if (_currentIndex < _dataList.Count)
        {
            Tensor inputTensor = null;
            Tensor gtTensor = null;
            inputTensor = CreateTensorForLSTM(_currentIndex);
            gtTensor = CreateGTTensorForLSTM(_currentIndex);

            if (inputTensor != null)
            {
                _worker.Execute(inputTensor);

                var output = _worker.PeekOutput();
                var tensorArray = output.ToReadOnlyArray();
                var gtTensorArray = gtTensor.ToReadOnlyArray();
                UseTensorLSTM(gtTensorArray, tensorArray);
                CalcMSE(gtTensorArray, tensorArray);
                inputTensor.Dispose();
                output.Dispose();
                gtTensor.Dispose();
            }
            _currentIndex++;
            //this._isInferencing = false;
        }
        else
        {
            this._isInferencing = false;
        }
        
        
    }
    private void CalcMSE(float[] _gtTensorArray, float[] _predArray)
    {
        double sum = 0.0;

        for (int i=0; i<7;i++)
        {
            double difference = _gtTensorArray[i] - _predArray[i];
            sum = sum + difference * difference;
        }
        double mse = sum / 7;  //<-- Don't know what x should be!
        this.mse = (float) mse;
        Console.WriteLine("The mean square error is {0}", mse);
    }
    private Tensor CreateGTTensorForLSTM(int idx)
    {
        var _array = new float[10];

        int i = 0;
        var relativeTrackerPos = _dataList[_currentIndex].headPos - _dataList[_currentIndex].tracker1Pos;
        _array[i++] = _scalers["relativeTracker1Posx"].Transform(relativeTrackerPos.x);
        _array[i++] = _scalers["relativeTracker1Posy"].Transform(relativeTrackerPos.y);
        _array[i++] = _scalers["relativeTracker1Posz"].Transform(relativeTrackerPos.z);

        _array[i++] = _scalers["tracker1RotQx"].Transform(_dataList[idx].tracker1RotQ.x);
        _array[i++] = _scalers["tracker1RotQy"].Transform(_dataList[idx].tracker1RotQ.y);
        _array[i++] = _scalers["tracker1RotQz"].Transform(_dataList[idx].tracker1RotQ.z);
        _array[i++] = _scalers["tracker1RotQw"].Transform(_dataList[idx].tracker1RotQ.w);

        _array[i++] = _dataList[idx].headPos.x;
        _array[i++] = _dataList[idx].headPos.y;
        _array[i++] = _dataList[idx].headPos.z;

        return new Tensor(1, 1, 1, 10, _array);
    }
    private Tensor CreateTensorForLSTM(int idx)
    {
        var _array = new float[10 * 19];
        int i = 0;
        if (idx <10)
        {
            for (int jdx = 0; jdx < 10; jdx++) {_array[i++] = _scalers["headPosy"].Transform(_dataList[idx].headPos.y);}
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["headRotQx"].Transform(_dataList[idx].headRotQ.x); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["headRotQy"].Transform(_dataList[idx].headRotQ.y); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["headRotQz"].Transform(_dataList[idx].headRotQ.z); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["headRotQw"].Transform(_dataList[idx].headRotQ.w); }
            var relativeHandRPos = _dataList[idx].headPos - _dataList[idx].handRPos;
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["relativeHandRPosx"].Transform(relativeHandRPos.x); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["relativeHandRPosy"].Transform(relativeHandRPos.y); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["relativeHandRPosz"].Transform(relativeHandRPos.z); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["handRRotQx"].Transform(_dataList[idx].handRRotQ.x); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["handRRotQy"].Transform(_dataList[idx].handRRotQ.y); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["handRRotQz"].Transform(_dataList[idx].handRRotQ.z); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["handRRotQw"].Transform(_dataList[idx].handRRotQ.w); }
            var relativeHandLPos = _dataList[idx].headPos - _dataList[idx].handLPos;
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["relativeHandLPosx"].Transform(relativeHandLPos.x); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["relativeHandLPosy"].Transform(relativeHandLPos.y); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["relativeHandLPosz"].Transform(relativeHandLPos.z); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["handLRotQx"].Transform(_dataList[idx].handLRotQ.x); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["handLRotQy"].Transform(_dataList[idx].handLRotQ.y); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["handLRotQz"].Transform(_dataList[idx].handLRotQ.z); }
            for (int jdx = 0; jdx < 10; jdx++) { _array[i++] = _scalers["handLRotQw"].Transform(_dataList[idx].handLRotQ.w); }
            /*
            for (int jdx = 0; jdx < 10; jdx++)
            {
                _array[i++] = _scalers["headPosy"].Transform(_dataList[idx].headPos.y);
                _array[i++] = _scalers["headRotQx"].Transform(_dataList[idx].headRotQ.x);
                _array[i++] = _scalers["headRotQy"].Transform(_dataList[idx].headRotQ.y);
                _array[i++] = _scalers["headRotQz"].Transform(_dataList[idx].headRotQ.z);
                _array[i++] = _scalers["headRotQw"].Transform(_dataList[idx].headRotQ.w);
                var relativeHandRPos = _dataList[idx].headPos - _dataList[idx].handRPos;
                _array[i++] = _scalers["relativeHandRPosx"].Transform(relativeHandRPos.x);
                _array[i++] = _scalers["relativeHandRPosy"].Transform(relativeHandRPos.y);
                _array[i++] = _scalers["relativeHandRPosz"].Transform(relativeHandRPos.z);
                _array[i++] = _scalers["handRRotQx"].Transform(_dataList[idx].handRRotQ.x);
                _array[i++] = _scalers["handRRotQy"].Transform(_dataList[idx].handRRotQ.y);
                _array[i++] = _scalers["handRRotQz"].Transform(_dataList[idx].handRRotQ.z);
                _array[i++] = _scalers["handRRotQw"].Transform(_dataList[idx].handRRotQ.w);
                var relativeHandLPos = _dataList[idx].headPos - _dataList[idx].handLPos;
                _array[i++] = _scalers["relativeHandLPosx"].Transform(relativeHandLPos.x);
                _array[i++] = _scalers["relativeHandLPosy"].Transform(relativeHandLPos.y);
                _array[i++] = _scalers["relativeHandLPosz"].Transform(relativeHandLPos.z);
                _array[i++] = _scalers["handLRotQx"].Transform(_dataList[idx].handLRotQ.x);
                _array[i++] = _scalers["handLRotQy"].Transform(_dataList[idx].handLRotQ.y);
                _array[i++] = _scalers["handLRotQz"].Transform(_dataList[idx].handLRotQ.z);
                _array[i++] = _scalers["handLRotQw"].Transform(_dataList[idx].handLRotQ.w);
            }*/
        }
        else
        {
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["headPosy"].Transform(_dataList[idx-jdx].headPos.y); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["headRotQx"].Transform(_dataList[idx - jdx].headRotQ.x); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["headRotQy"].Transform(_dataList[idx - jdx].headRotQ.y); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["headRotQz"].Transform(_dataList[idx - jdx].headRotQ.z); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["headRotQw"].Transform(_dataList[idx - jdx].headRotQ.w); }
            var relativeHandRPos = _dataList[idx ].headPos - _dataList[idx].handRPos;
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["relativeHandRPosx"].Transform(relativeHandRPos.x); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["relativeHandRPosy"].Transform(relativeHandRPos.y); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["relativeHandRPosz"].Transform(relativeHandRPos.z); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["handRRotQx"].Transform(_dataList[idx - jdx].handRRotQ.x); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["handRRotQy"].Transform(_dataList[idx - jdx].handRRotQ.y); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["handRRotQz"].Transform(_dataList[idx - jdx].handRRotQ.z); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["handRRotQw"].Transform(_dataList[idx - jdx].handRRotQ.w); }
            var relativeHandLPos = _dataList[idx].headPos - _dataList[idx].handLPos;
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["relativeHandLPosx"].Transform(relativeHandLPos.x); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["relativeHandLPosy"].Transform(relativeHandLPos.y); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["relativeHandLPosz"].Transform(relativeHandLPos.z); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["handLRotQx"].Transform(_dataList[idx - jdx].handLRotQ.x); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["handLRotQy"].Transform(_dataList[idx - jdx].handLRotQ.y); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["handLRotQz"].Transform(_dataList[idx - jdx].handLRotQ.z); }
            for (int jdx = 9; jdx >= 0; jdx--) { _array[i++] = _scalers["handLRotQw"].Transform(_dataList[idx - jdx].handLRotQ.w); }

            /*for (int jdx = 9; jdx >= 0; jdx--)
            {
                _array[i++] = _scalers["headPosy"].Transform(_dataList[idx-jdx].headPos.y);
                _array[i++] = _scalers["headRotQx"].Transform(_dataList[idx-jdx].headRotQ.x);
                _array[i++] = _scalers["headRotQy"].Transform(_dataList[idx-jdx].headRotQ.y);
                _array[i++] = _scalers["headRotQz"].Transform(_dataList[idx - jdx].headRotQ.z);
                _array[i++] = _scalers["headRotQw"].Transform(_dataList[idx - jdx].headRotQ.w);
                var relativeHandRPos = _dataList[idx - jdx].headPos - _dataList[idx - jdx].handRPos;
                _array[i++] = _scalers["relativeHandRPosx"].Transform(relativeHandRPos.x);
                _array[i++] = _scalers["relativeHandRPosy"].Transform(relativeHandRPos.y);
                _array[i++] = _scalers["relativeHandRPosz"].Transform(relativeHandRPos.z);
                _array[i++] = _scalers["handRRotQx"].Transform(_dataList[idx - jdx].handRRotQ.x);
                _array[i++] = _scalers["handRRotQy"].Transform(_dataList[idx - jdx].handRRotQ.y);
                _array[i++] = _scalers["handRRotQz"].Transform(_dataList[idx - jdx].handRRotQ.z);
                _array[i++] = _scalers["handRRotQw"].Transform(_dataList[idx - jdx].handRRotQ.w);
                var relativeHandLPos = _dataList[idx - jdx].headPos - _dataList[idx - jdx].handLPos;
                _array[i++] = _scalers["relativeHandLPosx"].Transform(relativeHandLPos.x);
                _array[i++] = _scalers["relativeHandLPosy"].Transform(relativeHandLPos.y);
                _array[i++] = _scalers["relativeHandLPosz"].Transform(relativeHandLPos.z);
                _array[i++] = _scalers["handLRotQx"].Transform(_dataList[idx - jdx].handLRotQ.x);
                _array[i++] = _scalers["handLRotQy"].Transform(_dataList[idx - jdx].handLRotQ.y);
                _array[i++] = _scalers["handLRotQz"].Transform(_dataList[idx - jdx].handLRotQ.z);
                _array[i++] = _scalers["handLRotQw"].Transform(_dataList[idx - jdx].handLRotQ.w);
            }*/
        }
        
        /*print(_array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' +
            _array[0].ToString() + ',' );*/
        
        return new Tensor(1, 1, 19, 10, _array);
    }
    private void UseTensorLSTM(float[] gtTensorArray, float[] tensorArray)
    {
        var gtPosition = new Vector3(_scalers["relativeTracker1Posx"].InverseTransform(gtTensorArray[0]),
            _scalers["relativeTracker1Posy"].InverseTransform(gtTensorArray[1]),
            _scalers["relativeTracker1Posz"].InverseTransform(gtTensorArray[2]));
        var headPos = new Vector3(gtTensorArray[7], gtTensorArray[8], gtTensorArray[9]);
        this._groundTruthTracker.transform.localPosition = headPos - gtPosition;

        var gtRotation = new Quaternion(_scalers["tracker1RotQx"].InverseTransform(gtTensorArray[3]),
            _scalers["tracker1RotQy"].InverseTransform(gtTensorArray[4]),
            _scalers["tracker1RotQz"].InverseTransform(gtTensorArray[5]),
            _scalers["tracker1RotQw"].InverseTransform(gtTensorArray[6]));

        this._groundTruthTracker.transform.eulerAngles = gtRotation.eulerAngles;


        var newPosition = new Vector3(_scalers["relativeTracker1Posx"].InverseTransform(tensorArray[0]),
            _scalers["relativeTracker1Posy"].InverseTransform(tensorArray[1]),
            _scalers["relativeTracker1Posz"].InverseTransform(tensorArray[2]));
        this._predictedTracker.transform.localPosition = headPos - newPosition;
        

        var newRotation = new Quaternion(_scalers["tracker1RotQx"].InverseTransform(tensorArray[3]),
            _scalers["tracker1RotQy"].InverseTransform(tensorArray[4]),
            _scalers["tracker1RotQz"].InverseTransform(tensorArray[5]),
            _scalers["tracker1RotQw"].InverseTransform(tensorArray[6]));
        this._predictedTracker.transform.eulerAngles = newRotation.eulerAngles;
    }
}
