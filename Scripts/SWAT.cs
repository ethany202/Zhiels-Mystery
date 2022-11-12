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
    public AudioSource audioSource;
    public AudioClip handgunSFX;
    public string targetName;

    [Header("Shooting Objects")]
    public GameObject bulletDecal;
    public GameObject bloodEffect;

    private Transform currentDest;
    private int health = 100;

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
        Vector3 viewPos = swatHead.WorldToViewportPoint(currentPlayer.transform.position);

        if (currentPlayer.tag != targetName)
        {
            return;
        }

        if (viewPos.x > 0f && viewPos.y > 0 && viewPos.z > 0 && viewPos.x < 1f && viewPos.y<1f)
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
        audioSource.PlayOneShot(handgunSFX);

        float hitValue = Random.Range(0.1f, 1f);

        if (hitValue <= 0.5)
        {
            Debug.Log("HIT PLAYER");

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
