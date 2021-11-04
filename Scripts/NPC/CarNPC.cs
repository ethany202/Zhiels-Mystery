using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarNPC : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform destination;

    void Start()
    {       
        agent.SetAreaCost(0, 3f);
        agent.SetAreaCost(4, 1f);
        agent.SetDestination(destination.position);
    }
}
