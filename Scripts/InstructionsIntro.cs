using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionsIntro : MonoBehaviour
{

    public GameObject cinemaTop, cinemaBot;

    public TMP_Text crouchText;

    public TMP_Text slideText;
    public TMP_Text sprintText;

    public TMP_Text grabText;

    public TMP_Text dropText;

    private void Start()
    {
        cinemaTop.SetActive(false);
        cinemaBot.SetActive(false);

        crouchText.text = Shorten(ControlsConstants.keys["crouch"].ToString());

        slideText.text = Shorten(ControlsConstants.keys["slide"].ToString());
        sprintText.text= Shorten(ControlsConstants.keys["sprint"].ToString());

        grabText.text = Shorten(ControlsConstants.keys["grab"].ToString());
        dropText.text = Shorten(ControlsConstants.keys["drop"].ToString());
    }

    public string Shorten(string value)
    {
        if (value.Length > 5)
        {
            return value.Substring(0, 6);
        }
        return value;
    }

}
