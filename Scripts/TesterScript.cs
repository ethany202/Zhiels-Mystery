using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TesterScript : MonoBehaviour
{

    public Animator anim;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("inCar", true);
        }
    }


    public void GrabObject()
    {

    }
}
