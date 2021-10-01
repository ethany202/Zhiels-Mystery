using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Steamworks;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject playerLeftMessage;
    public GameObject avatarError;
    public GameObject notLoggedIn;

    public TMP_Text currentUser;

    public RawImage profileImage;

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    void Start()
    {

        Cursor.lockState = CursorLockMode.None;

        SteamAPI.Init();
        if (SteamManager.Initialized)
        {
            LoadSteamInfo();

            ConnectToPhoton();

            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

            if (QueueManager.playerLeft)
                playerLeftMessage.SetActive(true);
            else
                playerLeftMessage.SetActive(false);
            QueueManager.SetPlayerLeft(false);
        }
        else
        {
            notLoggedIn.SetActive(true);
        }
        
    }

    public void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void LoadSteamInfo()
    {
        CSteamID steamId = SteamUser.GetSteamID();

        currentUser.text = SteamFriends.GetPersonaName();

        int imageId = SteamFriends.GetLargeFriendAvatar(steamId);
        
        if(imageId == -1)
        {
            imageId = SteamFriends.GetLargeFriendAvatar(steamId);
            avatarError.SetActive(true);
        }

        profileImage.texture = GetSteamImage(imageId);
    }

    private Texture2D GetSteamImage(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

        if (isValid)
        {
            byte[] image = new byte[width * height * 4];
            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));
            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }

        return texture;
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {}
}
