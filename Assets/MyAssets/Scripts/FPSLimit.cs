using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimit : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
