using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoleManage : MonoBehaviour
{

    public GameObject pointLight;
    public GameObject spotLight;
    public DayCycleController time;

    private bool lightsOn = false;

    void Update()
    {
        if(time.GetTimeOfDay()<=4.5f || time.GetTimeOfDay() >= 20f)
        {
            if (!lightsOn)
            {
                pointLight.SetActive(true);
                spotLight.SetActive(true);
                lightsOn = !lightsOn;
            }
        }
        else
        {
            if (lightsOn)
            {
                pointLight.SetActive(false);
                spotLight.SetActive(false);
                lightsOn = !lightsOn;
            }
        }
    }
}
