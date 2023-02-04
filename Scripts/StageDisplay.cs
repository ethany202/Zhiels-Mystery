using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageDisplay : MonoBehaviour
{
    public Image parentImg;

    public TMP_Text stageText;
    public string stageName;

    public RadioController announcement;
    public GameObject backgroundMusic;

    void FadeIn()
    {
        stageText.CrossFadeAlpha(1, 1f, false);
        parentImg.CrossFadeAlpha(1, 1f, false);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3f);
        stageText.CrossFadeAlpha(0, 1f, false);
        parentImg.CrossFadeAlpha(0, 1f, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            stageText.text = stageName + "";
            FadeIn();
            StartCoroutine("FadeOut");

            if (announcement != null)
            {
                StartCoroutine(announcement.PlayVoiceLineDelay(1.5f));
            }
            if (backgroundMusic != null)
            {
                backgroundMusic.SetActive(true);
            }
        }
    }
}
