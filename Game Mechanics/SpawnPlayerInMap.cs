using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnPlayerInMap : MonoBehaviour
{

    public Transform detectiveSpawn, hitmenSpawn;

    public AudioController audioController;

    void Start()
    {
        //LoadSceneLogic.SetSpawnLocation();
        //SetSpawn(LoadSceneLogic.spawnLocation);
        if (LoadSceneLogic.playerRole == 1)
        {
            PhotonNetwork.Instantiate(CustomizedData.GetSkinName(), detectiveSpawn.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(CustomizedData.GetSkinName(), hitmenSpawn.position, Quaternion.identity);
        }
        audioController.SetAllVolume();

        Debug.Log(GameObject.FindGameObjectsWithTag("Player").Length);
    }

}
