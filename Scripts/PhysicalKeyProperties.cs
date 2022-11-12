using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalKeyProperties : MonoBehaviour
{

    private string ID="";

    public AudioSource audioSource;
    public AudioClip dropSound;

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

    /*private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["grab"].ToString());
        }
    }*/
}
