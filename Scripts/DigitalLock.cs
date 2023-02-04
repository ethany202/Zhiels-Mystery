using UnityEngine;
using TMPro;
using System;

public class DigitalLock : MonoBehaviour
{
    public string SEQUENCE = "7885";
    private int guessCount = 0;

    public TMP_Text codeInput;
    private bool usingLock = false;
    private bool passed = false;

    private GameObject player;
    public GameObject lockUI;
    public Animator leftDoor, rightDoor;

    public AudioSource doorUnlock;
    public AudioClip unlockSFX;
    public AudioClip keypadSFX;

    public RadioController radio;

    private void OnGUI()
    {
        if (usingLock)
        {
            Event e = Event.current;
            if (e.isKey && e.type==EventType.KeyDown)
            {
                ManageInput(e.keyCode);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!passed)
        {
            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["open"].ToString());

            if (Input.GetKeyDown(ControlsConstants.keys["open"]))
            {
                player = other.gameObject;
                LockStatus(true);
            }
        }   
    }

    private void LockStatus(bool value)
    {
        usingLock = value;
        LoadSceneLogic.DisplayInstructions(value);
        player.GetComponent<CharacterManager>().enabled = !usingLock;
        lockUI.SetActive(value);
    }

    private void OnTriggerExit(Collider other)
    {
        LoadSceneLogic.DisplayInstructions(false);
    }

    private void ManageInput(KeyCode input)
    {
        if (input.CompareTo(KeyCode.Return) == 0)
        {
            if (codeInput.text.Equals(SEQUENCE))
            {
                passed = true;

                doorUnlock.PlayOneShot(unlockSFX);
                Invoke("OpenDoors", 0.5f);

                GetComponent<DigitalLock>().enabled = false;
            }
            else
            {
                codeInput.text = "";
                guessCount++;

                if (guessCount % 3 == 0)
                {
                    radio.PlayVoiceLine();
                }
            }
            LockStatus(false);
        }
        else if (codeInput.text.Length < 4)
        {
            try
            {
                string text = input.ToString();
                int number = int.Parse(text.Substring(text.Length - 1));
                codeInput.text += (number + "");

                doorUnlock.PlayOneShot(keypadSFX);
            }
            catch (FormatException error)
            {
            }
        }
    }

    private void OpenDoors()
    {
        leftDoor.SetTrigger("open");
        rightDoor.SetTrigger("open");
    }
}
