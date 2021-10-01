using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float buoyancy;

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water")
        {
            rigidbody.AddForce(new Vector3(0f, buoyancy * 9.81f, 0f));
        }
    }

}
