using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadData : MonoBehaviour
{

    public GameObject districtText;
    public GameObject healthText;

    void Start()
    {
        LoadSceneLogic.SetDistrictText(districtText);
        LoadSceneLogic.SetHealthBarUI(healthText);
    }
}
