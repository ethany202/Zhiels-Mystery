using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public GameObject camera3;

    public GameObject spawnMask;
    public GameObject character;
    public Transform pos;

    private void ActivateCamera2()
    {
        camera2.SetActive(true);
        //camera1.SetActive(false);
        StartCoroutine(DelayDeactivate(camera1));
    }

    private IEnumerator DelayDeactivate(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        obj.SetActive(false);
    }

    private void ActivateCamera3()
    {
        camera3.SetActive(true);
        //camera2.SetActive(false);
        StartCoroutine(DelayDeactivate(camera2));

    }

    private void ActivateSpawnMask()
    {
        //spawnMask.SetActive(true);
        character.SetActive(true);
        //CreatePlayer();
        camera3.SetActive(false);
        //StartCoroutine(DelayDeactivate(camera1));

    }

    public void CreatePlayer()
    {
        string choosenCharacter = CustomizedData.GetCharacterName();
        Instantiate(Resources.Load(choosenCharacter, typeof(GameObject)), pos.position, Quaternion.identity);
    }
}
