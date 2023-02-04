using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadSceneLogic : MonoBehaviour
{

    public static int examPhase;
    public static bool savedGame = false;

    public static CharacterManager player;

    private static GameObject healthText;
    private static GameObject instructionsPrimary;
    private static GameObject instructionsSecondary;

    public static GameObject GetHealthText()
    {
        return healthText;
    }

    public static void SetHealthBarUI(GameObject newText)
    {
        healthText = newText;
    }

    public static void SetInstructions(GameObject obj)
    {
        instructionsPrimary = obj;
    }

    public static void DisplayInstructions(bool value)
    {
        if (instructionsPrimary == null)
        {
            return;
        }
        instructionsPrimary.SetActive(value);
    }

    public static void ChangeInstructionsText(string value)
    {
        if (instructionsPrimary == null)
        {
            return;
        }

        if (value.Length > 4)
        {
            value = value.Substring(0, 4);
        }

        instructionsPrimary.GetComponentInChildren<TMP_Text>().text = value;
    }

    // Secondary instructions

    public static void SetInstructionsSecondary(GameObject obj)
    {
        instructionsSecondary = obj;
    }

    public static void DisplayInstructionsSecondary(bool value)
    {
        if (instructionsSecondary == null)
        {
            return;
        }
        instructionsSecondary.SetActive(value);
    }

    public static void ChangeInstructionsTextSecondary(string value)
    {
        if (instructionsSecondary == null)
        {
            return;
        }

        if (value.Length > 4)
        {
            value = value.Substring(0, 4);
        }

        instructionsSecondary.GetComponentInChildren<TMP_Text>().text = value;
    }
}
