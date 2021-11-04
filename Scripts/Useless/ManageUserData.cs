/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Steamworks;

public class ManageUserData : MonoBehaviour
{

    private DatabaseReference reference;
    private FirebaseDatabase database;
    private FirebaseApp app;

    private string steamId;
    private string currentPersonaName;

    private DataSnapshot snapshot;

    void Start()
    {

        InitializeFirebase();
        ConfirmGooglePlayServices();

        steamId = SteamUser.GetSteamID().ToString();
        currentPersonaName = SteamFriends.GetPersonaName();
        SendSteamCredentials(steamId);
        AlterSteamInfo(steamId, "currency", 1000);
    }

    private void InitializeFirebase()
    {
        database = FirebaseDatabase.GetInstance("https://zhiel-s-mystery-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void ConfirmGooglePlayServices()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public FirebaseDatabase GetFirebaseDatabase()
    {
        return database;
    }

    public void SendSteamCredentials(string steamId)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }
        reference.Child("users").Child(steamId).Child("name").SetValueAsync(currentPersonaName);
    }

    public void AlterSteamInfo(string steamId, string category, object val)
    {
        reference.Child("users").Child(steamId).Child(category).SetValueAsync(val);
    }

    public void AddSteamInfo(string steamId, string category, string val)
    {
        reference.Child("users").Child(steamId).Child(category).Child(val);
    }

    public void LoadSteamUserData(string steamId)
    {
        reference.Child("users").Child(steamId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                snapshot = task.Result;
            }
            else if (task.IsFaulted)
            {
                return;
            }
        });
    }

    public void InitializeSteamID()
    {
        steamId = SteamUser.GetSteamID().ToString();
    }

    public string GetSteamID()
    {
        return steamId;
    }

    public void InitializePersonaName()
    {
        currentPersonaName = SteamFriends.GetPersonaName();
    }

    public string GetPersonaName()
    {
        return currentPersonaName;
    }
}*/
