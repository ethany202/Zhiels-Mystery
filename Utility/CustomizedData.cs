using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizedData : MonoBehaviour
{
    private static string skinName = "Malcolm Skin";

    public static float baseSpeed;

    public static float regJumpHeight = 1.5f;
    public static float sprintJumpHeight = 1.75f;

    public static float vision;

    public static float weight;
    public static float strength;
    //public static float flexibility;

    public static int skinType = 0; // 0 = Type 1 Mixamo(Malcolm, remy), 1 = Type 2 Mixamo, 2 = Other

    public static int normalSensitivity;
    public static int aimSensitivity;

    public static bool showFPS;

    public static string GetSkinName()
    {
        return skinName;
    }

    public static void SetSkinName(string name)
    {
        skinName = name;
        if(name == "Malcolm Skin")
        {
            skinType = 0;
        }
        else
        {
            skinType = 1;
        }
    }
}
