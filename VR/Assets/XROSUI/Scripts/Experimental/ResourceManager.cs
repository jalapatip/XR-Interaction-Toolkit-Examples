using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Test script for saving and loading data
/// </summary>
public class ResourceManager : MonoBehaviour
{
    PlayerData _playerData = new PlayerData();

    private string _currentString;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DebugUpdate();
    }

    //Track Debug Inputs here
    //https://docs.google.com/spreadsheets/d/1NMH43LMlbs5lggdhq4Pa4qQ569U1lr_O7HSHESEantU/edit#gid=0
    private void DebugUpdate()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            _currentString = JsonUtility.ToJson(_playerData);
            Dev.Log(_currentString);
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            _playerData.hp = _playerData.hp++;
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            //string jsonString;
            _playerData = JsonUtility.FromJson<PlayerData>(_currentString);
            Dev.Log(_currentString);
        }
    }
}

//https://medium.com/@prasetion/saving-data-as-json-in-unity-4419042d1334
public class PotionSaveData : MonoBehaviour
{
    [SerializeField] private PotionData potionData = new PotionData();

    public void SaveIntoJSON()
    {
        string potion = JsonUtility.ToJson((potionData));
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PotionData.json", potion);
    }
}

[System.Serializable]
internal class PlayerData
{
    public string playerName = "British Louise";
    public string title = "Ambassador";
    public float hp = 10f;
}

[System.Serializable]
public class PotionData
{
    public string potionName;
    public int value;
    public List<Effect> effect = new List<Effect>();
}

[System.Serializable]
public class Effect
{
    public string name;
    public string desc;
}