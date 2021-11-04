using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviourPunCallbacks
{
    public int waitingRoomSceneIndex;
    public Animator sceneTransition;


    void Start()
    {
        waitingRoomSceneIndex = 1;
    }



    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        sceneTransition.SetTrigger("FadeIn");
        SceneManager.LoadScene(waitingRoomSceneIndex);
    }

}
