using UnityEngine;
using UnityEngine.UI; //create public inputfield 
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using System;
using KeyboardPosition;


public class SeparateKeyboardCharacterCreator : KeyboardController
{
    int KEYS_NUMBER = 32; //how many keys are in the keyboard
    float mirrorRealDifference = 0.4f; // mirror keyboard and real keyboard height difference
    public GameObject controllerPF;
    private GameObject _leftController; // left real controller
    private GameObject _rightController; // right real controller
    private GameObject _mirrorControllerLeft; // left mirror controller
    private GameObject _mirrorControllerRight; // right mirror controller
    XRGrabInteractable m_InteractableBase;
    public GameObject system;
    public int segments = 10;
    public float xradius = 0.01f;
    public float yradius = 0.01f;
    public float smallerXradius;

    public float smallerYradius;

    //Prefab for a 3D key
    public GameObject PF_Key;
    public Button Button_Timer;
    public bool active = false;
    List<GameObject> points = new List<GameObject>(); // including real/lower keys and mirror/upper keys
    private KeyboardWrapper kw = new KeyboardWrapper();

    private Vector3 keyboardModelPosition; // only include real/lower keys but not mirror/upper keys
    // Update is called once per frame

    private string filename = "JSON/positions.JSON";

    private void Start()
    {
        _leftController = Core.Ins.XRManager.GetLeftDirectController();
        _rightController = Core.Ins.XRManager.GetRightDirectControllerGO();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            EmptyPositions();
        }

        if (_mirrorControllerLeft)
        {
            _mirrorControllerLeft.transform.position =
                _leftController.transform.position + new Vector3(0f, mirrorRealDifference, 0);
            _mirrorControllerLeft.transform.rotation = _leftController.transform.rotation;
        }

