using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KonbiniWorker : MonoBehaviour
{

    private AnimatorControllerParameter[] animations;

    public Transform destination;
    public NavMeshAgent self;
    public Animator anim;

    private Transform body;

    public bool atWork;
    public bool isBusy;
    public bool isIdle;

    public int blockShift; // 4 shifts: 12am - 6am, 6am - 12pm, 12 pm-6pm, 6pm - 12am

    public DayCycleController time;

    void Start()
    {
        body = GetComponent<Transform>();
        atWork = false;
        isBusy = false;
        animations = anim.parameters;
    }


    void Update()
    {
        CheckShift();
        IsAtWork();
        ChooseAction();
    }

    public void CheckShift()
    {
        if (time.timeOfDay >= (blockShift - 1) * 6 && time.timeOfDay <= (blockShift) * 6)
        {
            isBusy = true;

        }
    }

    public void IsAtWork()
    {
        if ((transform.position - destination.position).magnitude <= 0.5f)
        {
            atWork = true;
        }
        else
        {
            atWork = false;
        }
    }

    public void ChooseAction()
    {
        if (isBusy && !atWork)
        {
            Travel(destination);
        }
        else if(isBusy && atWork)
        {
            StopTraveling();
        }
        else
        {
            //
        }
    }

    public void Travel(Transform destination)
    {
        self.SetDestination(destination.position);
        anim.SetBool("isWalking", true);
    }

    public void StopTraveling()
    {
        self.SetDestination(transform.position);
        anim.SetBool("isIdle", true);
        anim.SetBool("isWalking", false);
        isIdle = true;
    }


}
