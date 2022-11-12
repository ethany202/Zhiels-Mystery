using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleController : MonoBehaviour
{
    [Range(0, 24)]
    public float timeOfDay;
    public float orbitSpeed = 0.5f;
    public Light sun;
    public Light moon;
    public bool isNight;

    void Update()
    {
        Cycle();
    }

    private void Cycle()
    {
        timeOfDay += Time.deltaTime * orbitSpeed;
        if (timeOfDay >= 24)
        {
            timeOfDay = 0;
        }
        UpdateTime();
        CheckNightDayTransition();
    }

    private void UpdateTime()
    {
        float alpha = timeOfDay / 24.0f;
        float sunRotation = Mathf.Lerp(-90, 270, alpha);
        float moonRotation = sunRotation - 180f;
        sun.transform.rotation = Quaternion.Euler(sunRotation, -150f, 0);
        moon.transform.rotation = Quaternion.Euler(moonRotation, -150f, 0);

    }
    private void CheckNightDayTransition()
    {
        if (isNight)
        {
            if (moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
            }
        }
        else
        {
            if (sun.transform.rotation.eulerAngles.x > 180)
            {
                StartNight();
            }
        }
    }
    private void StartDay()
    {
        isNight = false;
        sun.shadows = LightShadows.Soft;
        moon.shadows = LightShadows.None;
    }
    private void StartNight()
    {
        isNight = true;
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;
    }
    public float GetTimeOfDay()
    {
        return this.timeOfDay;
    }
}
