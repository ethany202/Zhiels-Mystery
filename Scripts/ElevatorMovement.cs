using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorMovement : MonoBehaviour
{

    [Header("Elevator Objects")]
    public MazeElevator elevatorStatus;

    public Transform finalPosition;
    public Transform currentPosition;

    public Animator elevatorDoor;
    public GameObject endStageUI;

    [Header("Audio Components")]
    public AudioSource backgroundMusic;
    public AudioSource elevatorSrc;
    public AudioClip elevatorMoving;

    public RadioController announcementElevator;

    public bool levelCleared = false;

    public IEnumerator MoveElevator()
    {
        elevatorStatus.ChangeOperate(false);

        while (finalPosition.position.y > currentPosition.position.y)
        {
            currentPosition.Translate(Vector3.forward * 0.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        elevatorDoor.SetBool(Animator.StringToHash("openElevator"), true);

        elevatorStatus.SetStageCompleted(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterManager>() != null)
        {
            if (levelCleared)
            {
                endStageUI.SetActive(true);

                StartCoroutine(FadeBGM());
                Invoke("LoadNextScene", 3.5f);
            }
            else
            {
                StartCoroutine(announcementElevator.PlayVoiceLineDelay(1.5f));

                other.transform.SetParent(currentPosition);
                elevatorSrc.PlayOneShot(elevatorMoving);

                StartCoroutine(MoveElevator());
            }           
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<CharacterManager>() != null)
        {
            elevatorStatus.LowerDoor();
        }
    }

    private IEnumerator FadeBGM()
    {
        while (backgroundMusic.volume != 0f)
        {
            backgroundMusic.volume -= 0.01f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
