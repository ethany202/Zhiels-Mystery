using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using Steamworks;
using ExitGames.Client.Photon;
using TMPro;

public class CharacterSelectController : MonoBehaviourPunCallbacks, IOnEventCallback
{

    public const byte PlayerJoin = 4;
    public const byte PlayerReady = 5;

    private int multiplayerScene;

    private int lobbySceneIndex = 0;
    // private int loadingSceneIndex;

    private int playerCount;
    private int hitmenCount;
    private int detectiveCount;

    private bool ready;

    //private HashSet<string> readiedPlayersSet = new HashSet<string>();
    private List<string> readiedPlayersSet = new List<string>();
    private HashSet<string> selectedCharacters = new HashSet<string>();
    //private List<string> readiedPlayers = new List<string>();
    public TMP_Text roleText, player1Text, player2Text;

    public GameObject assassinCharacters, detectiveCharacters;

    void Start()
    {
        InitializeNames();

        multiplayerScene = MapInfoController.GetMultiplayerMapScene();
        ready = false;

        if (LoadSceneLogic.playerRole == 0)
        {
            roleText.text = "Role: Assassin";
        }
        else
        {
            roleText.text = "Role: Detective";
            detectiveCharacters.SetActive(true);
            assassinCharacters.SetActive(false);
        }
    }

    public void InitializeNames()
    {
        player1Text.text = LoadSceneLogic.playerName + " (you)";
        if (MapInfoController.maxPlayers > 2)
        {
            object[] joinData = new object[] { LoadSceneLogic.playerName, LoadSceneLogic.playerRole };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(PlayerJoin, joinData, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public void SetPlayerReady()
    {
        object[] sendData = new object[] { LoadSceneLogic.playerID, CustomizedData.GetCharacterName() };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(PlayerReady, sendData, raiseEventOptions, SendOptions.SendReliable);
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        QueueManager.SetPlayerLeft(true);
        DelayCancel();
    }

    void Update()
    {
        WaitingForCharacterSelect();
    }

    void WaitingForCharacterSelect()
    {
        if (ready)
        {
            LoadScene();
            ready = false;
        }
    }

     void LoadScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(multiplayerScene);

        }
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync(lobbySceneIndex);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PlayerReady)
        {
            object[] sendData = (object[])photonEvent.CustomData;

            string character = (string)sendData[1];
            string playerID = (string)sendData[0];

            selectedCharacters.Add(character);
            readiedPlayersSet.Add(playerID);
            if (readiedPlayersSet.Count == MapInfoController.maxPlayers)
            {
                ready = true;
            }
        }
        if (eventCode == PlayerJoin)
        {
            object[] joinData = (object[])photonEvent.CustomData;
            string player2Name = (string)joinData[0];
            int player2Role = (int)joinData[1];
            if (player2Role == LoadSceneLogic.playerRole)
            {
                player2Text.text = player2Name + "";
            }
        }
    }

    public HashSet<string> GetSelectedCharacters()
    {
        return selectedCharacters;
    }

}
