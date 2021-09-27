using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_MicrophoneGuiMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private AudioClip ac;
    
    private int debugButtonStartX = 10+100;
    private int debugButtonStartY = 10;
    private int debugButtonWidth = 200;
    private int debugButtonHeight = 50;
    void OnGUI()
    {
        int i = 0;
        if (GUI.Button(new Rect(debugButtonStartX, debugButtonStartY, debugButtonWidth, debugButtonHeight), "Start Recording"))
        {
            ac = Core.Ins.Microphone.StartRecording();
            i++;
        }

        if (GUI.Button(new Rect(debugButtonStartX, 110, debugButtonWidth, debugButtonHeight), "Stop Recording"))
        {
            Core.Ins.Microphone.EndRecording();
            i++;
        }
        if (GUI.Button(new Rect(debugButtonStartX, 210, debugButtonWidth, debugButtonHeight), "Save Recording"))
        {
            Core.Ins.Microphone.SaveRecording(ac);
            i++;
        }
    }
}
