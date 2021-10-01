using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{

    private SoundManager soundManager;
    public AudioSource audioSource;
    private int floorId = -1;

    public GameObject bulletDecal;
    public Camera gunCam;
    
    void Start()
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stone")
        {
            floorId = 0;
        }
        else
        {
            floorId = -1;
        }
    }

    private void Step()
    {
        soundManager.PlayMovingEffect(audioSource, floorId);
    }

    private void HandgunShoot()
    {
        soundManager.PlayShootingEffect(audioSource, 0);
        /*RaycastHit hit;
        if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, 100f))
        {
            
            GameObject newBulletDecal = Instantiate(bulletDecal, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            

            Quaternion currentRot = newBulletDecal.GetComponent<Transform>().rotation;
            
            currentRot.x = 90f;
            Debug.Log(currentRot);
            newBulletDecal.GetComponent<Transform>().rotation=currentRot;
        }*/
    }
}
