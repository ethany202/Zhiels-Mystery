using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorMovement : MonoBehaviour
{
    public MazeElevator elevatorStatus;

    public Transform finalPosition;
    public Transform currentPosition;

    public Animator elevatorDoor;
    public GameObject endStageUI;

    private bool levelCleared = false;

    public IEnumerator MoveElevator()
    {
        while (finalPosition.position.y > currentPosition.position.y)
        {
            currentPosition.Translate(Vector3.forward * 0.3f * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        elevatorDoor.SetBool(Animator.StringToHash("openElevator"), true);

        levelCleared=true;
        elevatorStatus.SetStageCompleted(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterManager>() != null)
        {
            if (levelCleared)
            {
                endStageUI.SetActive(true);
                Invoke("LoadNextScene", 4f);
            }
            else
            {
                other.transform.SetParent(currentPosition);
                StartCoroutine(MoveElevator());
            }           
        }   
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
