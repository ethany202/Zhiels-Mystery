using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class SWAT : MonoBehaviour, INPCTemplate
{
    [Header("Movement Components")]
    public Animator anim;
    public Camera swatHead;
    public NavMeshAgent agent;

    [Header("Misc.")]
    public PvELevelManager levelManager;
    public GameObject gun;
    public VisualEffect muzzleVFX;
    public AudioSource gunSource, bodySource;
    public AudioClip handgunSFX;
    public AudioClip runningSFX;
    public AudioClip[] groanSFX;
    public string targetName;

    [Header("Shooting Objects")]
    public GameObject bulletDecal;
    public GameObject bloodEffect;
    public float aimStationary = 0.85f;
    public float aimMoving = 0.3f;

    private Transform currentDest;
    private int health = 100;

    void Update()
    {
        if (this.health > 0)
        {
            Patrol();
        }
    }

    private void Running()
    {
        bodySource.PlayOneShot(runningSFX);
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
            DetermineFiring();
        }
    }

    void DetermineFiring()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool(AnimationParameters.parameters["isRunning"], false);
            anim.SetBool("isFiring", true);
        }
        else
        {
            anim.SetBool(AnimationParameters.parameters["isRunning"], true);
            anim.SetBool("isFiring", false);
        }
        
        Vector3 forward = currentDest.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(forward);
        rot.z = rot.x = 0;
        transform.localRotation = rot;
    }

    private void GunFire()
    {
        muzzleVFX.Play();
        gunSource.PlayOneShot(handgunSFX);

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
                GetComponent<CapsuleCollider>().enabled=false;

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
