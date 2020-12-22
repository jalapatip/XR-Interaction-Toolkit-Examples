using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyboardOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    public Button key;
    public Text input;
    void Start()
    {
        key.onClick.AddListener(AddInput);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddInput()
    {
        print("add input triggered");
        var keyText = key.transform.GetChild(0).GetComponent<Text>().text;
        input.text += keyText;
    }
}
