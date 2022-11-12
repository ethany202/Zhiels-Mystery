using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : Breakable
{

    public GameObject shatteredObject;
    public GameObject intactObject;

    public AudioSource bottleSource;
    public AudioClip shatterSound;

    public void ShatterObject()
    {
        shatteredObject.SetActive(true);

        base.ExplodeObject();
        bottleSource.PlayOneShot(shatterSound);

        //intactObject.SetActive(false);

        //shatteredObject.GetComponent<Rigidbody>().AddExplosionForce()
        intactObject.GetComponent<MeshRenderer>().enabled = false;
    }

}
