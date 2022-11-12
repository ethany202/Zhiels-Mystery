using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class RemyNPC : MonoBehaviour
{

    private string[] animations = { "isIdle", "isWalking" };

    public Transform destination;
    public NavMeshAgent self;
    public Animator anim;

    public CharacterState state;


    public bool isBusy;

    void Start()
    {
        state = CharacterState.Idle;
        isBusy = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("pressed");
            isBusy = !isBusy;
        }

        if (isBusy)
        {
            state = CharacterState.Busy;
        }
        else
        {
            state = CharacterState.Idle;
        }

        ChooseAction();
    }

    public void ChooseAction()
    {
        switch (state)
        {
            case CharacterState.Idle:
                RemainIdle();
                break;
            case CharacterState.Busy:
                Move(destination);
                break;
        }
    }

    public void Move(Transform destination)
    {
        self.SetDestination(destination.position);
        anim.SetBool("isWalking", true);
    }


    public void RemainIdle()
    {
        self.SetDestination(transform.position);
        ResetAnimations("isIdle");
        
    }

    public void ResetAnimations(string exception)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            if (exception == animations[i])
            {
                anim.SetBool(exception, true);
            }
            else
            {
                anim.SetBool(animations[i], false);
            }
        }
    }


    public void ChangeDestination(Transform trans)
    {
        destination = trans;
    }
    /*public void CheckSit()
    {
        if (Input.GetKey(KeyCode.T)&&!sitting) 
        {
            RaycastHit hit;
            if (Physics.Raycast(behindCam.transform.position, behindCam.transform.forward, out hit))
            {
                if (hit.collider.tag == "Chair")
                {
                    //animator.avatar = avatars[2];
                    animator.Play(StoredAnimations.clips["Chair Sitting"]);
                    sitting = true;
                    Debug.Log("Chair");
                }
                if(hit.collider.tag == "Ground")
                {
                    //animator.avatar = avatars[4];
                    animator.Play(StoredAnimations.clips["Ground Sitting"]);
                    Debug.Log("Ground Sitt");
                    sitting = true;
                }
                    
            }
            
        }
        if (Input.GetKey(KeyCode.T) && sitting)
        {
            //animator.avatar = avatars[0];
            animator.Play(StoredAnimations.clips["Idle"]);
            sitting = false;
        }
        
    }*/
}

public enum CharacterState
{
    Idle,
    Busy
}