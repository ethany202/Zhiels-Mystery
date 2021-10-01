using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface NPC
{   
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    string[] arr = { "isWalking", "isSprinting", "isRunningBackwards", "isJumping" };
    public NavMeshAgent person;
    public Transform workLocation;
    public bool hasWork;
    public Animator animator;
    public DoorProperties doorProperties;

    // Start is called before the first frame update
    void Start()
    {
        hasWork = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        WorkCommute();
    }
    private void CallAnimations(string animationName, bool value)
    {
        animator.SetBool(animationName, value);
    }
    private void WorkCommute()
    {
        if (hasWork)
        {
            person.SetDestination(workLocation.position);
            CallAnimations("isWalking", true);
        }
        if (person.remainingDistance == person.stoppingDistance)
        {
            for (int i = 0; i < 4; i++)
            {
                CallAnimations(arr[i], false);
            }
            //CallAnimations("isWalking", false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Door")
        {
            print("Door Ahead");
            doorProperties = other.GetComponent<DoorProperties>();
            if (person.remainingDistance != person.stoppingDistance && !doorProperties.GetIsOpen())
            {
                Animator a = other.GetComponentInChildren<Animator>();
                a.SetTrigger("OpenClose");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("Entered");
    }
}
*/
