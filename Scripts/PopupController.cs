using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PopupController : MonoBehaviour
{

    public GameObject popup;

    public AudioSource backgroundMusic;
    public VideoPlayer tvVideo;

    private bool isOpen = false;

    void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
            if (popup != null) { popup.SetActive(isOpen); }
            
        }
        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;

            if(tvVideo != null) tvVideo.Pause();
            if (backgroundMusic != null) { backgroundMusic.mute = true; }
        }
        else
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;

            if (tvVideo != null)  tvVideo.Play();
            if (backgroundMusic != null) { backgroundMusic.mute = false; }
        }

        if(LoadSceneLogic.player != null)
        {
            LoadSceneLogic.player.enabled = !isOpen;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
