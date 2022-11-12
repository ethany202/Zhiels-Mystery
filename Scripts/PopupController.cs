using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupController : MonoBehaviour
{

    public GameObject popup;
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
            popup.SetActive(isOpen);
        }
        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

    public void LeaveGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
