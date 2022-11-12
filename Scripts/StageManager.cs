using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject endStage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            endStage.SetActive(true);
            StartCoroutine(LateLoadScene());
        }
    }

    private IEnumerator LateLoadScene()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
