using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class MafiaBossNPC : MonoBehaviour, INPCTemplate
{

    [Header("Movement Components")]
    public Animator anim;
    public Camera swatHead;
    public NavMeshAgent agent;

    [Header("Misc.")]
    public PvELevelManager levelManager;
    public GameObject gun;
    public VisualEffect muzzleVFX;
    
    [Header("Audio Objects")]
    public AudioSource audioSource;
    public AudioClip stepSFX;  
    public AudioClip handgunSFX;


    public string targetName;

    private Transform currentDest;
    private int health = 100;

    void Update()
    {
        if (this.health > 0)
        {
            Patrol();
        }
    }

    //Movement Method:
    private void Step()
    {
        audioSource.PlayOneShot(stepSFX);
    }

    private void Patrol()
    {
        CharacterManager currentPlayer = LoadSceneLogic.player;
        Vector3 viewPos = swatHead.WorldToViewportPoint(currentPlayer.transform.position);

        if (currentPlayer.tag != targetName)
        {
            return;
        }

        if (viewPos.x > 0f && viewPos.y > 0 && viewPos.z > 0 && viewPos.x < 1f && viewPos.y < 1f)
        {
            currentDest = currentPlayer.bodyTarget.transform;
            agent.SetDestination(currentPlayer.transform.position);
        }

        if (currentDest != null)
        {
            anim.SetTrigger("drawGun");
            agent.baseOffset = -0.02f;

            DetermineFiring();
        }
    }

    void DetermineFiring()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool(AnimationParameters.parameters["isRunning"], false);
            anim.SetBool("isShooting", true);
        }
        else
        {
            anim.SetBool(AnimationParameters.parameters["isRunning"], true);
            anim.SetBool("isShooting", false);
        }

        Vector3 forward = currentDest.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(forward);
        rot.z = rot.x = 0;
        transform.localRotation = rot;
    }

    private void GunFire()
    {
        muzzleVFX.Play();
        audioSource.PlayOneShot(handgunSFX);
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

                DropGun();
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
        anim.SetBool("isFiring", false);
    }

    private void DrawGun()
    {
        gun.SetActive(true);
    }

    private void DropGun()
    {
        gun.GetComponent<Rigidbody>().isKinematic = false;
        gun.transform.SetParent(null);
    }

    public string GetTargetName()
    {
        return targetName;
    }
}
