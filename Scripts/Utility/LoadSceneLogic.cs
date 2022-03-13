using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSceneLogic : MonoBehaviour
{

    public static int playerRole; // 0 = hitman, 1 = detectives
    public static string playerName;
    public static string playerID;

    public static Dictionary<string, GameObject> scenePlayers = new Dictionary<string, GameObject>();
    public static List<GameObject> criminals = new List<GameObject>();

    private static GameObject districtText;
    private static GameObject healthText;

    public static GameObject GetDistrictText()
    {
        return districtText;
    }

    public static GameObject GetHealthText()
    {
        return healthText;
    }

    public static void SetDistrictText(GameObject text)
    {
        districtText = text;
    }

    public static void SetHealthBarUI(GameObject newText)
    {
        healthText = newText;
    }
}
