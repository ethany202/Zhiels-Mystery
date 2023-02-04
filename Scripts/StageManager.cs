using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject endStage;
    public GameObject fullScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            endStage.SetActive(true);

            if (fullScene != null)
            {
                fullScene.SetActive(false);

            }
            // StartCoroutine(LateLoadScene());
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private IEnumerator LateLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
