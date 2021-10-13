using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Debug : MonoBehaviour
{
    Dictionary<KeyCode, Action> debugCodeList = new Dictionary<KeyCode, Action>();

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    public void AddDebugCode(GameObject go, string scriptName, KeyCode kc, Action a)
    {
        if (debugCodeList.ContainsKey(kc))
        {
            Debug.LogError("KeyCode " +kc + " from " +  go + " " + scriptName + " is already in use");
            
        }
        else
        {
            debugCodeList.Add(kc, a);    
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (var keyValuePair in debugCodeList)
        {
            if (Input.GetKeyUp(keyValuePair.Key))
            {
                print(keyValuePair.Key.ToString());
                keyValuePair.Value.Invoke();
            }
        }
    }
}
