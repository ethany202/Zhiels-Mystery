using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class DiceManager : MonoBehaviour
{

    private bool isGuessing = false;
    private Vector3[] redRotations;
    private Vector3[] blueRotations;

    private CharacterManager player;

    public ClockManager clock;
    public GameObject guessUI;
    public TMP_Text guessText;

    public Material correctMat, defaultMat;
    public MeshRenderer[] buttons;

    public Rigidbody redDice, blueDice;

    public Animator doorAnimator;

    private void Awake()
    {
        redRotations = new Vector3[10] { new Vector3(270f, UnityEngine.Random.Range(0f, 180f), 0f), new Vector3(0, 45f, -90f), new Vector3(180f, 90f, 0f), new Vector3(180f, UnityEngine.Random.Range(0f, 270f), 180f), new Vector3(-90, UnityEngine.Random.Range(0, 90f), 0f), new Vector3(0f, 90f, 90f), new Vector3(0f, 90f, 0f), new Vector3(-90f, 90f, 0f), new Vector3(-180f, 0f, 90f), new Vector3(90, UnityEngine.Random.Range(0f, 45f), 0f) };
        blueRotations = new Vector3[10] { new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f), new Vector3(-90f, 90f, 0f), new Vector3(-90f, UnityEngine.Random.Range(0f, 360f), 0f), new Vector3(90f, UnityEngine.Random.Range(0f, 180f), 180f), new Vector3(270f, UnityEngine.Random.Range(0f, 90f), 0f), new Vector3(0f,0f,0f), new Vector3(0f, 90f, 180f), new Vector3(90f, UnityEngine.Random.Range(0f, 180f), 180f), new Vector3(0f, 90f, 180f), new Vector3(270f, UnityEngine.Random.Range(0f, 90f), 0f)};
    }


    private void OnGUI()
    {
        if (isGuessing)
        {
            Event e = Event.current;
            if (e.isKey && e.type == EventType.KeyDown)
            {
                ManageInput(e.keyCode);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<CharacterManager>();

            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["open"].ToString());

            if (Input.GetKeyDown(ControlsConstants.keys["open"]))
            {
                isGuessing = !isGuessing;
                guessUI.SetActive(isGuessing);
                player.enabled = !isGuessing;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LoadSceneLogic.DisplayInstructions(false);
    }

    private void ManageInput(KeyCode key)
    {
        if (key.CompareTo(KeyCode.Return)==0 && guessText.text.Length>0)
        {

            int playerGuess = int.Parse(guessText.text);
            int correctGuess = ClockManager.GetAnswers()[clock.CurrentSumIndex()];

            try
            {
                StartCoroutine(RollDice(playerGuess, correctGuess));
                guessText.text = "";
            }
            catch (Exception e)
            {
                guessText.text = "";
            }
        }
        else
        {
            try
            {
                string text = key.ToString();
                int number = int.Parse(text.Substring(text.Length - 1));
                guessText.text += (number + "");
                if (guessText.text.Length > 2)
                {
                    guessText.text = guessText.text.Substring(1);
                }
            }
            catch (FormatException error)
            {
            }
        }
        
    }

    private IEnumerator RollDice(int playerGuess, int correctGuess)
    {
        guessUI.SetActive(false);       // Temporarily deactivates Guess UI
        redDice.AddForce(transform.up*1.5f, ForceMode.Impulse);
        redDice.MoveRotation(Quaternion.Euler(redRotations[clock.CurrentSumIndex()]));

        blueDice.AddForce(transform.up * 1.5f, ForceMode.Impulse);
        blueDice.MoveRotation(Quaternion.Euler(blueRotations[clock.CurrentSumIndex()]));

        buttons[playerGuess - 1].GetComponent<Animator>().SetTrigger("pressedButton");

        yield return new WaitForSecondsRealtime(2f);

        if (correctGuess == playerGuess)
        {
            buttons[playerGuess - 1].material = correctMat;
            if (!clock.ChangeTime())
            {
                PassRoom();
            }
            else
            {
                guessUI.SetActive(true);
            }
        }
        else
        {
            ResetRoom();
            guessUI.SetActive(true);
        }
    }

    private void PassRoom()
    {
        LoadSceneLogic.DisplayInstructions(false);
        isGuessing = false;
        guessUI.SetActive(false);
        this.GetComponent<DiceManager>().enabled = false;
        player.enabled = true;

        doorAnimator.SetBool(Animator.StringToHash("isOpen"), true);
    }

    private void ResetRoom()
    {
        clock.ResetStage();
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].material = defaultMat;
        }
    }
}
