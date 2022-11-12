using UnityEngine;
using UnityEngine.AI;

public class RegularNPC : MonoBehaviour, INPCTemplate
{

    public Transform[] allLocations;
    public Transform currentTarget;

    public Animator animator;
    
    public NavMeshAgent character;
    public RegularState currentState;

    //public LayerMask layerMask;
    //public Transform groundCheck;

    private int health = 100;
    int index = 0;

    void Start()
    {
        currentState = RegularState.Walking;
    }

    void Update()
    {
        switch (currentState)
        {
            case RegularState.Walking:
                WalkToLocation();
                break;
            case RegularState.Standing:
                RemainIdle();
                break;
            case RegularState.Running:
                RunToLocation();
                break;
        }
    }

    public void WalkToLocation()
    {
        if (character.remainingDistance == 0f)
        {
            index = (index == allLocations.Length - 1) ? 0 : index++;
            currentTarget = allLocations[index];
            character.SetDestination(currentTarget.position);
            //Debug.Log("arrived");
        }
        else
        {
            character.speed = 0.5f;
            animator.SetBool(AnimationParameters.parameters["isWalking"], true);
        }
        
    }

    public void RemainIdle()
    {
        animator.SetBool(AnimationParameters.parameters["isWalking"], false);
        animator.SetBool(AnimationParameters.parameters["isRunning"], false);
    }

    public void RunToLocation()
    {
        animator.SetBool(AnimationParameters.parameters["isRunning"], true);
        character.speed = 1f;
    }

    public void SetHealth(int newHealth)
    {
        this.health = newHealth;
        if (this.health <= 0)
        {
            RemainIdle();
            animator.SetTrigger(Animator.StringToHash("death"));
            character.isStopped = true;
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

public enum RegularState
{
    Walking,
    Standing,
    Running
}
