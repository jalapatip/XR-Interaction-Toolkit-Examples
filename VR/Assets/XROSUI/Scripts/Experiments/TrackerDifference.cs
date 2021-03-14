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
    private float accumulatedDistance0 = 0;
    [SerializeField]
    private float accumulatedDistance1 = 0;

    [SerializeField]
    private int frameCount = 0;

    [SerializeField]
    private float avgDistance0 = 0;
    
    [SerializeField]
    private float avgDistance1 = 0;
    
    [SerializeField]
    private List<float> distanceList;
    // Start is called before the first frame update
    void Start()
    {
        distanceList = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDifference();

        accumulatedDistance0 += Vector3.Distance(actualTrackerTransform.localPosition, predictedTrackers[0].localPosition);
        accumulatedDistance1 += Vector3.Distance(actualTrackerTransform.localPosition, predictedTrackers[1].localPosition);
        frameCount++;
        avgDistance0 = accumulatedDistance0 / frameCount;
        avgDistance1 = accumulatedDistance1 / frameCount;
        //distanceList[0] = distance0;
        //distanceList[1] = distance1;

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