using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeonardNPC : MonoBehaviour, INPCTemplate
{

    [Header("Movement Components")]
    public Animator anim;
    public NavMeshAgent agent;
    public Camera cam;
    public Transform finalDestination;
    public float displacementDist = 3f;

    [Header("Misc.")]
    public PvELevelManager levelManager;
    public AudioSource audioSource;
    public string targetName;

    private int health = 5;
    private Transform character;

    void Update()
    {
        if (this.health > 0)
        {
            Patrol();
        }

        if (agent.remainingDistance ==0)
        {
            anim.SetBool("isDefending", true);
        }
    }

    private void Patrol()
    {
        CharacterManager currentPlayer = LoadSceneLogic.player;
        Vector3 viewPos = cam.WorldToViewportPoint(currentPlayer.transform.position);

        if (currentPlayer.tag != targetName)
        {
            return;
        }
        else
        {
            agent.SetDestination(finalDestination.position);
            anim.SetBool("isRunning", true);

            character = currentPlayer.transform;
            agent.baseOffset = -0.02f;
        }

        if (viewPos.x > 0f && viewPos.y > 0 && viewPos.z > 0 && viewPos.x < 1f && viewPos.y < 1f)
        {

        }
        else
        {
            FaceCharacter();
        }
    }

    private void FaceCharacter()
    {
        if (character == null) { return; }


        Vector3 forward = character.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(forward);
        rot.z = rot.x = 0;
        transform.localRotation = rot;
    }

    public void SetHealth(int newHealth)
    {
        if (this.health <= 0)
        {
            return;
        }
        else
        {
            this.health = newHealth;

            if (this.health <= 0)
            {
                ResetAnimations();
                anim.SetTrigger(Animator.StringToHash("death"));

                agent.isStopped = true;
                GetComponent<CapsuleCollider>().enabled = false;

                levelManager.remainingNPCs--;
                levelManager.CheckLevelComplete();
            }
        }
    }

    public int GetHealth()
    {
        return this.health;
    }

    private void ResetAnimations()
    {
        anim.SetBool("isDefending", false);
    }

    public string GetTargetName()
    {
        return targetName;
    }

}
