using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource source;

    [Header("Audio Clips")]
    public AudioClip footSteps;
    public AudioClip slideSFX;

    private void WalkingSFX()
    {
        source.PlayOneShot(footSteps);
    }

    private void SlideSFX()
    {
        source.PlayOneShot(slideSFX);
    }
}
