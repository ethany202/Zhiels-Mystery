using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;

    void Start()
    {
        
    }

    void Update()
    {
        transform.localPosition = player.localPosition + offset;
    }
}
