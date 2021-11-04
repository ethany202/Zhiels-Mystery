using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeGamemode : MonoBehaviour
{
    public GameObject gamemodeMenu;
    public TMP_Text text;
    private int gameScene;
    private string style = "Solo";
    private string map = "Zhieltropolis";

    public LobbyController lobbyController;



    public void SetMap(string map)
    {
        SetGamemode(1, map);
        lobbyController.SetMap(map);
    }

    public void SetStyle(string style)
    {
        SetGamemode(0, style);
        if(style == "Solo")
        {
            lobbyController.SetRoomSize(2);
        }
        if(style == "Duo")
        {
            lobbyController.SetRoomSize(4);
        }
    }

    public void SetGamemode(int val, string mode)
    {
        if (val == 0)
        {
            style = mode;
        }
        if (val == 1)
        {
            map = mode;
        }
        text.text = "GAMEMODE: " + style + "; " + map;
    }
}
