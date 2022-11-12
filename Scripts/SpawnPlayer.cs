using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public static GameObject playerBody;
    public Transform pos;
    public GameObject playerPrefab;

    void Awake()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {

        string choosenCharacter = CustomizedData.GetCharacterName();
        //PhotonNetwork.InstantiateRoomObject(choosenSkin, spawnLocation, Quaternion.identity);
        playerBody=Instantiate(playerPrefab, pos.position, pos.rotation);
    }

}
