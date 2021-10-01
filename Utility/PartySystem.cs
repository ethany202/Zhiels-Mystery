using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySystem : MonoBehaviour
{

    private List<string> players = new List<string>();

    private string userId;
    private string[] expectedUsers;
    private string partyLeaderID;

    private bool isPartyLeader;
    private bool currentlyInParty;

    public GameObject findMatchButton, notPartyLeaderButton, deletePartyButton, inPartyImage;

    void Start()
    {
        isPartyLeader = true;
        currentlyInParty = false;
    }

    public void SetPartyLeaderID(string val)
    {
        partyLeaderID = val;
    }

    public string GetPartyLeaderID()
    {
        return this.partyLeaderID;
    }

    public void SetInParty(bool val)
    {
        currentlyInParty = val;
        if (currentlyInParty)
        {
            inPartyImage.SetActive(true);
            if(isPartyLeader)
                deletePartyButton.SetActive(true);
        }
        else
        {
            deletePartyButton.SetActive(false);
        }
    }

    public bool IsInParty()
    {
        return currentlyInParty;
    }

    public void SetIsPartyLeader(bool val)
    {
        isPartyLeader = val;

        findMatchButton.SetActive(isPartyLeader);
        notPartyLeaderButton.SetActive(!isPartyLeader);
    }

    public void SetUserID(string id)
    {
        userId = id;
    }

    public void SetExpectedUsers()
    {
        expectedUsers = new string[players.Count];

        for(int i = 0; i < expectedUsers.Length; i++)
        {
            expectedUsers[i] = players[i];
        }
    }

    public void AddExpectedUsers(string userId)
    {
        players.Add(userId);
    }

    public void ClearExpectedUsers()
    {
        players.Clear();
    }

    public void RemoveFromExpectedUsers(string userId)
    {
        players.Remove(userId);
    }

    public string[] GetExpectedUsers()
    {
        SetExpectedUsers();
        return expectedUsers;
    }



}
