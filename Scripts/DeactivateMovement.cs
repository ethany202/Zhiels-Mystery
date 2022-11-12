using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeactivateMovement : MonoBehaviour
{

    public GameObject player;

    void Update()
    {
        InputListen();
    }

    public void InputListen()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.GetComponent<MoveCharacter>().enabled = !(player.GetComponent<MoveCharacter>().enabled);
        }
    }
}
