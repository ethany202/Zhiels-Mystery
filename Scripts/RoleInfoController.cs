using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoleInfoController : MonoBehaviour
{

    public TMP_Text hitmanText;
    public TMP_Text detectiveText;

    public Animator hitmanInfo;
    public Animator detectiveInfo;

    void Start()
    {
        //if (LoadSceneLogic.playerRole==1)
        //{
        //    detectiveText.text = "Role: Detective";
        //}
        //else
        //{
        //    hitmanText.text = "Role: hitman";
        //}
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.F1))
        //{
        //    if (LoadSceneLogic.playerRole == 1)
        //    {
        //        detectiveInfo.SetBool("view", true);
        //    }
        //    else
        //    {
        //        hitmanInfo.SetBool("view", true);
        //    }
        //}
        //if (Input.GetKeyUp(KeyCode.F1))
        //{
        //    if (LoadSceneLogic.playerRole == 1)
        //    {
        //        detectiveInfo.SetBool("view", false);
        //    }
        //    else
        //    {
        //        hitmanInfo.SetBool("view", false);
        //    }
        //}
    }
}
