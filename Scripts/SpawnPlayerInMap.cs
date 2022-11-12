using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.IO;
using Steamworks;

public class SpawnPlayerInMap : MonoBehaviourPun, IOnEventCallback
{

    public Transform detectiveSpawn, hitmenSpawn;

    public AudioController audioController;

    private const byte AddPlayerToList = 7;

    void Awake()
    {
        //LoadSceneLogic.SetSpawnLocation();
        //SetSpawn(LoadSceneLogic.spawnLocation);
        GameObject playerObject = null;
        //if (LoadSceneLogic.playerRole == 1)
        //{
        //    playerObject = PhotonNetwork.Instantiate(CustomizedData.GetCharacterName(), detectiveSpawn.position, Quaternion.identity);
        //}
        //else
        //{
        //    playerObject = PhotonNetwork.Instantiate(CustomizedData.GetCharacterName(), hitmenSpawn.position, Quaternion.identity);
        //}

        playerObject = PhotonNetwork.Instantiate(CustomizedData.GetCharacterName(), hitmenSpawn.position, Quaternion.identity);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(AddPlayerToList, "Player Joined", raiseEventOptions, SendOptions.SendReliable);

        audioController.SetAllVolume();

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
        if (eventCode == AddPlayerToList)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < players.Length; i++)
            {
                LoadSceneLogic.scenePlayers.Add(players[i].name, players[i]);
            }
        }
    }
}
