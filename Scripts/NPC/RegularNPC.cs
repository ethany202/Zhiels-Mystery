using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RegularNPC : MonoBehaviour
{

    public Transform[] allLocations;
    public Transform currentTarget;

    public Animator animator;
    
    public NavMeshAgent character;
    public RegularState currentState;

    private float gravity;

    public LayerMask layerMask;
    public Transform groundCheck;

    void Start()
    {
        currentState = RegularState.Walking;
        //currentTarget = null;
    }

    void Update()
    {
        switch (currentState)
        {
            case RegularState.Walking:
                WalkToLocation();
                break;
            case RegularState.Standing:
                RemainIdle();
                break;
            case RegularState.Running:
                RunToLocation();
                break;
        }
        if(!Physics.CheckSphere(groundCheck.position, 0.3f, layerMask))
        {
            gravity -= 0.05f;
            transform.Translate(new Vector3(0f, gravity, 0f));
        }
        else
        {
            gravity = 0f;
        }
    }

    public void WalkToLocation()
    {
        if (character.remainingDistance <= 1f)
        {
            int index = Random.Range(0, allLocations.Length - 1);
            currentTarget = allLocations[index];
            character.SetDestination(currentTarget.position);
            //currentState = RegularState.Standing;
            //waitTime = 0.75f;
        }
        else
        {
            character.speed = 0.5f;
            animator.SetBool(Animator.StringToHash("isWalking"), true);
        }
        
    }

    public void RemainIdle()
    {
        animator.SetBool(Animator.StringToHash("isWalking"), false);
        animator.SetBool(Animator.StringToHash("isRunning"), false);
        //character.SetDestination(null);
    }

    public void RunToLocation()
    {
        animator.SetBool(Animator.StringToHash("isRunning"), true);
        character.speed = 1f;
    }
}

public enum RegularState
{
    Walking,
    Standing,
    Running
}
