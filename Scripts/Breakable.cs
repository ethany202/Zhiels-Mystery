using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    public float explosionForce = 50f;
    public float explosionRadius = 2.0f;
    public float upwardsModifier = 3.0f;

    public void ExplodeObject()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach(var rb in rbs)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
        }
    }

}
