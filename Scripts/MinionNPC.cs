using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinionNPC : MonoBehaviour
{

    public GameObject part1Transition;

    public AudioSource audioSource;
    public AudioClip handgunSFX;

    public GameObject scene, finalCam;

    private void SceneTransition()
    {
        part1Transition.SetActive(true);
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        scene.SetActive(false);
        finalCam.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Loading Next Scene");
        SceneManager.LoadSceneAsync(2);
    }

    private void FireBullet()
    {
        audioSource.PlayOneShot(handgunSFX);
    }
}
