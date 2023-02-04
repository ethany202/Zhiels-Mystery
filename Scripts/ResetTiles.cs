using UnityEngine;

public class ResetTiles : MonoBehaviour
{

    public Animator button;
    public Transform[] wallParents;
    public RowInstaller[] rowTriggers;

    public AudioClip resetButton;
    public AudioSource resetButtonSource;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["open"].ToString());

            if (Input.GetKeyDown(ControlsConstants.keys["open"]))
            {
                button.SetTrigger("pressed");
                ResetWalls();

                resetButtonSource.PlayOneShot(resetButton);
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

    private void ResetWalls()
    {
        for (int i = 0; i < wallParents.Length; i++)
        {
            Animator[] walls = wallParents[i].GetComponentsInChildren<Animator>();
            rowTriggers[i].SetLockedIn(false);
            for (int j = 0; j < walls.Length; j++)
            {
                walls[j].SetBool("isOpen", false);

                //AudioSource wallAudio = walls[j].GetComponent<AudioSource>();

                //wallAudio.pitch += 0.4f;
                //wallAudio.PlayOneShot(tileLower);
                
            }
        }
    }
}
