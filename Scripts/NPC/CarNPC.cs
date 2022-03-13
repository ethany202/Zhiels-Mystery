using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarNPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform destination;

    public Transform[] destinations;
    private int index;

    void Awake()
    {
        index = 0;

        agent.SetAreaCost(0, 10f);
        agent.SetAreaCost(4, 1f);


        agent.SetDestination(destinations[index].position);
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (index == destinations.Length - 1)
            {
                agent.SetDestination(destinations[0].position);
            }
            else
            {
                index++;
            }        
            agent.SetDestination(destinations[index].position);
        }
    }
}
