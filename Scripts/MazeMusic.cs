using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MazeMusic : MonoBehaviour
{
    //public GameObject musicSource;
    //public Transform ceilingTransform;

    //public Animator topCinemaBar;
    //public Animator botCinemaBar;

    //public GameObject endRoom;

    //private bool reloadingScene = false;

    //private void Update()
    //{
    //    DetectCeilingLocation();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        musicSource.SetActive(true);
    //    }
    //    InvokeRepeating("LowerCeiling", 60f, 60f);
    //}

    //private void LowerCeiling()
    //{
    //    StartCoroutine(LowerWithTime());
    //}

    //private IEnumerator LowerWithTime()
    //{
    //    for (int i = 1; i <= 15; i++)
    //    {
    //        ceilingTransform.Translate(new Vector3(0f, 0.01f, 0f));
    //        yield return new WaitForSecondsRealtime(0.05f);
    //    }
    //}

    //private void DetectCeilingLocation()
    //{
    //    if (ceilingTransform.position.y <= -0.8f)
    //    {
    //        BeginCutscene();
    //    }
    //}

    //private void BeginCutscene()
    //{
    //    topCinemaBar.SetTrigger("BeginCutscene");
    //    botCinemaBar.SetTrigger("BeginCutscene");
        
    //    LoadSceneLogic.player.GetComponent<Animator>().SetBool("fallingDown", true);
    //    LoadSceneLogic.player.baseSpeed = 0f;
    //    LoadSceneLogic.player.sprintSpeed = 0f;
    //    //LoadSceneLogic.player.GetComponentInChildren<TPSMouseLook>().xRotation = -55f;
    //    //LoadSceneLogic.player.enabled = false;
    //    endRoom.SetActive(true);

    //    StartCoroutine(ReloadScene());
    //    reloadingScene = true;
    //}

    //private IEnumerator ReloadScene()
    //{
    //    if (reloadingScene)
    //    {
    //        yield break;
    //    }
    //    yield return new WaitForSecondsRealtime(2f);
    //    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    //}



}
