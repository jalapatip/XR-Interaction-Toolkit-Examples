using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VES_PeripersonalSocketManager : MonoBehaviour
{
    public GameObject PF_VES_PeripersonalSocket;

    private List<GameObject> socketList = new List<GameObject>();

    public float distanceBetweenSockets = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //DebugUpdate();
    }

    void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateNewPeripersonalSocket(Vector3.zero, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var xDistance = socketList.Count * distanceBetweenSockets;
            CreateNewPeripersonalSocket(new Vector3(xDistance, 0f, 0f), Quaternion.identity);
        }
    }

    private void LoadFile()
    {
        
    }

    private void SaveFile()
    {
        
    }
    
    public void CreateNewPeripersonalSocket(Vector3 position, Quaternion rotation)
    {
        GameObject go = Instantiate(PF_VES_PeripersonalSocket, Vector3.zero, rotation);
        go.transform.SetParent(this.transform);
        go.transform.position = this.transform.position + position;
        socketList.Add(go);
    }
}
