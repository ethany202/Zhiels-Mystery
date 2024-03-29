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


        agent.SetDestination(destination.position);
    }

    /*void Update()
    {
        if (agent.remainingDistance ==0f)
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
    }*/
}
