using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMouseLook : MonoBehaviour
{
    // Fields
    public Camera cam;
    public Transform player;
    public float normalSensitivity = 45f;
    public float sensitivity;
    float xRotation = 0f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Rotate()
    {
        sensitivity = normalSensitivity;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // Right/left motion
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // forward/backward motion
        xRotation -= mouseY;                                // xRotation represents looking along the x-axis(up/down vision)
        xRotation = Mathf.Clamp(xRotation, -65f, 65f);      // Clamps movement of the mouse by preventing it from going beyond -90 and 90 degrees

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);      // Local rotation moves the local xyz position
        player.Rotate(Vector3.up * mouseX);     // Rotates player

    }

    public void SetVision(float farView)
    {
        cam.farClipPlane = farView;
    }

    public void SetSensitivity(int val)
    {
        normalSensitivity = val;
    }

}
