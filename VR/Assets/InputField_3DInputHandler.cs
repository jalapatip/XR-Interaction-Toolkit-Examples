using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputField_3DInputHandler : MonoBehaviour
{
    private InputField _inputField;
    // Start is called before the first frame update
    void Start()
    {
        _inputField = this.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleNewKeyInput(string s)
    {
        int length = _inputField.text.Length;
        if (length >= 18)
        {
            _inputField.text = _inputField.text.Substring(1, length - 1);
        }
        _inputField.text += s;
    }
}
