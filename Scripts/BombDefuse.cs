using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.VFX;

public class BombDefuse : MonoBehaviour
{

    public Breakable breakableWall; // Wall that explodes after defusal
    public GameObject defuseUI; // UI displayed while defusing the bomb
    public GameObject C4object; // Physical C4 to be destroyed
    public GameObject lightBreak; // Light to break during explosion

    private bool defusable=false;   // Player cannot defuse until after opening previous door
    private bool usingCode;     // Has player hit "E" yet to defuse the bomb

    private const string SEQUENCE = "8030"; // Correct sequence to defuse the bomb
    public TMP_Text codeInput;  // Text display of user input

    private CharacterManager player; // Player movement script; disabled during input

    public Animator topCinemaBar, botCinemaBar; // Cinema bar animators
    public GameObject concussionEffect;
    public VisualEffect explosionVFX;

    public AudioSource audioSrc;
    public AudioClip keypadSFX;

    private void OnGUI()
    {
        if (usingCode)
        {
            Event e = Event.current;
            if (e.isKey && e.type == EventType.KeyDown)
            {
                ManageInput(e.keyCode);
            }
        }
        
    }

    public bool IsDefusable()
    {
        return defusable;
    }

    public void SetDefusable(bool defusable)
    {
        this.defusable = defusable;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && IsDefusable())
        {
            
            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["open"].ToString());

            if (Input.GetKeyDown(ControlsConstants.keys["open"]))
            {
                player = other.GetComponent<CharacterManager>();
                LockStatus(true);

                player.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadSceneLogic.DisplayInstructions(false);
        }
    }

    private void ManageInput(KeyCode input)
    {
        

        if (input.CompareTo(KeyCode.Return) == 0)
        {

            audioSrc.PlayOneShot(keypadSFX);

            if (codeInput.text.Equals(SEQUENCE))
            {
                Invoke("ExplodeWall", 2f);
                GetComponent<BombDefuse>().enabled = false;
            }
            else
            {
                codeInput.text = "";
                player.enabled = true;
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

                audioSrc.PlayOneShot(keypadSFX);
            }
            catch (FormatException error)
            {
            }
        }
    }

    private void LockStatus(bool value)
    {
        usingCode = value;
        LoadSceneLogic.DisplayInstructions(value);
        player.GetComponent<CharacterManager>().enabled = !usingCode;
        defuseUI.SetActive(value);
    }

    private void ExplodeWall()
    {
        breakableWall.ExplodeObject();
        Destroy(lightBreak);

        //Invoke("ConcussionEffect", 0.15f);
        StartCoroutine(ConcussionEffect());
    }

    private IEnumerator ConcussionEffect()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        explosionVFX.Play();
        StartCoroutine(LoadSceneLogic.player.viewObject.DamageShake(1f, 2.5f));

        yield return new WaitForSecondsRealtime(0.2f);
        concussionEffect.SetActive(true);

        float damagedHealth = 100 - Mathf.Floor(Vector3.Distance(LoadSceneLogic.player.transform.position, this.transform.position));
        LoadSceneLogic.player.Health = damagedHealth;
    }
}
