using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject light;
    private bool isOn = false;


    [Range(1f, 10f)]
    public float maxRange;

    private void Start()
    {
        FlickerLights();
    }

    private void FlickerLights()
    {
        isOn = !isOn;
        light.SetActive(isOn);
        Invoke("FlickerLights", Random.Range(0.01f, maxRange));
    }
}
