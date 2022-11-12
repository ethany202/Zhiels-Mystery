using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Steamworks;
using Photon.Realtime;

public class RoomController : MonoBehaviourPunCallbacks
{
    public int waitingRoomSceneIndex;
    public Animator sceneTransition;


    void Start()
    {
        waitingRoomSceneIndex = 1;
    }



    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        InitializeGameLogic();
        LoadCharacterSelectScene();
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        LoadCharacterSelectScene();
    }

    public void InitializeGameLogic()
    {
        MapInfoController.playerCount = PhotonNetwork.PlayerList.Length;
        MapInfoController.maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

        //if (MapInfoController.playerCount >  MapInfoController.maxPlayers/ 2)
        //{
        //    LoadSceneLogic.playerRole = 1;
        //}
        //else
        //{
        //    LoadSceneLogic.playerRole = 0;
        //}
        //LoadSceneLogic.playerName = SteamFriends.GetPersonaName();
        //LoadSceneLogic.playerID = SteamUser.GetSteamID().ToString();
    }

    private void LoadCharacterSelectScene()
    {
        if (PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            sceneTransition.SetTrigger("FadeIn");
            if (PhotonNetwork.IsMasterClient)
            {
                //SceneManager.LoadScene(waitingRoomSceneIndex);
                PhotonNetwork.LoadLevel(waitingRoomSceneIndex);
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }
}
