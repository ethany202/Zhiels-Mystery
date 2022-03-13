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

    public TMP_Text currentUser;

    public RawImage profileImage;

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

    private CSteamID steamID;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
            Application.Quit();
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
        steamID = SteamUser.GetSteamID();

        currentUser.text = SteamFriends.GetPersonaName();

        int imageId = SteamFriends.GetLargeFriendAvatar(steamID);
        
        if(imageId == -1)
        {
            avatarError.SetActive(true);
            return;
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

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if(callback.m_steamID.ToString() != steamID.ToString())
        {
            return;
        }

        profileImage.texture = GetSteamImage(callback.m_iImage);
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {}
}
