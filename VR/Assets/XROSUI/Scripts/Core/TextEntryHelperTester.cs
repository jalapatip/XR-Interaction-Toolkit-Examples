using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEntryHelperTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            print(TextEntryHelper.ComputeLevenshteinDistance("hello", "yaharo"));
            print(TextEntryHelper.ComputeLevenshteinDistance("kitten", "sitting"));
            print(TextEntryHelper.ComputeErrorRate("kitten", "sitting"));
            print(TextEntryHelper.ComputeWordsPerMinute("kitten", 6));
        }
    }
}
