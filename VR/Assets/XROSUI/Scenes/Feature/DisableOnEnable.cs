using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.SetActive(false);
    }
}
