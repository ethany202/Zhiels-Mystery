using UnityEngine;


public class VehicleData : MonoBehaviour
{

    public bool hasDoors;
    public bool isDriverSeat;
    private bool doorOpen;
    private bool isOccupied;

    public GameObject vehicleBody;
    public Animator doorAnim;

    public GameObject carCamera;

    private GameObject characterOccupied;

    public GameObject hiddenPlayerBody;
    //public Animator hiddenPlayerAnim;

    void Update()
    {
        if (isOccupied && characterOccupied != null)
        {
            PlayerInput();
        }
    }

    public void PlayerInput()
    {
        if (Input.GetKeyDown(ControlsConstants.keys["open"]))
        {
            //driverDoorAnim.SetTrigger(Animator.StringToHash("toggle"));
            ToggleDoor();
        }
        if (IsDoorOpen())
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerExit();

            }
        }
    }


    // Replace ActiveCharacterGlobally with RaiseEvent methods


    private void PlayerExit()
    {
        characterOccupied.transform.position = hiddenPlayerBody.transform.position;
        characterOccupied.SetActive(true);
        characterOccupied.GetComponent<SinglePlayerMove>().enabled = true;

        characterOccupied = null;

        carCamera.SetActive(false);
        if (isDriverSeat)
        {
            vehicleBody.GetComponent<CarController>().enabled = false;
        }
        LoadSceneLogic.DisplayInstructions(false);

        isOccupied = false;
    }


    public void SetCharacter(GameObject character)
    {
        characterOccupied = character;
    }

    public void ToggleDoor()
    {
        doorOpen = !doorOpen;
        doorAnim.SetBool(Animator.StringToHash("doorOpen"), doorOpen);
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void SetIsOccupied(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }

    public bool IsDoorOpen()
    {
        return doorOpen;
    }

    public bool HasDoors()
    {
        return hasDoors;
    }

    public bool GetIsDriverSeat()
    {
        return isDriverSeat;
    }

    public void ActivateVehicleScript()
    {
        vehicleBody.GetComponent<CarController>().enabled = true;
        vehicleBody.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void DeactivateVehicleScript()
    {
        vehicleBody.GetComponent<CarController>().enabled = false;
        vehicleBody.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void ActivateVehicleCam()
    {
        carCamera.SetActive(true);
    }

    public void DeactivateVehicleCam()
    {
        carCamera.SetActive(false);
    }

}
