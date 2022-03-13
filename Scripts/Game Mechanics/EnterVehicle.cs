using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnterVehicle : MonoBehaviour
{
    public GameObject thisCharacter;
    public Animator characterAnim;
    private VehicleEnterData doorData;
    private bool isInVehicle;

    private Vector3 outOfCarPosition;


    /*void Update()
    {
        if (isInVehicle)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                doorData.ToggleDoor();
            }
            if (doorData.IsDoorOpen())
            {
                if (Input.GetKeyDown(KeyCode.F))
                {

                }
            }
        }
    }*/

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Vehicle")
        {
            doorData = other.GetComponent<VehicleEnterData>();
            Debug.LogError("Near Car");
        }
        else
        {
            return;
        }


        if (doorData.IsOccupied())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (doorData.HasDoors())
            {
                doorData.ToggleDoor();
                return;
            }             
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (doorData.IsDoorOpen())
            {
                thisCharacter.SetActive(false);
                thisCharacter.GetComponent<PhotonView>().RPC("DeactivateCharacterGlobally", RpcTarget.OthersBuffered, thisCharacter.name);

                EnterDooredVehicle();
            }
        }
    }

    [PunRPC]
    private void DeactivateCharacterGlobally(string characterName)
    {
        GameObject character = GameObject.Find(characterName);
        character.SetActive(false);
    }


    private void EnterDooredVehicle()
    {
        doorData.SetIsOccupied(true);
        doorData.SetCharacter(thisCharacter);
        
        if (doorData.GetIsDriverSeat())
        {
            doorData.ActivateVehicleScript();
            thisCharacter.GetComponent<MoveCharacter>().enabled = false;
            thisCharacter.GetComponent<DeactivateMovement>().enabled = false;
        }
        doorData.ActivateVehicleCam();
        isInVehicle = true;
    }
}
