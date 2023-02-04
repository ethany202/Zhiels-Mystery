using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCapture : MonoBehaviour
{
    public KeyCode screenShotButton;
    void Update()
    {
        if (Input.GetKeyDown(screenShotButton))
        {
            UnityEngine.ScreenCapture.CaptureScreenshot("LibraryCapsuleV1.png");
            Debug.Log("A screenshot was taken!");
        }
    }
}
