using System.Collections;
using UnityEngine;

public class LightSwitchManager : MonoBehaviour
{
    public GameObject lightObject;
    public Animator doorAnim;

    public GameObject pianoMusic;

    public GameObject diceRoomText;

    public RadioController stage5Radio;
    public RadioController stage5ReturnClip;

    private bool voicelinePlayed=false;
    public bool stagePassed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (stagePassed)
            {
                StartCoroutine(stage5ReturnClip.PlayVoiceLineDelay(1f));
                stagePassed = false;
            }

            lightObject.SetActive(true);
            pianoMusic.SetActive(false);

            diceRoomText.SetActive(true);

            if (!voicelinePlayed)
            {
                doorAnim.SetBool("isOpen", false);
                voicelinePlayed = true;

                StartCoroutine(PlayRadio());
            }
            //gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayRadio()
    {
        yield return new WaitForSecondsRealtime(2f);
        stage5Radio.PlayVoiceLine();
    }


}
