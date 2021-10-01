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

    public int size = 6;

    void Update()
    {
        if (time.GetTimeOfDay() <= 4.5f || time.GetTimeOfDay() >= 20f)
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
