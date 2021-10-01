using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapInfoController : MonoBehaviour
{
    public static int multiplayerMapScene = 3;

    public static int maxPlayers = 0;
    public static int playerCount = 0;

    public static string currentMap;
    public static string currentStyle;

    public static void SetMultiplayerMapScene(int n)
    {
        multiplayerMapScene = n;
    }

    public static int GetMultiplayerMapScene()
    {
        return multiplayerMapScene;
    }

    public static string GetCurrentMap()
    {
        return currentMap;
    }

    public static string GetCurrentStyle()
    {
        return currentStyle;
    }

}
