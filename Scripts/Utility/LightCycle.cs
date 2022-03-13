using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCycle : MonoBehaviour
{

    // DayCycleController gameobject
    public DayCycleController time;

    public GameObject[] lights;
    public GameObject[] shells;
    private bool lightsOn;

    void Update()
    {
        if (time.GetTimeOfDay() <= 7.5f || time.GetTimeOfDay() >= 16f)
        {
            if (!lightsOn)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].SetActive(true);
                    shells[i].SetActive(false);
                }
                lightsOn = !lightsOn;
            }
            
        }
        else
        {
            if (lightsOn)
            {
                for (int i = 0; i < shells.Length; i++)
                {
                    shells[i].SetActive(true);
                    lights[i].SetActive(false);
                }
                lightsOn = !lightsOn;
            }
        }
    }
}
