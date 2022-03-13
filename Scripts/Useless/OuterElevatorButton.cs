using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterElevatorButton : MonoBehaviour
{
    public ElevatorMovement elevatorPosition;
    public int floorLevel;

    public GameObject pressedButton, arrivedButton;

    public GameObject instructions;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            #if !UNITY_EDITOR
            if (Input.GetKey(ControlsConstants.keys["open"]))
            {
                CallMovement();
            }
            #endif
            #if UNITY_EDITOR
            if(Input.GetKey(KeyCode.E))
            {
                CallMovement();
            }
            #endif
            instructions.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            instructions.SetActive(false);
        }
    }

    public void CallMovement()
    {
        if (floorLevel <= elevatorPosition.GetLevel() && elevatorPosition.GetDirection() != 1)
        {
            Action(-1);
        }
        if (floorLevel > elevatorPosition.GetLevel() && elevatorPosition.GetDirection() != -1)
        {
            Action(1);
        }
    }

    private void Action(int dir)
    {
        int distance = elevatorPosition.GetLevel() - floorLevel;
        elevatorPosition.SetMoving(true);

        pressedButton.SetActive(true);
        arrivedButton.SetActive(false);
        
        StartCoroutine(elevatorPosition.MoveElevator(distance, 0.03f * dir * 0.34f));
        SetDefault();


        pressedButton.SetActive(false);
        arrivedButton.SetActive(true);
    }

    private void SetDefault()
    {
        elevatorPosition.SetLevel(floorLevel);
        elevatorPosition.SetMoving(false);
        elevatorPosition.SetDirection(0);
    }
}
