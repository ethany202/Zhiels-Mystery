using UnityEngine;

public class ItemManager : MonoBehaviour
{

    [Header("Character Components")]
    public Transform grabPos;
    public Transform cam;
    public CharacterManager character;

    private Transform objectBody;

    [Header("FPS Objects")]
    public GameObject rifleArms;
    public GameObject pistolArms;
    public GameObject syringeArms;
    public GameObject firstAid;

    // Private data types
    private int syringeCount = 0;
    private bool grabbingObject = false;

    void Update()
    {
        UseSyringe();
        DropObject();
    }

    private void UseSyringe()
    {
        if (syringeCount == 0) { return; }

        if (Input.GetKeyDown(ControlsConstants.keys["open"]))
        {
            syringeArms.SetActive(true);

            rifleArms.SetActive(false);
            pistolArms.SetActive(false);
        }
    }

    public void SyringeInjected()
    {
        character.Health = Mathf.Min(character.Health + 25f, 100f);
        syringeCount--;

        if (syringeCount == 0) 
        {
            firstAid.SetActive(false);
        }

        syringeArms.SetActive(false);
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (grabbingObject)
        {
            if (objectBody.gameObject.layer == LayerMask.NameToLayer("Rifle"))
            {
                rifleArms.GetComponent<Gun>().SetGunBody(objectBody.GetComponent<ObjectProperties>());
                rifleArms.SetActive(true);
            }

            if (objectBody.gameObject.layer == LayerMask.NameToLayer("Pistol"))
            {
                pistolArms.GetComponent<Gun>().SetGunBody(objectBody.GetComponent<ObjectProperties>());
                pistolArms.SetActive(true);
            }
        }
    }

    private void GrabObject()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["grab"]))
        {
            grabbingObject = true;

            UpdatePosition();

            objectBody.gameObject.SetActive(false);
            LoadSceneLogic.DisplayInstructions(false);

            LoadSceneLogic.ChangeInstructionsTextSecondary(ControlsConstants.keys["drop"].ToString());
            LoadSceneLogic.DisplayInstructionsSecondary(true);
        }
    }

    private void GrabSyringe(GameObject physicalObj)
    {
        if (Input.GetKeyDown(ControlsConstants.keys["grab"]))
        {
            syringeCount++;
            firstAid.SetActive(true);

            LoadSceneLogic.DisplayInstructions(false);

            physicalObj.SetActive(false);
        }
        
    }

    private void GrabKey()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["grab"]))
        {
            grabbingObject = true;
            objectBody.gameObject.SetActive(false);

            PhysicalKeyProperties playerKeyCopy = cam.GetComponentInChildren<PhysicalKeyProperties>(true);
            playerKeyCopy.GetKeyUI().SetActive(true);
            playerKeyCopy.SetKeyID(objectBody.GetComponent<PhysicalKeyProperties>().GetKeyID());

            LoadSceneLogic.DisplayInstructions(false);

            LoadSceneLogic.ChangeInstructionsTextSecondary(ControlsConstants.keys["drop"].ToString());
            LoadSceneLogic.DisplayInstructionsSecondary(true);
        }
    }   

    private void DropObject()
    {
        if (grabbingObject)
        {
            if (Input.GetKeyDown(ControlsConstants.keys["drop"]))
            {
                // Set to grabbable state
                grabbingObject = false;

                // Physically releasing the object
                objectBody.position = grabPos.position;
                objectBody.gameObject.SetActive(true);
                objectBody = null;

                // Deactivate everything
                rifleArms.SetActive(false);
                pistolArms.SetActive(false);

                // Specific to Key UI only
                cam.GetComponentInChildren<PhysicalKeyProperties>(true).GetKeyUI().SetActive(false);

                LoadSceneLogic.DisplayInstructions(false);
                LoadSceneLogic.DisplayInstructionsSecondary(false);
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Object")
        {
            if (!grabbingObject)  // If player is currently not grabbing a gun
            {
                objectBody = other.GetComponent<Transform>();

                LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["grab"].ToString());
                LoadSceneLogic.DisplayInstructions(true);    
                
                GrabObject();
            }
        }
        if (other.tag == "Syringe")
        {
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["grab"].ToString());
            LoadSceneLogic.DisplayInstructions(true);

            GrabSyringe(other.gameObject);
        }
        if (other.tag=="Special Key")
        {
            objectBody = other.GetComponent<Transform>();

            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["grab"].ToString());
            LoadSceneLogic.DisplayInstructions(true);

            GrabKey();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            LoadSceneLogic.DisplayInstructions(false);
        }
    }
}
