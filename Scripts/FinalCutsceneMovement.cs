using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutsceneMovement : MonoBehaviour
{
    public EndGame endGame;
    public Animator anim;
    public GameObject player;
    public Transform destination;

    public CharacterController character;

    private float gravity = 0f;
    private float speed = 0.6f;

    private void Update()
    {
        if (endGame.isMoving)
        {
            //MoveForward();
            Translate();
            AddGravity();
        }
        
    }

    private void StartMoving()
    {
        
    }

    private void Translate()
    {
        float distance = Vector3.Distance(player.transform.position, destination.position);

        if (distance > 0f)
        {
            Vector3 direction = Vector3.forward * speed; // * speed
            player.GetComponent<CharacterController>().Move(direction * Time.deltaTime);
        }
    }

    private void StopMoving()
    {
        
    }

    private void AddGravity()
    {
        if (character.isGrounded)
        {
            gravity = 0f;
        }
        else
        {
            character.Move(new Vector3(0f, gravity, 0f));
            gravity -= (0.2f * Time.deltaTime);
        }
    }

}
