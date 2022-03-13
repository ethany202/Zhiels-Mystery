using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnPlayer : MonoBehaviour
{

    public Transform pos;

    void Start()
    {
        CreatePlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void CreatePlayer()
    {

        string choosenCharacter = CustomizedData.GetCharacterName();
        //PhotonNetwork.InstantiateRoomObject(choosenSkin, spawnLocation, Quaternion.identity);
        GameObject newPlayer = PhotonNetwork.Instantiate("Malcolm", pos.position, Quaternion.identity);
        Debug.LogError(newPlayer);
    }

}
