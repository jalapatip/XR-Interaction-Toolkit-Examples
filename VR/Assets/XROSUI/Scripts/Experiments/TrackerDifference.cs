using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TrackerDifference : MonoBehaviour
{
    //public GameObject actualTracker;
    public Transform actualTrackerTransform;

    public List<Transform> predictedTrackers = new List<Transform>();

    [SerializeField]
    private List<float> accumulatedDistances;
    
    [SerializeField]
    private List<float> avgDistances;
    
    [SerializeField]
    private List<float> maxDistanceDiff;
    
    [SerializeField]
    private List<float> minDistanceDiff;
    
    [SerializeField]
    private int frameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        accumulatedDistances = new List<float>();
        avgDistances = new List<float>();
        maxDistanceDiff = new List<float>();
        minDistanceDiff = new List<float>();
        
        for (int i = 0; i < predictedTrackers.Count; i++)
        {
            accumulatedDistances.Add(0);
            avgDistances.Add(0);
            maxDistanceDiff.Add(0);
            minDistanceDiff.Add(float.MaxValue);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //CalculateDifference();
        if (DataReplayManager.Ins != null && frameCount > DataReplayManager.Ins.GetMaxIndex())
        {
            return;
        }
        
        frameCount++;
        for (int i = 0; i < predictedTrackers.Count; i++)
        {
            var difference =Vector3.Distance(actualTrackerTransform.localPosition, predictedTrackers[i].localPosition);
            accumulatedDistances[i] += difference;
                
            avgDistances[i] = accumulatedDistances[i] / frameCount;
            
            if (difference > maxDistanceDiff[i])
            {
                maxDistanceDiff[i] = difference;
            }
            if (difference < minDistanceDiff[i])
            {
                minDistanceDiff[i] = difference;
            }
        }
    }

    private void CalculateDifference()
    {
        if (!actualTrackerTransform) return;
        
        foreach (var t in predictedTrackers)
        {
            if (t.gameObject.activeSelf)
            {
                //IMPORTANT
                //If we print a vector, it won't print all the fine details, but will be rounded up
                var newP = (actualTrackerTransform.localPosition - t.localPosition);
                var newR = (actualTrackerTransform.localEulerAngles - t.localEulerAngles);
//                    print(t.name + "," + VectorFormatter(newP) + "," + RotationFormatter(newR));
            }
            else
            {
                //print(t.name+ " not active");
            }
        }
    }

    
    public static string VectorFormatter(Vector3 v)
    {
        return v.x + "," + v.y + "," + v.z;
    }
    public static string RotationFormatter(Vector3 v)
    {
        var v2 = Vector3.zero;
        v2.x = v.x > 180 ? 360 - v.x : v.x;
        v2.y = v.y > 180 ? 360 - v.y : v.y;
        v2.z = v.z > 180 ? 360 - v.z : v.z;
        
        return v2.x + "," + v2.y + "," + v2.z;
    }
}