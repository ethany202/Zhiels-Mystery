using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class StatsController : MonoBehaviourPunCallbacks
{

    public GameObject fpsUI;
    public GameObject pingUI;

    public TMP_Text fpsText;
    public TMP_Text pingText;

    void Start()
    {
        fpsUI.SetActive(CustomizedData.showFPS);
        pingUI.SetActive(CustomizedData.showFPS);

    }

    void Awake()
    {
        if (CustomizedData.showFPS)
        {
            InvokeRepeating("ShowFPS", 0f, 5f);
            //ShowFPS();
            InvokeRepeating("ShowPing", 0f, 5f);
            //ShowPing();
        }



    }

    void ShowFPS()
    {
        fpsText.text = "fps: " + (int)(1 / Time.unscaledDeltaTime);
    }

    void ShowPing()
    {
        pingText.text = "ping: " + PhotonNetwork.GetPing();
    }
}
