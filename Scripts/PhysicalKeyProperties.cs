using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalKeyProperties : MonoBehaviour
{

    private string ID="";

    public AudioSource audioSource;
    public AudioClip dropSound;

    public GameObject keyUI;

    public void SetKeyID(string newID)
    {
        ID = newID;
        Debug.Log(ID);
    }

    public string GetKeyID()
    {
        return ID;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayDropSound();
    }

    private void PlayDropSound()
    {
        audioSource.PlayOneShot(dropSound);
    }

    public GameObject GetKeyUI()
    {
        return keyUI;
    }
    /*private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["grab"].ToString());
        }
    }*/
}
