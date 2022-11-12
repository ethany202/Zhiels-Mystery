using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveCar : MonoBehaviour
{
    private PlayerMovement player; // Reference to the player
    private Camera playerCam;
    public Camera carCamera;
    public GameObject car;
    public GameObject instructions;

    public Transform driverSeat; // Represents the position of the driver seat(where the character will be located when driving)
    private Transform playerPosition; // Represents the position of the player currently

    bool inCar;

    void Start()
    {
        inCar = false;
    }

    void FixedUpdate()
    {
        if (player == null)
            return;
        else
            if(inCar)
                playerPosition.position = driverSeat.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            instructions.SetActive(true);
            player = other.GetComponent<PlayerMovement>();
            playerCam = other.GetComponentInChildren<Camera>();
            playerPosition = other.GetComponent<Transform>();
            if (Input.GetKeyUp(KeyCode.E))
            {
                inCar = !inCar;
                CheckLocation(inCar, player, playerCam, playerPosition);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            instructions.SetActive(false);
            player = null;
            playerCam = null;
            playerPosition = null;
        }
    }
    private void CheckLocation(bool inCar, PlayerMovement player, Camera playerCam, Transform playerPos)
    {
        if (inCar)
        {
            carCamera.enabled = true;
            playerCam.enabled = false;
            player.DisableScript();
            car.GetComponent<CarController>().enabled = true;
            playerPos.position = driverSeat.position; // If player enters car, set location of player as the driver seat
        }
        else
        {
            carCamera.enabled = false;
            playerCam.enabled = true;
            player.EnableScript();
            car.GetComponent<CarController>().enabled = false;
        }
    }

    public bool VerifyInCar()
    {
        return this.inCar;
    }
}
