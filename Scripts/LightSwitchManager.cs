using UnityEngine;

public class LightSwitchManager : MonoBehaviour
{
    public GameObject lightObject;
    public Animator doorAnim;

    public GameObject pianoMusic;

    public GameObject diceRoomText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lightObject.SetActive(true);
            doorAnim.SetBool("isOpen", false);
            pianoMusic.SetActive(false);

            diceRoomText.SetActive(true);

            gameObject.SetActive(false);
        }
    }

    
}
