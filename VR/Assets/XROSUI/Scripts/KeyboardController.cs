using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //create public inputfield 

public delegate void Delegate_NewKeyInput(string name);

public class KeyboardController : MonoBehaviour
{
    public static event Delegate_NewKeyInput EVENT_NewKeyInput;
    public bool isHovering = false;
    bool isWaiting;
    // Start is called before the first frame update
    void Start()
    {
        isWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterInput(string s)
    {
        EVENT_NewKeyInput?.Invoke(s);
    }

    public void Wait()
    {
        isWaiting = true;
    }
    public bool GetWaiting()
    {
        return isWaiting;
    }
    public void SetWaiting()
    {
        StartCoroutine("WaitAndPrint");
    }
    IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(0.2f);
        isWaiting = !isWaiting;
    }
}

