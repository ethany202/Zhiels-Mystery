using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PopupController : MonoBehaviour
{

    public GameObject popup;
    private bool isOpen;

    void Start()
    {
        isOpen = false;
        popup.SetActive(false);

    }

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
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
