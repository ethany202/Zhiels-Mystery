using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleFadeScript : MonoBehaviour
{

    private Animator title;

    private void Awake()
    {
        title = GetComponent<Animator>();
        title.SetBool("isFading", true);
    }


}
