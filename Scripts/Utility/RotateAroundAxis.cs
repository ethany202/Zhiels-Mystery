using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    public GameObject target;
    private Vector3 currentRotation = new Vector3(1,0,0);
    private float rotateSpeed = 8;



    void Update()
    {
        //transform.Rotate(0, 0, 45 * Time.deltaTime * Mathf.Sin(Time.time));
        //rotateSpeed--;
        transform.RotateAround(target.transform.position, new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime * Mathf.Sin(Time.time));

    }
}
