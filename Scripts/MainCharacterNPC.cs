using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainCharacterNPC : MonoBehaviour
{

    public NavMeshAgent agent;
    
    public Animator anim;
    public GameObject minion;

    public Transform finalDest;

    private void Start()
    {
        agent.SetDestination(finalDest.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Minion Surprise")
        {
            minion.SetActive(true);
        }

    }
}
