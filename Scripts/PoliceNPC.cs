using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceNPC : MonoBehaviour, INPCTemplate
{

    public Animator animator;

    public Camera policeHead;

    public Transform[] allDestinations;

    public NavMeshAgent agent;

    private int index;
    private PoliceState currentState;
    private Transform currentDest;

    public LayerMask layerMask;
    public Transform groundCheck;

    //private float gravity;
    private int health = 100;

    void Awake()
    {
        agent.SetAreaCost(0, 2f);
        //agent.SetAreaCost(5)
        currentState = PoliceState.Patrol;
    }

    void Update()
    {
        switch (currentState)
        {
            case PoliceState.Idle:
                ResetAnimations();
                agent.isStopped = true;
                break;
            case PoliceState.Patrol:
                agent.isStopped = false;
                Patrol();
                break;
            case PoliceState.Pursuit:
                agent.isStopped = false;
                Chase();
                break;
        }
    }

    void Patrol()
    {
        if (currentDest == null)
        {
            index = 0;
            currentDest = allDestinations[index];
            agent.SetDestination(currentDest.position);

            ResetAnimations();
            animator.SetBool(AnimationParameters.parameters["isWalking"], true);
        }
        else
        {
            if (agent.remainingDistance <= 1f)
            {
                if (index == allDestinations.Length - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
                currentDest = allDestinations[index];
                agent.SetDestination(currentDest.position);
            }
        }
        Watch();
    }

    private void Watch()
    {
        //for (int i = 0; i < LoadSceneLogic.criminals.Count; i++)
        //{
        //    Transform currentCorpse = LoadSceneLogic.criminals[i].transform;
        //    Vector3 viewPos = policeHead.WorldToViewportPoint(currentCorpse.position);

        //    if (viewPos.x > 0f && viewPos.y > 0 && viewPos.z > 0)
        //    {
        //        currentDest = currentCorpse;
        //        agent.SetDestination(currentDest.position);

        //        ResetAnimations();
        //        animator.SetBool(AnimationParameters.parameters["isRunning"], true);

        //        currentState = PoliceState.Pursuit;

        //        return;
        //    }
        //}
    }
    
    private void Chase()
    {
        if (agent.remainingDistance == 1f)
        {
            ResetAnimations();
            animator.SetBool(AnimationParameters.parameters["isIdle"], true);
        }
        else
        {
            ResetAnimations();
            animator.SetBool(AnimationParameters.parameters["isRunning"], true);
        }
    }


    private void ResetAnimations()
    {
        animator.SetBool(AnimationParameters.parameters["isIdle"], false);
        animator.SetBool(AnimationParameters.parameters["isRunning"], false);
        animator.SetBool(AnimationParameters.parameters["isWalking"], false);
    }

    public void SetHealth(int newHealth)
    {
        this.health = newHealth;
        if (this.health <= 0)
        {
            ResetAnimations();
            animator.SetTrigger(Animator.StringToHash("death"));
            agent.isStopped = true;
        }
    }

    public int GetHealth()
    {
        return this.health;
    }

    public string GetTargetName()
    {
        return "";
    }
}
enum PoliceState
{
    Idle,
    Patrol,
    Pursuit
}
