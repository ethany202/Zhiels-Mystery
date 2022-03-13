using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{

    private SoundManager soundManager;
    public AudioSource audioSource;
    private int floorId = -1;

    
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
    }
}
