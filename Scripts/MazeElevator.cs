using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeElevator : MonoBehaviour
{

    public Animator elevatorDoors;

    private bool stageCompleted = false;
    private bool canOperate = true;

    public AudioSource elevatorAudio;
    public AudioClip closeElevatorDoor;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            elevatorDoors.SetBool(Animator.StringToHash("openElevator"), true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterManager>() != null)
        {
            if (stageCompleted && canOperate)
            {
                elevatorDoors.SetBool(Animator.StringToHash("openElevator"), true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterManager>() != null)
        {
            LowerDoor();
        }
    }

    public void LowerDoor()
    {
        elevatorDoors.SetBool(Animator.StringToHash("openElevator"), false);
        //elevatorAudio.Stop();
        elevatorAudio.PlayOneShot(closeElevatorDoor);
    }

    public void ChangeOperate(bool operate)
    {
        canOperate = operate;
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
