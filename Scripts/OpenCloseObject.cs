using System.Collections;
using UnityEngine;

public class OpenCloseObject : MonoBehaviour
{
    public Animator anim;
    public bool lockedDoor = true;

    public BombDefuse bomb;
    public LightSwitchManager switchManager;

    private const string VALID_KEY_ID = "C4";

    private CharacterManager playerBody; 

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerBody = other.GetComponent<CharacterManager>();

            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["open"].ToString());
            if (Input.GetKeyDown(ControlsConstants.keys["open"]))
            {
                if (lockedDoor)
                {
                    ManageKeyAttempt(other);
                }
                else
                {
                    //anim.SetTrigger("OpenClose");
                    anim.SetBool("isOpen", !anim.GetBool("isOpen"));
                    bomb.SetDefusable(true);
                }
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

    private void ManageKeyAttempt(Collider other)
    {
        PhysicalKeyProperties physicalKey = playerBody.GetComponentInChildren<PhysicalKeyProperties>(true);
        if (physicalKey != null)
        {
            if (physicalKey.GetKeyID() == VALID_KEY_ID)
            {
                //anim.SetTrigger("OpenClose");
                anim.SetBool("isOpen", !anim.GetBool("isOpen"));
                bomb.SetDefusable(true);

                switchManager.stagePassed = true;            
            }
            Debug.Log(physicalKey.GetKeyID());

        }
    }
}
