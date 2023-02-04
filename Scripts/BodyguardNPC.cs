using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyguardNPC : MonoBehaviour, INPCTemplate
{

    [Header("Movement Components")]
    public Animator anim;
    public Camera swatHead;
    public NavMeshAgent agent;

    [Header("Misc.")]
    public PvELevelManager levelManager;
    public GameObject baton;

    [Header("Audio Objects")]
    public AudioSource audioSource, bodySource;
    public AudioClip runningSFX;
    public AudioClip slashSFX;
    public AudioClip slashHitSFX;
    public string targetName;

    private Transform currentDest;
    private int health = 150;

    private bool voiceLinePlayed = false;

    void Update()
    {
        if (this.health > 0)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        CharacterManager currentPlayer = LoadSceneLogic.player;

        if (currentPlayer.tag != targetName)
        {
            return;
        }
        else
        {
            currentDest = currentPlayer.transform;
            agent.SetDestination(currentDest.position);
        }

        if (currentDest != null)
        {
            DetermineAttacking();
        }
    }

    void DetermineAttacking()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool(AnimationParameters.parameters["isRunning"], false);
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool(AnimationParameters.parameters["isRunning"], true);
            anim.SetBool("isAttacking", false);
        }
        Vector3 forward = currentDest.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(forward);
        rot.z = rot.x = 0;
        transform.localRotation = rot;
    }

    void Slash()
    {
        audioSource.PlayOneShot(slashHitSFX);
        LoadSceneLogic.player.Health -= 20f;
    }

    private void Running()
    {
        bodySource.PlayOneShot(runningSFX);
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
                GetComponent<CapsuleCollider>().center -= new Vector3(0f, 100f, 0f);

                levelManager.remainingNPCs--;
                levelManager.CheckLevelComplete();

                DropBaton();
            }
        }
    }

    public int GetHealth()
    {
        return this.health;
    }

    private void ResetAnimations()
    {
        anim.SetBool(AnimationParameters.parameters["isRunning"], false);
        anim.SetBool("isAttacking", false);
    }

    private void DropBaton()
    {
        baton.GetComponent<Rigidbody>().isKinematic = false;
        baton.transform.SetParent(null);
    }

    public string GetTargetName()
    {
        return targetName;
    }
}
