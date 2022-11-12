using UnityEngine;

public class EnterVehicleSinglePlayer : MonoBehaviour
{
    public GameObject thisCharacter;
    private VehicleData doorData;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Vehicle")
        {
            doorData = other.GetComponent<VehicleData>();
            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText(ControlsConstants.keys["open"].ToString());
        }
        else
        {
            return;
        }


        if (doorData.IsOccupied())
        {
            return;
        }
        if (Input.GetKeyDown(ControlsConstants.keys["open"]))
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
                EnterDooredVehicle();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Vehicle")
        {
            doorData = null;
            LoadSceneLogic.DisplayInstructions(false);
        }
    }


    private void EnterDooredVehicle()
    {
        doorData.SetIsOccupied(true);
        doorData.SetCharacter(thisCharacter);

        if (doorData.GetIsDriverSeat())
        {
            doorData.ActivateVehicleScript();
            thisCharacter.GetComponent<SinglePlayerMove>().enabled = false;
        }
        doorData.ActivateVehicleCam();
    }
}
