using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public AudioSource room;
    public AudioSource source;
    public AudioClip bgm;
    public AudioClip footsteps;

    public GameObject scene, endCreds;
    public GameObject player;

    public GameObject oldCam, newCam;

    public bool isMoving = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void StartCutscene()
    {
        player.GetComponent<Animator>().SetTrigger("startScene");
        Invoke("StartMoving", 3.25f);

        source.PlayOneShot(bgm);

        StartCoroutine(EndScene(bgm.length));
        StartCoroutine(SwitchCameras((bgm.length / 3) * 2));
    }

    public void StartMoving()
    {
        isMoving = true;
        player.GetComponent<Animator>().SetBool("isWalking", true);
       
        player.GetComponent<AudioSource>().Play();
        room.Play();
    }

    private IEnumerator SwitchCameras(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        oldCam.SetActive(false);
        newCam.SetActive(true);
    }

    private IEnumerator EndScene(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        scene.SetActive(false);
        endCreds.SetActive(true);
    }
}
