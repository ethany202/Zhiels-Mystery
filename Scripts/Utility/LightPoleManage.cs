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
        if(time.GetTimeOfDay()<=7.5f || time.GetTimeOfDay() >= 16f)
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
