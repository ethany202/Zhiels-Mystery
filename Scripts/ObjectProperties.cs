using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{

    public float totalMags;
    public float fullMagAmmo;
    public float currentMagAmmo;

    public AudioSource bodySource;
    public AudioClip dropClip;

    public float GetTotalMags()
    {
        return totalMags;
    }

    public float GetFullMagAmmo()
    {
        return fullMagAmmo;
    }

    public float GetCurrentMagAmmo()
    {
        return currentMagAmmo;
    }

    public void SetTotalMags(float totalMags)
    {
        this.totalMags = totalMags;
    }

    public void SetFullMagAmmo(float fullMagAmmo)
    {
        this.fullMagAmmo=fullMagAmmo;
    }

    public void SetCurrentMagAmmo(float currentMagAmmo)
    {
        this.currentMagAmmo=currentMagAmmo;
    }

    private void OnCollisionEnter(Collision collision)
    {
        bodySource.PlayOneShot(dropClip);
    }

}
