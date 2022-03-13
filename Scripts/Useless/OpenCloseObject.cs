using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseObject : MonoBehaviour
{
    public GameObject instructions;
    public Animator anim;

    void Start()
    {
        instructions.GetComponentInChildren<Text>().text = ControlsConstants.keys["open"].ToString();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            instructions.SetActive(true);
            if (Input.GetKeyDown(ControlsConstants.keys["open"]))
            {
                anim.SetTrigger("OpenClose");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            instructions.SetActive(false);
        }
    }

    public GameObject GetInstructionsObject()
    {
        return instructions;
    }
}
