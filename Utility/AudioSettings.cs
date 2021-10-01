using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{

    private static int masterVolume;
    private static int musicVolume;
    private static int sfxVolume;
    private static string soundQuality;

    public static void SetMasterVolume(int val)
    {
        masterVolume = val;
    }

    public static void SetMusicVolume(int val)
    {
        musicVolume = val;
    }

    public static void SetSFXVolume(int val)
    {
        sfxVolume = val;
    }

    public static void SetSoundQuality(string val)
    {
        soundQuality = val;
    }

    public static int GetMasterVolume()
    {
        return masterVolume;
    }

    public static int GetMusicVolume()
    {
        return musicVolume;
    }

    public static int GetSFXVolume()
    {
        return sfxVolume;
    }

    public static string GetSoundQuality()
    {
        return soundQuality;
    }
}
