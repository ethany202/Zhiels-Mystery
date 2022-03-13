using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using Steamworks;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    public void SetDefaultKeys()
    {
        Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>();

        //mutable keybinds:
        controls.Add("sprint", KeyCode.LeftControl);
        controls.Add("crouch", KeyCode.LeftShift);
        controls.Add("jump", KeyCode.Space);
        controls.Add("open", KeyCode.E);
        controls.Add("grab", KeyCode.G);
        controls.Add("drop", KeyCode.Z);
        controls.Add("targetData", KeyCode.Alpha1);
        controls.Add("slide", KeyCode.F);

        // immutable keys:
        controls.Add("forward", KeyCode.W);
        controls.Add("backward", KeyCode.S);
        controls.Add("right", KeyCode.D);
        controls.Add("left", KeyCode.A);
        controls.Add("attack", KeyCode.Mouse0);
        controls.Add("scope", KeyCode.Mouse1);

        ControlsConstants.keys = controls;
    }


    void Start()
    {
        SetDefaultKeys();
        ConnectToPhoton();
    }

    public void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
    }

    public void JoinTestingScene()
    {
        PhotonNetwork.JoinOrCreateRoom("StorySparkies", new RoomOptions(), null, null);
    }

    public override void OnJoinedRoom()
    {

        PhotonNetwork.LoadLevel(1);
    }


}
