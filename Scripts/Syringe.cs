using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{

    public ItemManager characterItem;

    public AudioSource source;
    public AudioClip syringeSFX;

    private void SyringeInjected()
    {
        characterItem.SyringeInjected();
    }

    private void PlayEffect()
    {
        source.PlayOneShot(syringeSFX);
    }


}
