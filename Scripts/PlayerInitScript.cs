using UnityEngine;
using Steamworks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerInitScript : MonoBehaviour
{
    public GameObject avatarError, loadSavedScene, startButton;

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


            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

            HandleSaveData();
        }
        else
        {
            Application.Quit();
        }
    }

    private void LoadSteamInfo()
    {
        steamID = SteamUser.GetSteamID();

        currentUser.text = SteamFriends.GetPersonaName();

        int imageId = SteamFriends.GetLargeFriendAvatar(steamID);

        if (imageId == -1)
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
        if (callback.m_steamID.ToString() != steamID.ToString())
        {
            return;
        }

        profileImage.texture = GetSteamImage(callback.m_iImage);
    }

    public void LoadCharacterSelect()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    { }

    public void HoveringStartButton(Image btn)
    {
        btn.color = Color.black;
    }

    public void HoveringStartText(TMP_Text txt)
    {
        txt.color = Color.white;
    }

    public void ExitStartButton(Image btn)
    {
        btn.color = Color.white;
    }

    public void ExitStartText(TMP_Text txt)
    {
        txt.color = Color.black;
    }

    public void HandleSaveData()
    {
        SaveData playerData = SaveSystem.LoadPlayerState();
        if (playerData != null)
        {
            startButton.SetActive(false);
            loadSavedScene.SetActive(true);
            LoadSceneLogic.savedGame = true;
            LoadSceneLogic.examPhase = playerData.examPhase;
        }
        else
        {
            LoadSceneLogic.savedGame = false;
        }
    }

    public void LoadSavedGame()
    {        
        SceneManager.LoadSceneAsync(LoadSceneLogic.examPhase);
    }
}
