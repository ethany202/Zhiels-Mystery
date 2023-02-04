using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBCScript : MonoBehaviour
{

    public GameObject tbcText;
    public bool isActive = true;

    private void Awake()
    {
        InvokeRepeating("Active", 3f, 3f);
    }

    public void Active()
    {
        isActive = !isActive;
        tbcText.SetActive(isActive);
    }
}
