using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    //public AudioSource walkingOnStone1, walkingOnStone2, walkingOnStone3;
    public AudioClip[] movingOnStoneSFX = new AudioClip[3];

    public AudioClip handgunSFX;

    // floorId = 0 -> stone
    public void PlayMovingEffect(AudioSource src, int floorId)
    {
        if(floorId == 0)
        {
            AudioClip current = movingOnStoneSFX[Random.Range(0, movingOnStoneSFX.Length-1)];
            src.PlayOneShot(current);
        }
    }

    public void PlayShootingEffect(AudioSource src, int gunId)
    {
        if (gunId == 0)
        {
            src.PlayOneShot(handgunSFX); 
        }
    }

    public void PlayPersonalSFX(AudioSource src, AudioClip clip)
    {
        src.PlayOneShot(clip);
    }
}
