using System.Collections;
using UnityEngine;

public class TileController : MonoBehaviour
{

    public Transform[] walls;
    public RowInstaller rowInstaller;

    public bool isExit;
    public RadioController stage4Radio;

    private bool voicelinePlayed = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !rowInstaller.IsLockedIn())
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].GetComponent<Animator>().SetBool("isOpen", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && this.tag == "Exit")
        {
            walls[0].GetComponent<Animator>().SetBool("isOpen", false);

            if (!voicelinePlayed) 
            {
                voicelinePlayed = true;
                StartCoroutine(PlayRadio()); 
            }
            
        }
    }

    private IEnumerator PlayRadio()
    {
        yield return new WaitForSecondsRealtime(1f);
        stage4Radio.PlayVoiceLine();

        // yield return new WaitForSecondsRealtime(12.5f);
        // LoadSceneLogic.player.GetComponent<SnydorVoiceLines>().PlayStage4Clip();

    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && (!rowInstaller.IsLockedIn()))
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].GetComponent<Animator>().SetBool("isOpen", false);
            }
        }
        
    }*/
}
