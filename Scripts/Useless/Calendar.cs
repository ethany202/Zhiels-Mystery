using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour
{

    public DayCycleController dayController;
    public int dayOfWeek = 1;
    public bool dayCheckCalled = false;


    // Update is called once per frame
    void Update()
    {
        CheckDay();
    }
    private void CheckDay()
    {
        while (dayController.timeOfDay > 23 && !dayCheckCalled)
        {
            if (dayOfWeek == 7) dayOfWeek = 1;
            else dayOfWeek++;
            dayCheckCalled = true;
        }
        if (dayController.timeOfDay >= 0 && dayController.timeOfDay <= 1)
        {
            dayCheckCalled = false;
        }
    }
}
