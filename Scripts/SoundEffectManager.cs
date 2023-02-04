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

    private float originalPitch;

    private void Start()
    {
        originalPitch = source.pitch;
    }

    private void ResetSFX()
    {
        source.Stop();
    }

    private void WalkingSFX()
    {
        source.PlayOneShot(footSteps);
    }

    private void IncreasePitch()
    {
        source.pitch = originalPitch + 0.25f;
    }

    private void ResetPitch()
    {
        source.pitch = originalPitch;
    }

    private void SlideSFX()
    {
        source.pitch = 1f;
        source.PlayOneShot(slideSFX);
    }
}
