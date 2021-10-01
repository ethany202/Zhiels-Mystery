using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Steamworks;

public class SkillsPresets : MonoBehaviour
{

    private string steamId;

    public GameObject warning;

    private int remainingPoints;
    public TMP_Text remainingPointsText;

    public TMP_Text speedText;
    public TMP_Text agilityText;
    //public TMP_Text heightText;
    public TMP_Text visionText;
    public TMP_Text strengthText;
    public TMP_Text flexibilityText;
    public TMP_Text weightText;
    public TMP_Text combatExpText;

    Dictionary<string, int> recordedValues;

    Dictionary<string, TMP_Text> textValues;

    void Start()
    {
        remainingPoints = 4;
        recordedValues = new Dictionary<string, int>()
        {
            {"speed", 3 }, 
            {"agility", 3}, 
            //{"height", 3}, 
            {"vision", 3 },
            {"strength", 3}, 
            //{"flexibility", 3}, 
            {"weight", 3}, 
            {"combat experience", 3}
        };
        textValues = new Dictionary<string, TMP_Text>()
        {
            {"speed", speedText}, 
            {"agility", agilityText}, 
            //{"height", heightText}, 
            {"vision", visionText },
            {"strength", strengthText}, 
            //{"flexibility", flexibilityText}, 
            {"weight", weightText}, 
            {"combat experience", combatExpText}
        };

        SetConstants();
    }

    public void InstantiateValues()
    {
        for(int i = 0; i < recordedValues.Count; i++)
        {
            if (PlayerPrefs.HasKey(steamId + recordedValues.ElementAt(i).Key))
            {
                recordedValues[recordedValues.ElementAt(i).Key] = PlayerPrefs.GetInt(steamId + recordedValues.ElementAt(i).Key);
                UpdateValue(recordedValues.ElementAt(i).Key);
            }         
        }
    }

    public void UpdateValue(string key)
    {
        textValues[key].text = key + ":  " + recordedValues[key] + " / 6";
        SetConstants();
    }

    public void IncreaseValue(string key)
    {
        if(recordedValues[key] == 6)
        {
            return;
        }
        recordedValues[key] += 1;
        CalculateRemainingPoints();

        if(remainingPoints < 0)
        {
            recordedValues[key] -= 1;
            CalculateRemainingPoints();
            warning.SetActive(true);
        }
        UpdateValue(key);
        remainingPointsText.text = "remaining points: " + remainingPoints;
    }

    public void DecreaseValue(string key)
    {
        if (recordedValues[key] == 1)
        {
            return;
        }
        recordedValues[key] -= 1;
        CalculateRemainingPoints();
        UpdateValue(key);
        remainingPointsText.text = "remaining points: " + remainingPoints;

    }

    public void CalculateRemainingPoints()
    {
        remainingPoints = 25;
        foreach(var value in recordedValues.Values)
        {
            remainingPoints -= value;
        }
    }

    private void SetPlayerPrefs(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public void ResetValues()
    {
        foreach(var key in recordedValues.Keys.ToList())
        {
            recordedValues[key] = 3;
            UpdateValue(key);
        }
        remainingPoints = 4;
        remainingPointsText.text = "remaining points: " + remainingPoints;
    }

    public void SaveValues()
    {
        foreach (var key in recordedValues)
        {
            SetPlayerPrefs(steamId + key.Key, key.Value);
        }
    }

    public void SetConstants()
    {
        CustomizedData.baseSpeed = (recordedValues["speed"]/3.0f) * 1.17f;

        CustomizedData.vision = 400f + ((recordedValues["vision"] - 3) * 10);
        CustomizedData.weight = Mathf.Sqrt(recordedValues["weight"] / 3.0f) * 90;

        //CustomizedData.flexibility = recordedValues["flexibility"];
        CustomizedData.strength = recordedValues["strength"];
    }

    public void SaveData()
    {
        if (SteamManager.Initialized)
        {
            steamId = SteamUser.GetSteamID().ToString();
            SaveValues();
            SetConstants();
        }
    }
}
