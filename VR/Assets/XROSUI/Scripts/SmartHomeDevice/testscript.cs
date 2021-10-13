using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://forum.unity.com/threads/what-is-the-math-behind-calculating-transform-forward-of-an-object.521318/
/// </summary>

public class testscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // print("rotation: " + this.transform.rotation);
        // print("unity forward");
        // print(this.transform.forward.x);
        // print(this.transform.forward.y);
        // print(this.transform.forward.z);
        //
        // print("euler forward");
        // float X1 = Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad) * Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad);
        // float Y1 = Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad);
        // float Z1 = Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad) * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        // print("X1"+X1);
        // print("Y1"+Y1);
        // print("Z1"+Z1);
        //
        // print("quaternion forward");
        // Vector3 v = NewRotation(transform.rotation, Vector3.forward);
        // print("X2" +v.x);
        // print("Y2" +v.y);
        // print("Z2" +v.z);
    }
    
     
    public Vector3 NewRotation(Quaternion rotation, Vector3 point) 
    {
        float num1 = rotation.x * 2f;
        float num2 = rotation.y * 2f;
        float num3 = rotation.z * 2f;
        float num4 = rotation.x * num1;
        float num5 = rotation.y * num2;
        float num6 = rotation.z * num3;
        float num7 = rotation.x * num2;
        float num8 = rotation.x * num3;
        float num9 = rotation.y * num3;
        float num10 = rotation.w * num1;
        float num11 = rotation.w * num2;
        float num12 = rotation.w * num3;
        Vector3 vector3;
        vector3.x = (float) ((1.0 - ((double) num5 + (double) num6)) * (double) point.x + ((double) num7 - (double) num12) * (double) point.y + ((double) num8 + (double) num11) * (double) point.z);
        vector3.y = (float) (((double) num7 + (double) num12) * (double) point.x + (1.0 - ((double) num4 + (double) num6)) * (double) point.y + ((double) num9 - (double) num10) * (double) point.z);
        vector3.z = (float) (((double) num8 - (double) num11) * (double) point.x + ((double) num9 + (double) num10) * (double) point.y + (1.0 - ((double) num4 + (double) num5)) * (double) point.z);
        return vector3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
