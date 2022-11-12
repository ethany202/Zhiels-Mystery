using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemBehavior : MonoBehaviour
{
    public ParticleSystem booster;
    public DriveCar driverSeat;
    bool isActivated;

    void Start()
    {
        isActivated = false;
    }


    void Update()
    {
        if (driverSeat.VerifyInCar())
            isActivated = true;
        else
            isActivated = false;
        if (isActivated)
            ShowBooster();
    }
    private void ShowBooster()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (!booster.isPlaying)
                {
                    booster.Play();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.W))
        {
            if (booster.isPlaying)
            {
                booster.Stop();
            }
        }
        
    }
}
