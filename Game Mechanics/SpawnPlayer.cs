using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnPlayer : MonoBehaviour
{

    public Vector3 spawnLocation = new Vector3(0f, 0f, 0f);



    public void CreatePlayer()
    {

        string choosenSkin = CustomizedData.GetSkinName();
        //PhotonNetwork.InstantiateRoomObject(choosenSkin, spawnLocation, Quaternion.identity);
        PhotonNetwork.Instantiate(choosenSkin, spawnLocation, Quaternion.identity);
    }

    public void SetSpawn(Vector3 position)
    {
        spawnLocation = position;
    }
}
