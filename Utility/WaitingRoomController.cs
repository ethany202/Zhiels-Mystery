using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    private int multiplayerScene;
    private int maxPlayers;

    private int lobbySceneIndex = 0;
    // private int loadingSceneIndex;

    private int playerCount;
    private int hitmenCount;
    private int detectiveCount;

    public TMP_Text roleDisplay;

    bool ready;

    public GameObject menu;
    bool menuOpen;

    public SpawnPlayer spawnLocation;

    public Transform hitmenSpawnTransform;
    public Transform detectiveSpawnTransform;
    public Vector3 hitmenSpawn;
    public Vector3 detectiveSpawn;

    public GameObject gameReady;

    public TMP_Text currentMap;
    public TMP_Text currentStyle;

    public AudioController audioController;
    void ResetMapController()
    {
        MapInfoController.maxPlayers = 0;
        MapInfoController.playerCount = 0;
        playerCount = 0;
        hitmenCount = 0;
        detectiveCount = 0;
    }

    public void SpawnLogistics()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        MapInfoController.playerCount = playerCount;

        if(playerCount > MapInfoController.maxPlayers / 2)
        {
            spawnLocation.SetSpawn(detectiveSpawn);
            RoleManagement.SetRole(1);
            LoadSceneLogic.playerRole = 1;
            roleDisplay.text = "ROLE: DETECTIVE";
        }
        else
        {
            spawnLocation.SetSpawn(hitmenSpawn);
            RoleManagement.SetRole(0);
            LoadSceneLogic.playerRole = 0;
            roleDisplay.text = "ROLE: HITMAN";
        }
        spawnLocation.CreatePlayer();
        audioController.SetAllVolume();

    }

    void Start()
    {
        hitmenSpawn = hitmenSpawnTransform.position;
        detectiveSpawn = detectiveSpawnTransform.position;

        currentMap.text = "Map: "+MapInfoController.GetCurrentMap();
        currentStyle.text = "Style: "+MapInfoController.GetCurrentStyle();
        
        gameReady.SetActive(false);
        ResetMapController();
        multiplayerScene = MapInfoController.GetMultiplayerMapScene();
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        MapInfoController.maxPlayers = maxPlayers;
        SpawnLogistics();
        ready = false;
        menuOpen = false;
        PlayerCountUpdate();

        
    }

    public void PlayerCountUpdate()
    {
        
        playerCount = PhotonNetwork.PlayerList.Length;

        MapInfoController.playerCount = playerCount;
        if (playerCount == maxPlayers)
        {
            ready = true;
        }
        else
        {
            ready = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        audioController.SetAllVolume();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        QueueManager.SetPlayerLeft(true);
        DelayCancel();
    }

    void Update()
    {
        WaitingForMorePlayers();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu(!menuOpen);
        }
    }

    void WaitingForMorePlayers()
    {
        if (ready)
        {
            gameReady.SetActive(true);
            StartCoroutine(LoadScene());
            ready = false;
        }
    }

    IEnumerator LoadScene()
    {  
        if (!PhotonNetwork.IsMasterClient)
        {
            yield break;
            //return;
        }
        yield return new WaitForSecondsRealtime(3f);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(lobbySceneIndex);
    }

    public void OpenMenu(bool val)
    {
        menu.SetActive(val);
        menuOpen = val;
        if (menuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    

}
