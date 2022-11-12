using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageDisplay : MonoBehaviour
{
    public TMP_Text stageText;
    public string stageName;

    void FadeIn()
    {
        stageText.CrossFadeAlpha(1, 0.5f, false);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);
        stageText.CrossFadeAlpha(0, 0.5f, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //if (stageText == null)
            //{
            //    stageText = LoadSceneLogic.GetDistrictText().GetComponent<TMP_Text>();
            //    stageText.CrossFadeAlpha(0, 0f, false);
            //}
            stageText.text = stageName + "";
            FadeIn();
            StartCoroutine("FadeOut");
        }
    }
}
