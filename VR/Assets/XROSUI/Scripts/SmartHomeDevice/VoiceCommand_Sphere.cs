using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceCommand_Sphere : MonoBehaviour
{
    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        Core.Ins.Microphone.RegisterVoiceCommand("Move North", MoveNorth);
        Core.Ins.Microphone.RegisterVoiceCommand("Move South", MoveSouth);
        Core.Ins.Microphone.RegisterVoiceCommand("Move East", MoveEast);
        Core.Ins.Microphone.RegisterVoiceCommand("Move West", MoveWest);
        Core.Ins.Microphone.RegisterVoiceCommand("Stop", Stop);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            MoveNorth();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            MoveSouth();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            MoveEast();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            MoveWest();
        }
    }


    public void MoveNorth()
    {
        _rigidbody.AddForce(new Vector3(0, 0, 50));
        
    }
    public void MoveSouth()
    {
        _rigidbody.AddForce(new Vector3(0, 0, -50));
        
    }

    public void MoveEast()
    {
        _rigidbody.AddForce(new Vector3(-50, 0, 0));
    }

    public void MoveWest()
    {
        _rigidbody.AddForce(new Vector3(50, 0, 0));
    }
    
    public void Stop()
    {
        //_rigidbody.for
    }
}