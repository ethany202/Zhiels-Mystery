using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    private AudioSource[] audioSources;

    public void SetAllVolume()
    {
        audioSources = GameObject.FindObjectsOfType<AudioSource>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            
            if (audioSources[i].tag == "SFX")
            {
                audioSources[i].volume = AudioSettings.GetSFXVolume() / 100f;
            }
            else if (audioSources[i].tag == "Music")
            {
                audioSources[i].volume = AudioSettings.GetMusicVolume()/100f;
            }
            else
            {
                audioSources[i].volume = AudioSettings.GetMasterVolume()/100f;
            }
        }
    }
}
