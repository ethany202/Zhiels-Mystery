using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class TPSMouseLook : MonoBehaviourPunCallbacks, IOnEventCallback
{
    // Fields
    private const byte PlayerAim = 15;

    public Camera cam;
    public Transform player;
    public float normalSensitivity = 45f;
    public float scopeSensitivity = 20f;
    public float xRotation = 0f;

    public Animator animator;
    public RigBuilder playerRig;
    public Transform aimTarget;

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
        xRotation = Mathf.Clamp(xRotation, -64f, 64f);      // Clamps movement of the mouse by preventing it from going beyond -90 and 90 degrees

        if (animator.GetBool(AnimationParameters.parameters["isAiming"]))
        {
            //ActivateRig();
            playerRig.layers[0].rig.weight = 1f;
            if (Mathf.Abs(xRotation)<=64f)
            {
                aimTarget.Translate(new Vector3(0f, mouseY * scopeSensitivity * 0.5f, 0f));
                transform.localRotation = Quaternion.Euler(GetAngle2D(), 0f, 0f);
                player.Rotate(Vector3.up * mouseX * scopeSensitivity);     // Rotates player
            }
        }
        else
        {
            //DeactivateRig();
            playerRig.layers[0].rig.weight = 0f;
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);      // Local rotation moves the local xyz position
            player.Rotate(Vector3.up * mouseX * normalSensitivity);     // Rotates player
        }
    }

    private void DeactivateRig()
    {
        object[] customData = { player.name, 0f };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(PlayerAim, customData, raiseEventOptions, SendOptions.SendReliable);
    }

    private void ActivateRig()
    {
        object[] customData = { player.name, 1f };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(PlayerAim, customData, raiseEventOptions, SendOptions.SendReliable);
    }

    private float GetAngle2D()
    {
        Vector2 origin = new Vector2(transform.position.z, transform.position.y);

        Vector2 newAimPos = new Vector2(aimTarget.position.z, aimTarget.position.y);
        Vector2 horizontal = new Vector2(aimTarget.position.z, transform.position.y);

        float hypotenuse = Vector2.Distance(origin, newAimPos);
        float horizontalDist = Vector2.Distance(origin, horizontal);

        float angle = Mathf.Acos(horizontalDist / hypotenuse) * 180f / Mathf.PI;

        if (newAimPos.y > horizontal.y)
        {
            angle *= -1;
        }

        return angle;
    }

    public void SetVision(float farView)
    {
        cam.farClipPlane = farView;
    }

    public void SetNormalSensitivity(float val)
    {
        normalSensitivity = val;
    }

    public void SetScopeSensitivity(float val)
    {
        scopeSensitivity = val;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PlayerAim)
        {
            object[] customData = (object[])photonEvent.CustomData;
            if (LoadSceneLogic.scenePlayers.ContainsKey((string)customData[0]))
            {
                LoadSceneLogic.scenePlayers[(string)customData[0]].GetComponent<TPSMouseLook>().playerRig.layers[0].rig.weight = (float)customData[1];
            }

        }

    }

}
