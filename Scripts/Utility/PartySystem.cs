using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySystem : MonoBehaviour
{

    private static List<string> players = new List<string>();
    private static string userId;
    private static string[] expectedUsers;
    private static bool isPartyLeader = true;
    private static bool currentlyInParty = false;
    private static string partyLeaderID;

    public static void SetPartyLeaderID(string val)
    {
        partyLeaderID = val;
    }

    public static string GetPartyLeaderID()
    {
        return partyLeaderID;
    }

    public static void SetInParty(bool val)
    {
        currentlyInParty = val;
    }

    public static bool IsInParty()
    {
        return currentlyInParty;
    }

    public static bool IsPartyLeader()
    {
        return isPartyLeader;
    }

    public static void SetIsPartyLeader(bool val)
    {
        isPartyLeader = val;
        
    }

    public static void SetUserID(string id)
    {
        userId = id;
    }

    public static string GetUserID()
    {
        return userId;
    }

    public static void SetExpectedUsers()
    {
        expectedUsers = new string[players.Count];
        for (int i = 0; i < expectedUsers.Length; i++)
        {
            expectedUsers[i] = players[i];
        }
    }

    public static void AddExpectedUsers(string userId)
    {
        players.Add(userId);
    }

    public static void ClearExpectedUsers()
    {
        players.Clear();
        players.Add(userId);
    }

    public static void RemoveFromExpectedUsers(string userId)
    {
        players.Remove(userId);
    }

    public static string[] GetExpectedUsers()
    {
        SetExpectedUsers();
        return expectedUsers;
    }



}
