using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizedData : MonoBehaviour
{
    private static string characterName;

    public static float baseSpeed;

    public static float vision;

    public static float weight;
    public static float strength;

    public static float normalSensitivity;
    public static float scopeSensitivity;

    public static bool showFPS;

    public static string GetCharacterName()
    {
        return characterName;
    }

    public static void SetCharacterName(string name)
    {
        characterName = name;
    }
}
