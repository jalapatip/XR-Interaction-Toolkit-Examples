using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Text;
using UnityEngine;
using VRKeyboard.Utils;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Test_Coroutine
{
    public static IEnumerator ServerCommunicate(string json_str, DeviceCollection device_object)
    {
        var test = 0;
        // Send request
        var uwr = new UnityWebRequest("http://localhost:5000/", "POST");
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json_str);
        uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        
        for (int i = 0; i < 1; ++i)
        {
            Debug.Log("test1" + test);
            yield return uwr.SendWebRequest();
            
            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);

                foreach (var shd in device_object.DeviceList)
                {
                    if (shd.GetApplianceType().ToString().Equals("SHD_Oven"))
                    {
                        shd.OpenDevice(true);
                    }
                }

                test += 10;
                Debug.Log("test2" + test);
            }
            // test += 10;
            // Debug.Log("test2"+test);
        }

        //{ 'intent': { 'name': 'open', 'confidence': 0.21507842187459003}, 'entities': [], 'intent_ranking': [{ 'name': 'open', 'confidence': 0.21507842187459003}, { 'name': 'grab', 'confidence': 0.16888505397336773}, { 'name': 'turn_on', 'confidence': 0.13635046052067937}, { 'name': 'affirm', 'confidence': 0.10458936738995539}, { 'name': 'turn_off', 'confidence': 0.09855768581656091}, { 'name': 'turn_light_off', 'confidence': 0.09458055597400178}, { 'name': 'turn_light_on', 'confidence': 0.07759489853224143}, { 'name': 'close', 'confidence': 0.0682651469565307}, { 'name': 'goodbye', 'confidence': 0.03609840896207258}], 'text': 'open the door'}
        //print(jsonResponse);

        //RASAResult info = JsonUtility.FromJson<Result>(jsonResponse).result;}
    }
}