        if (_mirrorControllerRight)
        {
            _mirrorControllerRight.transform.position =
                _rightController.transform.position + new Vector3(0f, mirrorRealDifference, 0);
            _mirrorControllerRight.transform.rotation = _rightController.transform.rotation;
        }
    }

    public void CreateMirrorKeyboard(float startingX, float startingY, float startingZ)
    {
        bool empty = ReadKeyPositions();
        if (empty)
        {
            CreateDefaultPoints(startingX, startingY, startingZ);
        }
        else
        {
            CreateCustomPoints(startingX, startingY, startingZ);
            kw.keys = kw.keys.GetRange(32, 32);
        }

        // creating the mirror keyboard on top
        MirrorKeys(startingX, startingY + mirrorRealDifference, startingZ);
        // create mirrored controllers
        SetUpMirrorControllers(startingX, startingY, startingZ);
    }

    //float topRowOffsetX = -0.15f;
    //float middleRowOffsetX = -0.15f;
    //float bottomRowOffsetX = -0.15f;
    float topRowOffsetY = 0.06f;
    float middleRowOffsetY = 0f;
    float bottomRowOffsetY = -0.06f;
    float spaceKeyY = -0.12f; //-0.18

    public void CreateDefaultPoints(float startingX, float startingY, float startingZ)
    {
        keyboardModelPosition = new Vector3(startingX, startingY, startingZ);
        // delete
        GameObject go = CreateKey(-0.15f + startingX, 0.14f + startingY, 0.05f + startingZ, "DEL");
        Vector3 scale = go.transform.localScale;
        scale.x = 2 * scale.x;
        go.transform.localScale = scale;

        go = CreateKey(0.15f + startingX, 0.14f + startingY, 0.05f + startingZ, "DEL");
        scale = go.transform.localScale;
        scale.x = 2 * scale.x;
        go.transform.localScale = scale;

        CreateLine(-0.15f + startingX, topRowOffsetY + startingY, -0.05f + startingZ, -10f, smallerXradius,
            smallerYradius, "qwert");
        CreateLine(0.15f + startingX, topRowOffsetY + startingY, -0.05f + startingZ, 10f, smallerXradius,
            smallerYradius, "yuiop");
        CreateLine(-0.15f + startingX, middleRowOffsetY + startingY, startingZ, -10f, xradius, yradius, "asdfg");
        CreateLine(0.15f + startingX, middleRowOffsetY + startingY, startingZ, +10f, xradius, yradius, "hjkl;");
        CreateLine(-0.15f + startingX, bottomRowOffsetY + startingY, -0.05f + startingZ, -10f, smallerXradius,
            smallerYradius, "zxcv");
        CreateLine(0.15f + startingX, bottomRowOffsetY + startingY, -0.05f + startingZ, 10f, smallerXradius,
            smallerYradius, "bnm,");

        // space
        go = CreateKey(-0.15f + startingX, spaceKeyY + startingY, 0.05f + startingZ, " ");
        scale = go.transform.localScale;
        scale.x = 2 * scale.x;
        go.transform.localScale = scale;

        //go = CreateKey(0.15f + startingX, spaceKeyY + startingY, 0.05f + startingZ, " ");
        //scale = go.transform.localScale;
        //scale.x = 2 * scale.x;
        //go.transform.localScale = scale;

        //start
        //x is left
        go = CreateKey(startingX, -0.18f + startingY, 0.05f + startingZ, "start");
        scale = go.transform.localScale;
        scale.x = 2 * scale.x;
        go.transform.localScale = scale;
    }

    public void CreateCustomPoints(float startingX, float startingY, float startingZ)
    {
        keyboardModelPosition = new Vector3(startingX, startingY, startingZ);
        for (int i = 0; i < KEYS_NUMBER; i++)
        {
            KeyWrapper key = kw.keys[i];
            GameObject go = CreateKey(key.x + startingX, key.y + startingY, key.z + startingZ, key.text);
        }
    }

    void CreateLine(float offsetX, float offsetY, float offsetZ, float angleOffset, float xradius, float yradius,
        string letters)
    {
        float x;
        float z;

        float angle = 300f - letters.Length * 2f + angleOffset;
        for (int i = 0; i < (letters.Length); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;
            CreateKey(x + offsetX, offsetY, z + offsetZ, "" + letters[i]);
            angle += (180f / (letters.Length)) % 360;
        }
    }

    GameObject CreateKey(float x, float y, float z, string s, bool mirror = false)
    {
        GameObject go = Instantiate(PF_Key, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0));
        ;
        go.transform.SetParent(this.transform);
        XRKey key = go.GetComponent<XRKey>();
        points.Add(go);
        KeyWrapper keywrappper = new KeyWrapper(s, x - keyboardModelPosition.x, y - keyboardModelPosition.y,
            z - keyboardModelPosition.z);
        key.Setup(s, this, Button_Timer, keywrappper);
        if (mirror == false) // no need to store mirror keys
            kw.keys.Add(keywrappper);
        return go;
    }

    // remove the whole keyboard
    public void DestroyPoints()
    {
        foreach (GameObject point in points)
        {
            if (point != null)
                Destroy(point);
        }

        points.Clear();
        kw.keys.Clear();
        DestroyMirrorControllers();
    }


    void MirrorKeys(float startingX, float startingY, float startingZ)
    {
        for (int i = 0; i < KEYS_NUMBER; i++)
        {
            KeyWrapper key = kw.keys[i];
            GameObject go = CreateKey(key.x + startingX, key.y + startingY, key.z + startingZ, key.text, true);
            points[i].GetComponent<XRKey>().AssignMirroredKey(go);
        }
    }

    public void EmptyPositions()
    {
        print("triggered");
        string filename = "JSON/positions.JSON";
        StreamWriter writer = new StreamWriter(filename, false);

        try
        {
            writer.Write("");
        }
        catch (Exception exp)
        {
            print(exp.Message);
        }
        finally
        {
            writer.Close();
        }
    }

    public void SaveKeyPositions()
    {
        string json;
        kw.keyboardName = "lower";
        json = JsonUtility.ToJson(kw);
        StreamWriter writer = new StreamWriter(filename, false);

        try
        {
            writer.Write(json);
        }
        catch (Exception exp)
        {
            print(exp.Message);
        }
        finally
        {
            writer.Close();
        }
    }

    public bool ReadKeyPositions()
    {
        TextAsset json;
        try
        {
            json = Resources.Load(filename) as TextAsset;
            var jsonString = json.text;
            kw = JsonUtility.FromJson<KeyboardWrapper>(jsonString);
        }
        catch (Exception exp)
        {
            print(exp.Message);
        }

        if (kw?.keys == null || kw.keys.Count == 0)
        {
            kw = new KeyboardWrapper();
            return true; // positions.JSON is empty or malformed
        }

        return false; // positions.JSON is not empty
    }

    private void SetUpMirrorControllers(float x, float y, float z)
    {
        _mirrorControllerLeft = Instantiate(controllerPF, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0));
        _mirrorControllerLeft.name = "mirrorControllerLeft";
        _mirrorControllerRight = Instantiate(controllerPF, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0));
        _mirrorControllerRight.name = "mirrorControllerRight";
    }

    private void DestroyMirrorControllers()
    {
        if (_mirrorControllerLeft != null)
            Destroy(_mirrorControllerLeft);
        if (_mirrorControllerRight != null)
            Destroy(_mirrorControllerRight);
    }
}