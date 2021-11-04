using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseObject : MonoBehaviour
{
    public GameObject instructions;
    private Animator anim;

    void Start()
    {
        #if !UNITY_EDITOR 
        instructions.GetComponentInChildren<Text>().text = ControlsConstants.keys["open"].ToString();
        #endif

        #if UNITY_EDITOR
        instructions.GetComponentInChildren<Text>().text = "E";
        #endif

        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            instructions.SetActive(true);
            #if !UNITY_EDITOR
            if (Input.GetKeyDown(ControlsConstants.keys["open"]))
            {
                anim.SetTrigger("OpenClose");
            }
            #endif
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("OpenClose");
            }
            #endif
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
