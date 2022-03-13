using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class VehicleEnterData : MonoBehaviourPunCallbacks, IOnEventCallback
{

    private const byte PlayerLeftCar = 10;

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
        if (isOccupied && characterOccupied!=null)
        {
            PlayerInput();
        }
    }

    public void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

        characterOccupied.GetComponent<MoveCharacter>().enabled = true;
        characterOccupied.GetComponent<DeactivateMovement>().enabled = true;

        doorAnim.SetBool(Animator.StringToHash("isOccupied"), false);

        characterOccupied = null;

        carCamera.SetActive(false);
        if (isDriverSeat)
        {
            vehicleBody.GetComponent<CarController>().enabled = false;
        }

        SendPlayerLeft();
    }

    private void SendPlayerLeft()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(PlayerLeftCar, characterOccupied.name, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SetCharacter(GameObject character)
    {
        characterOccupied = character;
    }

    public void ToggleDoor()
    {
        Debug.Log("Toggle Door");
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
        doorAnim.SetBool(Animator.StringToHash("isOccupied"), this.isOccupied);
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
    }

    public void DeactivateVehicleScript()
    {
        vehicleBody.GetComponent<CarController>().enabled = false;

    }

    public void ActivateVehicleCam()
    {
        carCamera.SetActive(true);
    }

    public void DeactivateVehicleCam()
    {
        carCamera.SetActive(false);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PlayerLeftCar)
        {
            string playerName = (string)photonEvent.CustomData;
            GameObject.Find(playerName).SetActive(true);
        }
        
    }
}
