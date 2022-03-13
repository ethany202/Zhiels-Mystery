using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Discord;

public class DiscordManager : MonoBehaviour
{

    private static long CLIENT_ID = 946585748432388136;
    public Discord.Discord discord;

    void Start()
    {
        discord = new Discord.Discord(CLIENT_ID, (UInt64)Discord.CreateFlags.Default);
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "In Lobby",
            Assets =
            {
                LargeImage = "zhielslogo"
            }
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Everything is fine!");
            }
        });
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        discord.GetActivityManager().ClearActivity((result) =>
        {
            if (result == Discord.Result.Ok)
            {
                Debug.Log("Success!");
            }
        });
    }


}
