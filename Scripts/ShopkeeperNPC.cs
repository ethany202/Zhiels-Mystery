using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopkeeperNPC : MonoBehaviour
{
    [Header("Scene Transition")]
    public GameObject sceneTransition;

    [Header("Animations")]
    public Animator animator;

    [Header("Dialogue Objects")]
    public GameObject dialogue;
    public TMP_Text dialogueText;

    public static string[] allDialogue = { "Hey, how can I help you?", "Oh? You're here to join the organization? That's certainly rare" };
    private int index = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadSceneLogic.DisplayInstructions(true);
            LoadSceneLogic.ChangeInstructionsText("E");
            StartCoroutine(ToggleDialogue(other));            
        }
    }

    private IEnumerator ToggleDialogue(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            other.GetComponent<SinglePlayerMove>().enabled = false;
            if (index >= allDialogue.Length)
            {
                dialogue.SetActive(false);
                LoadSceneLogic.DisplayInstructions(false);
                animator.SetBool(Animator.StringToHash("isTalking"), false);
                animator.SetFloat(Animator.StringToHash("idleState"), 1f);
                sceneTransition.SetActive(true);
                LoadSceneLogic.examPhase = 2;
                yield return new WaitForSecondsRealtime(2f);
                SceneManager.LoadSceneAsync(2);
            }
            else
            {
                dialogue.SetActive(true);
                animator.SetBool(Animator.StringToHash("isTalking"), true);
                dialogueText.text = allDialogue[index];
                index++;
                yield return new WaitForSecondsRealtime(1f);
            }
        }
    }
}
