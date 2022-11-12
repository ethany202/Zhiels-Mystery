using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeElevator : MonoBehaviour
{

    public Animator elevatorDoors;
    private bool stageCompleted = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            elevatorDoors.SetBool(Animator.StringToHash("openElevator"), true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CharacterManager>() != null)
        {
            if (stageCompleted)
            {
                elevatorDoors.SetBool(Animator.StringToHash("openElevator"), true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterManager>() != null)
        {
            elevatorDoors.SetBool(Animator.StringToHash("openElevator"), false);
        }
    }

    public bool IsStageCompleted()
    {
        return stageCompleted;
    }

    public void SetStageCompleted(bool stageCompleted)
    {
        this.stageCompleted = stageCompleted;
    }
}
