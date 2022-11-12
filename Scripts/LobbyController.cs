using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using Steamworks;

public class LobbyController : MonoBehaviourPunCallbacks
{
    // Buttons
    public GameObject cancelButton;
    public GameObject connectButton;
    public GameObject loadButton;
    public GameObject changeModeButton;
    public GameObject changeModeConnectingButton;

    private int desiredRoomSize;
    private int mapId;
    private int roomIndex = 0;

    private string style;
    private string map;
    public const string MAP_KEY = "map";


    //public string[] allMaps = { "Zhieltropolis", "Zeliticus", "Zhijulo" };

    public ChatManager chatSystem;

    private Dictionary<string, int> mapsDictionary = new Dictionary<string, int>()
    {
        {"Zhieltropolis",3 },
        {"Zeliticus",4 },
        {"Zhijulo",5 }
    };


    public RoomController roomController;

    private string region;
    private int ping;

    public TMP_Text regionInfo;
    public TMP_Text regionDisplayHome;

    public Animator regionAnimator;


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        connectButton.SetActive(true);
        loadButton.SetActive(true);

        changeModeButton.SetActive(true);
        changeModeConnectingButton.SetActive(false);

        

        desiredRoomSize = 2;
        map = "Zhieltropolis";
        style = "Solo";
        mapId = 0;

        MapInfoController.currentMap = "Zhieltropolis";
        MapInfoController.currentStyle = "Solo";

        region = PhotonNetwork.CloudRegion;

        DisplayPing();
    }

    public void QueueMatch()
    {
        roomIndex = 0;
        connectButton.SetActive(false);
        cancelButton.SetActive(true);

        FindMatch();

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed");
        FindMatch();
    }

    public void QueueCancel()
    {
        connectButton.SetActive(true);
        cancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public void SetRoomSize(int n)
    {
        desiredRoomSize = n;
        if (n == 2)
        {
            style = "Solo";
            roomController.waitingRoomSceneIndex = 1;
            MapInfoController.maxPlayers = 2;
            MapInfoController.currentStyle = "Solo";
        }
        if (n == 4)
        {
            style = "Duo";
            roomController.waitingRoomSceneIndex = 2;
            MapInfoController.maxPlayers = 4;
            MapInfoController.currentStyle = "Duo";
        }
    }

    public void SetMap(string map)
    {
        MapInfoController.currentMap = map;
        MapInfoController.SetMultiplayerMapScene(mapsDictionary[map]);

        if (PartySystem.IsInParty())
        {
            for(int i = 0; i < PartySystem.GetExpectedUsers().Length;i++)
            {
                chatSystem.SendDirectMessage(PartySystem.GetExpectedUsers()[i], "MapTypeMsg:"+map);
            }
            
        }
        
    }

    public Hashtable CreateProperties()
    {
        Hashtable roomSettings = new Hashtable();
        roomSettings[MAP_KEY] = mapId;
        return roomSettings;
    }

    public void FindMatch()
    {
        roomIndex++;
        RoomOptions roomOps = new RoomOptions();
        roomOps.IsVisible = true;
        roomOps.IsOpen = true;
        roomOps.MaxPlayers = (byte)desiredRoomSize;
        roomOps.CustomRoomProperties = CreateProperties();

        if (!PartySystem.IsInParty())
        {
            PhotonNetwork.JoinOrCreateRoom(map + " " + style + " " + roomIndex, roomOps, null, null);
        }
        else
        {
            for (int i = 0; i < PartySystem.GetExpectedUsers().Length; i++)
            {
                chatSystem.SendDirectMessage(PartySystem.GetExpectedUsers()[i], "RoomIndexMsg:" + roomIndex);
            }

            PhotonNetwork.JoinOrCreateRoom(map + " " + style + " " + roomIndex, roomOps, null, PartySystem.GetExpectedUsers());
            for (int i = 0; i < PartySystem.GetExpectedUsers().Length; i++)
            {
                chatSystem.SendDirectMessage(PartySystem.GetExpectedUsers()[i], "ServerMessage1200");
            }
        }
    }

    public void FindMatchAsPartyMember(string map, string style, int roomIndex)
    {
        PhotonNetwork.JoinRoom(map + " " + style+" "+roomIndex, null);
    }

    public void ChangeRegion(string code)
    {
        region = code;
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToRegion(code);

        DisplayPing();
    }

    public void SetRegionAnimation(string trigger)
    {
        regionAnimator.SetTrigger(trigger);
    }

    private void DisplayPing()
    {
        ping = PhotonNetwork.GetPing();

        regionInfo.text = region + " (" + ping + "ms)";

        regionDisplayHome.text = "REGION: " + region.ToUpper();
    }

}
