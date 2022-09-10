using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSync : MonoBehaviour
{
    void Awake() 
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 30;
    }
    void Start()
    {
        // Switch to 640 x 480 full-screen
        Screen.SetResolution(1280, 720, true);
    }
}
