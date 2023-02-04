using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public GameObject camera3;

    public GameObject spawnMask;
    public GameObject character;
    public GameObject firstCar;
    public GameObject funkyMusic;

    public GameObject tvCanvas;

    public Transform pos;

    [Header("TV Clip")]
    public AudioSource tvSource;
    public AudioClip messageNewton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ActivateCamera2()
    {
        camera2.SetActive(true);
        camera1.SetActive(false);
    }

    private IEnumerator DelayDeactivate(GameObject obj, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        obj.SetActive(false);
    }

    private void ActivateCamera3()
    {
        camera3.SetActive(true);
        firstCar.SetActive(false);
        StartCoroutine(DelayDeactivate(camera2, 0.025f));
    }

    private void PlayNewtonAudio()
    {
        tvCanvas.SetActive(true);
        tvSource.PlayOneShot(messageNewton);
    }

    private void ActivateSpawnMask()
    {
        character.SetActive(true);
        //camera3.SetActive(false);
        StartCoroutine(DelayDeactivate(camera3, 0.025f));
        funkyMusic.SetActive(false);
    }

    private void DeactivateTV()
    {
        tvCanvas.SetActive(false);
        tvSource.Pause();
    }

    public void CreatePlayer()
    {
        string choosenCharacter = CustomizedData.GetCharacterName();
        Instantiate(Resources.Load(choosenCharacter, typeof(GameObject)), pos.position, Quaternion.identity);
    }
}
