using System.Collections;
using UnityEngine;

public class TPSMouseLook : MonoBehaviour
{

    [Header("Camera Movement Variables")]
    public Camera cam;
    public Transform player;
    public float normalSensitivity = 35f;
    public float xRotation = 0f;

    [Header("Camera Shake Variables")]
    public float duration = 1f;
    public AnimationCurve curve;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime; // Right/left motion
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime; // forward/backward motion
        xRotation -= (mouseY * normalSensitivity);                                // xRotation represents looking along the x-axis(up/down vision)
        xRotation = Mathf.Clamp(xRotation, -55f, 70f);      // Clamps movement of the mouse by preventing it from going beyond -90 and 90 degrees
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX * normalSensitivity);     // Rotates player
    }

    public void SetVision(float farView)
    {
        cam.farClipPlane = farView;
    }

    public void SetNormalSensitivity(float val)
    {
        normalSensitivity = val;
    }

    public IEnumerator ShakeCamera()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }

}
