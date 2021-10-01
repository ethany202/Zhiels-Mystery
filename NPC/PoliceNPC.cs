using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceNPC : MonoBehaviour
{

    private PoliceState currentState;
    public Transform dest1, dest2;

    public int destCode = 1; // 1 = dest1, 2 = dest2
    public NavMeshAgent character;

    public Animator animator;
    public Camera cam;

    private Transform chaseDest;

    private GameObject[] criminals;

    void Start()
    {
        currentState = PoliceState.Patrol;
        criminals = GameObject.FindGameObjectsWithTag("Criminal");
    }

    void Update()
    {
        switch (currentState)
        {
            case PoliceState.Idle:
                PlayIdle();
                Watch();
                break;
            case PoliceState.Patrol:
                Patrol();
                Watch();
                break;
            case PoliceState.Pursuit:
                Chase(chaseDest);
                break;
        }
    }

    void Patrol()
    {
        var destination = Vector3.zero;
        if (destCode == 1 && character.remainingDistance <= character.stoppingDistance)
        {
            destination = dest1.position;
            destCode = 2;
        }
        if(destCode==2 && character.remainingDistance<=character.stoppingDistance)
        {
            destination = dest2.position;
            destCode = 1;
        }
        //animator.SetBool("isWalking", true);
        ResetAnimations("isWalking");
        character.SetDestination(destination);
    }

    private void Watch()
    {
        for (int i = 0; i < criminals.Length; i++)
        {
            Vector3 criminalRelativePos = cam.WorldToViewportPoint(criminals[i].GetComponent<Transform>().position);
            if (criminalRelativePos.x > 0f && criminalRelativePos.y > 0f && criminalRelativePos.z > 0f && Vector3.Distance(transform.position, criminals[i].GetComponent<Transform>().position)<50f)
            {
                currentState = PoliceState.Pursuit;
                chaseDest = criminals[i].GetComponent<Transform>();
                return;
            }
        }
    }

    private void PlayIdle()
    {
        ResetAnimations();
    }
    
    private void Chase(Transform person)
    {
        character.SetDestination(person.position);
        if (character.remainingDistance >= 50f)
        {
            currentState = PoliceState.Patrol;
        }
        if(character.remainingDistance <= character.stoppingDistance)
        {
            ResetAnimations();
        }
        else
        {
            ResetAnimations("isRunning");
        }
    }

    private void ResetAnimations(string exception)
    {
        string[] parameters = { "isWalking", "isRunning" };
        for(int i = 0; i < parameters.Length; i++)
        {
            if (exception != parameters[i])
            {
                animator.SetBool(parameters[i], false);
            }
            else
            {
                animator.SetBool(parameters[i], true);
            }
        }
    }

    private void ResetAnimations()
    {
        ResetAnimations("");
    }

}
enum PoliceState
{
    Idle,
    Patrol,
    Pursuit
}
