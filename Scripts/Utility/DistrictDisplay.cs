using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistrictDisplay : MonoBehaviour
{
    private TMP_Text districtName;

    void FadeIn()
    {
        districtName.CrossFadeAlpha(1, 0.5f, false);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);
        districtName.CrossFadeAlpha(0, 0.5f, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "District")
        {
            if (districtName == null)
            {
                districtName = LoadSceneLogic.GetDistrictText().GetComponent<TMP_Text>();
                districtName.CrossFadeAlpha(0, 0f, false);
            }
            districtName.text = other.name + "";
            FadeIn();
            StartCoroutine("FadeOut");
        }
    }
}
