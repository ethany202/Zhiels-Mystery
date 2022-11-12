using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInteraction : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent agent;

    public Camera headPosition;

    private Transform currentDest;

    void Awake()
    {
        //SceneReferences.criminals.Add(GameObject.FindWithTag("Criminal").transform);
    }

    void Update()
    {
        if (currentDest != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            ResetAnimations();
            anim.SetBool("isFiring", true);
        }
        else if(agent.remainingDistance>agent.stoppingDistance)
        {
            ResetAnimations();
            anim.SetBool("isRunning", true);
        }
        //for (int i = 0; i < LoadSceneLogic.criminals.Count; i++)
        //{
        //    Transform criminal = LoadSceneLogic.criminals[i].transform;
        //    Vector3 viewPos = headPosition.WorldToViewportPoint(criminal.position);

        //    if (viewPos.x > 0f && viewPos.y > 0 && viewPos.z > 0)
        //    {
        //        currentDest = criminal;
        //        agent.SetDestination(currentDest.position);


        //        Vector3 forward = criminal.position - transform.position;
        //        //forward.y = transform.position.y;
        //        Quaternion rot = Quaternion.LookRotation(forward);
        //        rot.z = rot.x = 0;
        //        transform.localRotation = rot;

        //        return;
        //    }
        //}
    }

    private void ResetAnimations()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isFiring", false);
    }
}
