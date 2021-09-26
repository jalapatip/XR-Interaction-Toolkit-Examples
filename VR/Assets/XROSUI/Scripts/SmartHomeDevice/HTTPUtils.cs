using System;
using System.Net;
using System.IO;

public class HTTPUtils
{
    public static string ServerCommunicate(string json_str)
    {
        // Send request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://localhost:5000/"));
        request.ContentType = "application/json";
        request.Method = "POST";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(json_str);
        }

        // Get response
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();


        return jsonResponse;

        //{ 'intent': { 'name': 'open', 'confidence': 0.21507842187459003}, 'entities': [], 'intent_ranking': [{ 'name': 'open', 'confidence': 0.21507842187459003}, { 'name': 'grab', 'confidence': 0.16888505397336773}, { 'name': 'turn_on', 'confidence': 0.13635046052067937}, { 'name': 'affirm', 'confidence': 0.10458936738995539}, { 'name': 'turn_off', 'confidence': 0.09855768581656091}, { 'name': 'turn_light_off', 'confidence': 0.09458055597400178}, { 'name': 'turn_light_on', 'confidence': 0.07759489853224143}, { 'name': 'close', 'confidence': 0.0682651469565307}, { 'name': 'goodbye', 'confidence': 0.03609840896207258}], 'text': 'open the door'}
        //print(jsonResponse);

        //RASAResult info = JsonUtility.FromJson<Result>(jsonResponse).result;}
    }
}
