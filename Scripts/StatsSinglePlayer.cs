using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class StatsSinglePlayer : MonoBehaviour
{

    public GameObject fpsUI;
    public TMP_Text fpsText;

    void Update()
    {
        fpsUI.SetActive(CustomizedData.showFPS);
        ShowFPS();
    }

    void ShowFPS()
    {
        fpsText.text = "fps: " + (int)(1 / Time.unscaledDeltaTime);
    }
}
