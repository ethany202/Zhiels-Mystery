using UnityEngine;

public class CustomizedData : MonoBehaviour
{
    private static string characterName = "Malcolm";

    public static float normalSensitivity=40f;

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
