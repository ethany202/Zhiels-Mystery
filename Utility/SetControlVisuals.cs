using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetControlVisuals : MonoBehaviour
{

    public Text openClose;

    void Start()
    {
        openClose.text = ControlsConstants.keys["open"].ToString();
    }


}
