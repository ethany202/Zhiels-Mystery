using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalSceneNPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform destination;

    public GameObject minion;

    private void Start()
    {
        agent.SetDestination(destination.position);
        Debug.Log("Player Created");

        Invoke("EnableMinion", 6.1f);
    }

    private void EnableMinion()
    {
        minion.SetActive(true);
    }
    // TODO: Play character Animations
}
