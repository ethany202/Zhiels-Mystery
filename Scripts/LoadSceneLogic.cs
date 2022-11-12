using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadSceneLogic : MonoBehaviour
{

    public static int examPhase;
    public static bool savedGame = false;

    public static CharacterManager player;

    public static Dictionary<string, GameObject> scenePlayers = new Dictionary<string, GameObject>();

    private static GameObject healthText;
    private static GameObject instructions;

    public static Transform spawnPos;

    public static GameObject GetHealthText()
    {
        return healthText;
    }

    public static void SetHealthBarUI(GameObject newText)
    {
        healthText = newText;
    }

    public static void SetInstructionsGameObject(GameObject obj)
    {
        instructions = obj;
    }

    public static void DisplayInstructions(bool value)
    {
        if (instructions == null)
        {
            return;
        }
        instructions.SetActive(value);
    }

    public static void ChangeInstructionsText(string value)
    {
        if (instructions == null)
        {
            return;
        }
        instructions.GetComponentInChildren<TMP_Text>().text = value;
    }

    public static void RestartCombatStage()
    {
        SceneManager.LoadScene(2);
        player.body.position = spawnPos.position;
    }
}
