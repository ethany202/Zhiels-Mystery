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
    public AudioSource bodySource;
    public AudioClip stepSFX;  
    public AudioClip handgunSFX;
    public AudioClip[] groanSFX;

    [Header("Shooting Objects")]
    public GameObject bulletDecal;
    public GameObject bloodEffect;
    public float aimStationary = 0.85f;
    public float aimMoving = 0.3f;

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

        float hitValue = Random.Range(0.1f, 1f);

        if ((!LoadSceneLogic.player.isMoving && hitValue <= aimStationary) || (hitValue <= aimMoving))
        {
            LoadSceneLogic.player.Health -= 10f;
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(swatHead.transform.position, swatHead.transform.forward, out hit, 50f))
            {
                Vector3 point = hit.point;
                point.x += 0.0002f;
                point.y += 0.0002f;
                point.z += 0.0002f;
                GameObject newBulletDecal = Instantiate(bulletDecal, point, Quaternion.FromToRotation(Vector3.back, hit.normal));

                Destroy(newBulletDecal, 30f);
            }
        }
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

                DropGun();
                Groan();
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

    private void Groan()
    {
        int index = Random.Range(0, groanSFX.Length);
        bodySource.PlayOneShot(groanSFX[index]);
    }
}
