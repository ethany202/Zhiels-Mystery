using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExamHandler : MonoBehaviour
{

    public SinglePlayerMove player;
    public Animator anim;
    public GameObject phoneObject;

    public void SetExamState(int examPhase)
    {
        if(examPhase == 2)
        {
            //anim.SetFloat(Animator.StringToHash("spawnState"), 1f);
            player.SetHealth(player.GetHealth() * 0.75f);
            phoneObject.GetComponentInChildren<Text>().text = "0*1**0000";
        }
    }

    private void ResetIdleState()
    {
        anim.SetFloat(Animator.StringToHash("spawnState"), 0f);
        Debug.LogError(anim.GetFloat("spawnState"));
    }


}
