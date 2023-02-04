using UnityEngine;
using Steamworks;

public class SpawnPlayer : MonoBehaviour
{
    public static GameObject playerBody;
    public Transform pos;
    public GameObject playerPrefab;

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    void Awake()
    {
        CreatePlayer();

        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
        }
    }

    public void CreatePlayer()
    {

        string choosenCharacter = CustomizedData.GetCharacterName();
        //PhotonNetwork.InstantiateRoomObject(choosenSkin, spawnLocation, Quaternion.identity);
        playerBody=Instantiate(playerPrefab, pos.position, pos.rotation);
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    { }

}
