using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalSceneNPC : MonoBehaviour
{

    public SnydorVoiceLines voiceManager;
    public NavMeshAgent agent;
    public Transform destination;

    public GameObject minion;

    public Animator anim;

    public AudioSource src;
    public AudioClip clip;

    private void Start()
    {
        agent.SetDestination(destination.position);
        anim.SetBool(AnimationParameters.parameters["isWalking"], true);

        Invoke("EnableMinion", 6f);
        StartCoroutine(PlayVoiceLine());
    }

    private void EnableMinion()
    {
        minion.SetActive(true);
    }
    
    private void Step()
    {
        src.PlayOneShot(clip);
    }

    private IEnumerator PlayVoiceLine()
    {
        yield return new WaitForSecondsRealtime(3.85f);
        voiceManager.PlayZhieltropolisClip();
    }
}
