using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    public Text codeText;

    private bool tick;

    private void Awake()
    {
        InvokeRepeating("Ticker", 0f, 0.5f);
    }

    public void Ticker()
    {
        if (tick)
        {
            codeText.text += "l";
        }
        else
        {
            codeText.text = codeText.text.Substring(0, codeText.text.Length-1);
        }
        tick = !tick;
    }
}
